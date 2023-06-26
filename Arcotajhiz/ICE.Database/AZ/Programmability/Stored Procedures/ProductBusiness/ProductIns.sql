CREATE PROCEDURE [AZ].[ProductIns](
	@name NVARCHAR(10) NULL, 
    @description NVARCHAR(MAX) NULL, 
    @image VARCHAR(MAX) NULL, 
    @price DECIMAL NULL, 
    @categoryID UNIQUEIDENTIFIER = NULL, 
    @isEnabled BIT = 1, 
    @isDeleted BIT = 0, 
    @isSpecialed BIT = 0, 
    @score DECIMAL = NULL,
	@productPropertiesXML XML = NULL
 )
AS
BEGIN
	BEGIN TRAN
	BEGIN TRY
		DECLARE @productId NVARCHAR(50)
		DECLARE @MyTableOutput TABLE
		(
			NewID UNIQUEIDENTIFIER NOT NULL
		);	
		
		INSERT AZ.Product([name],[description],[image],price,categoryID,isEnabled,isDeleted,isSpecialed,score)
		OUTPUT inserted.ID AS NewID
		INTO @MyTableOutput(NewID)
		VALUES(@name,@description,@image,@price,@categoryID,@isEnabled,@isDeleted,@isSpecialed,@score)
		
		IF @productPropertiesXML IS NOT NULL
		BEGIN
			SELECT	
					Field.value('@value','NVARCHAR(50)') [value],
					CONVERT(uniqueidentifier,(select NewID From @MyTableOutput)) productID,
					Field.value('@pID','uniqueidentifier') propertyID
			INTO #Data FROM @productPropertiesXML.nodes('PP') Details(Field)
			INSERT AZ.ProductProperty([value],productID,propertyID)
			SELECT * FROM #Data
		END

		Select P.*,
		('<ProductCategories>' + (SELECT C.* FROM AZ.Category C WHERE C.id = P.categoryID FOR XML PATH('ProductCategory')) + '</ProductCategories>' ) Category,
		('<ProductProperties>' + (SELECT *,(SELECT * FROM AZ.Property (NOLOCK) PR WHERE PR.id = propertyID FOR XML PATH('Property'),type) FROM AZ.ProductProperty NOLOCK WHERE productID = P.id FOR XML PATH('ProductProperty')) + '</ProductProperties>') ProductProperty
		from AZ.Product P  WHERE id = CONVERT(uniqueidentifier,(select NewID From @MyTableOutput))
		COMMIT TRAN
	END TRY
		BEGIN CATCH
		DECLARE @ErrNo INT,@ErrMsg NVARCHAR(4000),@ErrSev INT,@ErrStt INT
		SELECT @ErrMsg = '(' + CONVERT(NVARCHAR(4000),ERROR_NUMBER()) + ') ' + ERROR_MESSAGE(),@ErrSev = ERROR_SEVERITY(),@ErrStt = ERROR_STATE()
		IF @@TRANCOUNT > 0 ROLLBACK TRAN
		RAISERROR(@ErrMsg,@ErrSev,@ErrStt)
	END CATCH
END