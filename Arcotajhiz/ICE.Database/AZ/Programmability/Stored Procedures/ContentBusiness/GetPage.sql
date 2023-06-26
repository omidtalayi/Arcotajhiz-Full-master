CREATE PROCEDURE [AZ].[GetPage](
	@Id UNIQUEIDENTIFIER = NULL,
	@Link NVARCHAR(MAX) = NULL,
	@PageTypeVCode INT = NULL,
	@LastCommentedPage BIT = 0
)
AS
BEGIN
	Select P.*,
		('<PagesComments>' + (SELECT * FROM AZ.PagesComment (NOLOCK) WHERE Id = P.Id And ApprovalStateVCode = 2 ORDER BY Id DESC FOR XML PATH('PagesComment')) + '</PagesComments>') PagesComments,
		1 R
	From az.Pages P
	WHERE id = @id AND P.IsDeleted = 0
END

	--IF @LastCommentedPage = 0
	--BEGIN
	--	IF @VCode IS NOT NULL OR @Link IS NOT NULL
	--	BEGIN
	--		SELECT	*,
	--				('<PagesImages>' + (SELECT * FROM AZ.PagesImage (NOLOCK) WHERE PagesVCode = P.VCode FOR XML PATH('PagesImage')) + '</PagesImages>') PagesImages,
	--				('<PagesComments>' + (SELECT * FROM AZ.PagesComment (NOLOCK) WHERE PagesVCode = P.VCode And ApprovalStateVCode = 2 ORDER BY VCode DESC FOR XML PATH('PagesComment')) + '</PagesComments>') PagesComments,
	--				(SELECT COUNT(1) FROM AZ.PagesRate (NOLOCK) WHERE PagesVCode = P.VCode) RateCount,
	--				(SELECT ROUND(AVG(Rate),1) FROM AZ.PagesRate (NOLOCK) WHERE PagesVCode = P.VCode) Rate
	--		FROM AZ.Pages (NOLOCK) P
	--		WHERE P.id = ISNULL(@VCode,P.VCode) AND
	--			P.Link = ISNULL(@Link,P.Link) AND
	--			P.PagesTypeVCode = ISNULL(@PageTypeVCode,P.PagesTypeVCode) AND
	--			P.IsDeleted = 0
	--	END
	--END
	--ELSE 
	--BEGIN
	--	DECLARE @PagesVCode BIGINT
	--	--SET @PagesVCode = (SELECT TOP 1 PagesVCode FROM AZ.PagesComment WHERE ApprovalStateVCode = 2 ORDER BY PagesVCode DESC)
	--	Set @PagesVCode = (SELECT TOP 1 VCode FROM AZ.Pages WHERE PagesTypeVCode = 1 AND IsActivate = 1 AND IsDeleted = 0 ORDER BY VCode DESC)
	--	SELECT	*,
	--			('<PagesImages>' + (SELECT * FROM AZ.PagesImage (NOLOCK) WHERE PagesVCode = P.VCode FOR XML PATH('PagesImage')) + '</PagesImages>') PagesImages,
	--			('<PagesComments>' + (SELECT * FROM AZ.PagesComment (NOLOCK) WHERE PagesVCode = P.VCode And ApprovalStateVCode = 2 ORDER BY VCode DESC FOR XML PATH('PagesComment')) + '</PagesComments>') PagesComments,
	--			(SELECT COUNT(1) FROM AZ.PagesRate (NOLOCK) WHERE PagesVCode = P.VCode) RateCount,
	--			(SELECT ROUND(AVG(Rate),1) FROM AZ.PagesRate (NOLOCK) WHERE PagesVCode = P.VCode) Rate
	--	FROM AZ.Pages (NOLOCK) P
	--	WHERE P.VCode = @PagesVCode
	--END
--END
--GO