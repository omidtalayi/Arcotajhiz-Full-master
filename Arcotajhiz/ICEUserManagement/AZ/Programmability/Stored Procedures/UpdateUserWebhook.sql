CREATE PROCEDURE [AZ].[UpdateUserWebhook](
	@UserVCode BIGINT,
	@Webhook AS NVARCHAR(MAX)
)
AS
BEGIN
	IF NOT EXISTS(SELECT 1 FROM AZ.UserWebHook WHERE UserVCode = @UserVCode AND WebHookTypeVCode = 3)
	BEGIN
		INSERT AZ.UserWebHook(UserVCode,WebHook,WebHookTypeVCode) VALUES(@UserVCode,@Webhook,3)
	END
END