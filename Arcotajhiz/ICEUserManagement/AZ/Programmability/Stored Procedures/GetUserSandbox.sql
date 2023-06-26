CREATE PROCEDURE AZ.GetUserSandbox(
	@UserVCode INT = NULL,
	@Username NVARCHAR(100) = NULL,
	@CellPhone VARCHAR(20) = NULL,
	@Email VARCHAR(100) = NULL,
	@UserCode VARCHAR(20) = NULL,
	@FirmRegistrationVCode INT = NULL,
	@Token VARCHAR(1000) = NULL,
	@TokenState SMALLINT = 0 OUTPUT,
	@SubSystemVCode SMALLINT = 1,
	@TrackingCode NVARCHAR(500) = NULL,
	@DeviceId NVARCHAR(200) = NULL
)
AS
BEGIN
	EXEC ICEUserManagement_TEST.AZ.GetUser
		@UserVCode = @UserVCode,
		@Username = @Username,
		@CellPhone = @CellPhone,
		@Email = @Email,
		@UserCode = @UserCode,
		@FirmRegistrationVCode = @FirmRegistrationVCode,
		@Token = @Token,
		@TokenState = @TokenState,
		@SubSystemVCode = @SubSystemVCode,
		@TrackingCode = @TrackingCode,
		@DeviceId = @DeviceId
END
GO

