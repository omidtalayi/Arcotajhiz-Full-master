CREATE PROCEDURE [AZ].[GetFAQs](
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
		ROW_NUMBER() OVER(ORDER BY F.VCode) [Row], 
		F.VCode,
		F.Question,
		F.Answer,
		F.ImageLink,
		F.IsDeleted,
		F.Link,
		F.ImageLink,
		F.LastModifiedDate,
		F.EntryDate,
		F.LastModifiedDate,
		F.EntryUserVCode,
		F.TopicVCode
	FROM AZ.FAQ F
	WHERE	F.IsDeleted = 0
	ORDER BY VCode
	OFFSET ' + @RowOffsetStatement + ' ROWS ' + @RowPageStatement
	EXEC sp_executesql @stmt = @Statement
END
