CREATE PROCEDURE [AZ].[GetUserConfirmed](
	@UserVCode INT
)
AS
BEGIN
	SELECT COUNT(1) FROM AZ.Identification (NOLOCK) 
	WHERE UserVCode = @UserVCode AND IdentificationStateVCode IN (4,13)
END