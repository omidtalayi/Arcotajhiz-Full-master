CREATE PROCEDURE AZ.Admin_PartnerApiIns
(
	@UserVCode BIGINT,
	@ApiVCode INT
)
AS
BEGIN
	INSERT INTO [$(ICEUserManagement)].AZ.UserApi(UserVCode,ApiVCode,EntryDate)
	VALUES(@UserVCode,@ApiVCode,GETDATE())
END