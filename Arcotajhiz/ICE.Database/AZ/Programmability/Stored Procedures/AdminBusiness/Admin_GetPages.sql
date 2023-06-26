CREATE PROCEDURE [AZ].[Admin_GetPages]
(
	@PageSize INT = NULL,
	@PageNo INT = NULL,
	@RowCount INT = NULL
)
AS
BEGIN
	DECLARE @RowOffsetStatement NVARCHAR(MAX) = CONVERT(NVARCHAR(MAX),ISNULL((@PageNo - 1) * @PageSize,0))
	DECLARE @RowPageStatement NVARCHAR(MAX) = CASE WHEN @PageNo IS NOT NULL AND @PageSize IS NOT NULL THEN 'FETCH NEXT ' + CONVERT(NVARCHAR(MAX),@PageSize) + ' ROWS ONLY' ELSE '' END

	DECLARE @Statement NVARCHAR(MAX) = '
	SELECT 
		[RowCount] = COUNT(1) OVER(),
		ROW_NUMBER() OVER(ORDER BY P.VCode) [Row], 
		P.VCode,
		P.[Name],
		P.PagesTypeVCode,
		PT.[Name] PagesTypeName,
		P.Title,
		P.ImagePath,
		P.[Description],
		P.[Keywords],
		P.Body,
		P.Link,
		P.ImageLink,
		P.Entrydate,
		P.LastModifiedDate,
		P.TopicVCode,
		(''<PagesImages>'' + (SELECT * FROM AZ.PagesImage (NOLOCK) WHERE PagesVCode = P.VCode FOR XML PATH(''PagesImage'')) + ''</PagesImages>'') PagesImages
	FROM AZ.Pages P LEFT JOIN AZ.PagesType PT ON P.PagesTypeVCode = PT.VCode
	WHERE P.IsDeleted <> 1 
	ORDER BY EntryDate DESC
	OFFSET ' + @RowOffsetStatement + ' ROWS ' + @RowPageStatement 
	EXEC sp_executesql @stmt = @Statement
END
