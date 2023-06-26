CREATE PROCEDURE [AZ].[GetPages](
	@PageSize INT = NULL,
	@PageNo INT = NULL,
	@PageTypeId INT = NULL
)
AS
BEGIN
	--		(SELECT ROUND(AVG(Rate),1) FROM AZ.PagesRate (NOLOCK) WHERE PagesVCode = P.id) Rate
	DECLARE @RowOffsetStatement NVARCHAR(MAX) = CONVERT(NVARCHAR(MAX),ISNULL((@PageNo - 1) * @PageSize,0))
	DECLARE @RowPageStatement NVARCHAR(MAX) = CASE WHEN @PageNo IS NOT NULL AND @PageSize IS NOT NULL THEN 'FETCH NEXT ' + CONVERT(NVARCHAR(MAX),@PageSize) + ' ROWS ONLY' ELSE '' END
	DECLARE @w nvarchar(max) = ''

	DECLARE @Statement NVARCHAR(MAX) = '
	SELECT 
		[RowCount] = COUNT(1) OVER(),
		ROW_NUMBER() OVER(ORDER BY P.id) [Row], 
		P.Id,
		P.[Name],
		P.PagesTypeId,
		PT.[Name] PagesTypeName,
		P.Title,
		P.[Description],
		P.Body,
		P.Keywords,
		P.Link,
		P.Image,
		P.Entrydate,
		P.LastModifiedDate,
		(''<PagesComments>'' + (SELECT * FROM AZ.PagesComment (NOLOCK) WHERE PagesId = P.Id AND ApprovalStateVCode = 2 ORDER BY Id DESC FOR XML PATH(''PagesComment'')) + ''</PagesComments>'') PagesComments
	FROM AZ.Pages P LEFT JOIN AZ.PagesType PT ON P.PagesTypeId = PT.Id
	WHERE	P.IsDeleted = 0
	AND P.PagesTypeId = @PageTypeId AND P.IsDeleted = 0'

	set @Statement += @w +  ' ORDER BY EntryDate DESC
	OFFSET ' + @RowOffsetStatement + ' ROWS ' + @RowPageStatement 
	EXEC sp_executesql @stmt = @Statement,@params = N'@PageTypeId AS INT',@PageTypeId = @PageTypeId
END
GO