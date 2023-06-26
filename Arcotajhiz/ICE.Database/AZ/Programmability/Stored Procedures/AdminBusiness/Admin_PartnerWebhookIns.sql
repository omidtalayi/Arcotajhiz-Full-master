CREATE PROCEDURE AZ.Admin_PartnerWebhookIns
(
	@UserVCode BIGINT,
	@WebHook NVARCHAR(MAX),
	@WebHookTypeVCode INT
)
AS
BEGIN
	INSERT INTO [$(ICEUserManagement)].AZ.UserWebHook(UserVCode,WebHookTypeVCode,WebHook,EntryDate)
	VALUES(@UserVCode,@WebHook,@WebHookTypeVCode,GETDATE())
END