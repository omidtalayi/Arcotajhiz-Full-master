CREATE PROCEDURE AZ.Admin_GetUserWebhook
(
	@UserVCode INT = NULL
)
AS
BEGIN
	SELECT * FROM [$(ICEUserManagement)].AZ.UserWebHook A
	WHERE A.UserVCode = ISNULL(@UserVCode,A.UserVCode)
END
