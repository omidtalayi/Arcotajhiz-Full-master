CREATE PROCEDURE AZ.GetFrontPageCount
(
	@PageTypeVCode INT = NULL
)
AS 
BEGIN
	
	DECLARE @cnt INT
	SET @cnt = 
	(
		SELECT CEILING(COUNT(*)/10.0)
		FROM AZ.Pages P LEFT JOIN AZ.PagesType PT ON P.PagesTypeVCode = PT.VCode
		WHERE	P.IsDeleted = 0 AND P.PagesTypeVCode = ISNULL(@PageTypeVCode,P.PagesTypeVCode)
	)
	
	SELECT @cnt AS frontPageCount
END
