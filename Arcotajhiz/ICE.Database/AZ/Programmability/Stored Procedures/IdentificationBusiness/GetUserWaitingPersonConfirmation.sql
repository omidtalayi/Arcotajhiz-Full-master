CREATE PROCEDURE [AZ].[GetUserWaitingPersonConfirmation](
	@UserVCode INT
)
AS
BEGIN
	SELECT COUNT(1) FROM AZ.Identification (NOLOCK) 
	WHERE UserVCode = @UserVCode AND IdentificationStateVCode IN (17,11,18)
END