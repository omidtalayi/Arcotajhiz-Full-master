CREATE PROCEDURE [AZ].[GetUserRejected](
	@UserVCode INT
)
AS
BEGIN
	SELECT COUNT(1) FROM AZ.Identification (NOLOCK) 
	WHERE UserVCode = @UserVCode AND IdentificationStateVCode IN (7,14,15)
END