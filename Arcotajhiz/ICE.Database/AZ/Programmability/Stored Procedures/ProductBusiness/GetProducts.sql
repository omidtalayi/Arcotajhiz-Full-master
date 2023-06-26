CREATE PROCEDURE [AZ].[GetProducts](
		@Name NVARCHAR(50) = NULL,
		@PageSize INT = NULL,
		@PageNo INT = NULL,
		@RowCount INT = NULL,
		@CategoryId UNIQUEIDENTIFIER = null,
		@ProductProperties XML = NULL
	)
	AS
	DECLARE @RowOffsetStatement NVARCHAR(MAX) = CONVERT(NVARCHAR(MAX),ISNULL((@PageNo - 1) * @PageSize,0))
		DECLARE @RowPageStatement NVARCHAR(MAX) = CASE WHEN @PageNo IS NOT NULL AND @PageSize IS NOT NULL THEN 'FETCH NEXT ' + CONVERT(NVARCHAR(MAX),@PageSize) + ' ROWS ONLY ' ELSE '' END
	declare @w nvarchar(max) = ''
			DECLARE @Statement NVARCHAR(MAX) = '
			SELECT	ROW_NUMBER() OVER(ORDER BY P.id DESC) R,
					P.*,
					(''<ProductCategories>'' + (SELECT * FROM AZ.Category NOLOCK WHERE Id = P.categoryId FOR XML PATH(''ProductCategory'')) + ''</ProductCategories>'') Category,
					(''<ProductProperties>'' + (SELECT *,(SELECT * FROM AZ.Property (NOLOCK) PR WHERE PR.id = propertyID FOR XML PATH(''Property''),type) FROM AZ.ProductProperty NOLOCK WHERE productID = P.id FOR XML PATH(''ProductProperty'')) + ''</ProductProperties>'') ProductProperty,
					(''<Galleries>'' + (SELECT * FROM AZ.Gallery (NOLOCK) WHERE productID = P.id FOR XML PATH(''Gallery'')) + ''</Galleries>'') ProductGallery,
					COUNT(1) OVER() [RowCount]
			INTO #T FROM AZ.Product (NOLOCK) P '
			
			if( @Name is not null)
			BEGIN
				set @w = 'WHERE name like N''%' + @Name + '%'''
				if( @CategoryId is not null)
					set @w += ' AND categoryID = ''' + convert(nvarchar(36),@CategoryId) +''''
			END
			ELSE
			BEGIN
				if( @CategoryId is not null)
				BEGIN
					set @w = 'WHERE  P.categoryID = @CategoryId OR  P.categoryID in (SELECT id from az.Category WHERE parentId = ''' + convert(nvarchar(36),@CategoryId) +''')'
					
				END
			END
			set @Statement += @w + ' ORDER BY P.id DESC
			OFFSET ' + @RowOffsetStatement + ' ROWS ' + @RowPageStatement
			

			IF @ProductProperties is not null
			BEGIN
				set @Statement += 'SELECT	
						Field.value(N''@value'',''NVARCHAR(50)'') [value],
						Field.value(''@pID'',''uniqueidentifier'') propertyID
				INTO #T1 FROM @ProductProperties.nodes(''PP'') Details(Field)
				SELECT * FROM #T as Pr
				INNER JOIN AZ.ProductProperty PP On PP.productId = Pr.id
				WHERE PP.Id in (SELECT propertyID From #T1) AND Pr.isDeleted = 0
				DROP TABLE #T
				DROP TABLE #T1
				'
			END
			ELSE
			BEGIN
				set @Statement += ' SELECT * FROM #T WHERE isDeleted = 0'
				set @Statement += 'DROP TABLE #T'
			END
			print @Statement
		
	EXEC sp_executesql @stmt = @Statement,@params = N'@Name VARCHAR(50),@CategoryId UNIQUEIDENTIFIER,@ProductProperties XML',	@Name = @Name,@CategoryId=@CategoryId,@ProductProperties = @ProductProperties