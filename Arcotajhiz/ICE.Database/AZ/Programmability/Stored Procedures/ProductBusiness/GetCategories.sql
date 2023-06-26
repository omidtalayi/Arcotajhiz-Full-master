﻿CREATE PROCEDURE [AZ].[GetCategories](
		@PageSize INT = NULL,
		@PageNo INT = NULL,
		@RowCount INT = NULL
	)
	AS
	DECLARE @RowOffsetStatement NVARCHAR(MAX) = CONVERT(NVARCHAR(MAX),ISNULL((@PageNo - 1) * @PageSize,0))
		DECLARE @RowPageStatement NVARCHAR(MAX) = CASE WHEN @PageNo IS NOT NULL AND @PageSize IS NOT NULL THEN 'FETCH NEXT ' + CONVERT(NVARCHAR(MAX),@PageSize) + ' ROWS ONLY' ELSE '' END
	declare @w nvarchar(max) = ''
			DECLARE @Statement NVARCHAR(MAX) = '
			SELECT	ROW_NUMBER() OVER(ORDER BY P.id DESC) R,
					P.*,
					(''<Galleries>'' + (SELECT * FROM AZ.Gallery (NOLOCK) WHERE productID = P.id FOR XML PATH(''Gallery'')) + ''</Galleries>'') CategoryGallery
			INTO #T FROM AZ.Category (NOLOCK) P 
			ORDER BY P.id DESC
			OFFSET ' + @RowOffsetStatement + ' ROWS ' + @RowPageStatement + 
			' SELECT * FROM #T WHERE IsDeleted = 0 

			DROP TABLE #T'
	EXEC sp_executesql @stmt = @Statement