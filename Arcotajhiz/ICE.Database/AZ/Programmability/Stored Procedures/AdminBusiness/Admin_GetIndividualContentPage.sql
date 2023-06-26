CREATE PROCEDURE [AZ].[Admin_GetIndividualContentPage]
(
@VCode INT = NULL
)
AS
BEGIN
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
		P.Body,
		P.Keywords,
		P.Link,
		P.ImageLink,
		P.Entrydate,
		P.LastModifiedDate,
		P.TopicVCode,
		('<PagesImages>' + (SELECT * FROM AZ.PagesImage (NOLOCK) WHERE PagesVCode = P.VCode FOR XML PATH('PagesImage')) + '</PagesImages>') PagesImages
	FROM AZ.Pages P LEFT JOIN AZ.PagesType PT ON P.PagesTypeVCode = PT.VCode
	WHERE P.VCode = @VCode
END