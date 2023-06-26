CREATE PROCEDURE [AZ].[GetPagesComments](
	@ApprovalStateVCode BIGINT = NULL,
	@PageVCode BIGINT = NULL
)
AS
BEGIN
	IF @ApprovalStateVCode IS NULL
	BEGIN
		SELECT * FROM AZ.PagesComment (NOLOCK) 
		--WHERE ApprovalStateVCode = 1 
		--OR (DATEDIFF(DAY,LastModifiedDate,GETDATE()) <= 3 AND
		--ISNULL(PagesVCode,0) =  ISNULL(@PageVCode,ISNULL(PagesVCode,0)))
		Order by VCode DESC
	END
	ELSE
	BEGIN
		SELECT * FROM AZ.PagesComment (NOLOCK) WHERE 
		ApprovalStateVCode = @ApprovalStateVCode AND
		ISNULL(PagesVCode,0) =  ISNULL(@PageVCode,ISNULL(PagesVCode,0))
		Order by VCode DESC
	END
END
GO