CREATE PROCEDURE [AZ].[UserInsSandbox](
	@VCode INT OUTPUT,
	@Username NVARCHAR(100) = NULL,
	@Password NVARCHAR(100) = NULL,
	@PasswordSalt NVARCHAR(60) = NULL,
	@Email NVARCHAR(100) = NULL,
	@CellPhone NVARCHAR(20) = NULL,
	@IsSubscribed BIT = 1,
	@Name NVARCHAR(1000) = NULL,
	@FirmRegistrationVCode BIGINT = NULL,
	@UserTypeVCode SMALLINT = 2,
	@TrackingCode NVARCHAR(500) = NULL,
	@SourceShare DECIMAL(18,10) = NULL,
	@IceShare DECIMAL(18,10) = NULL,
	@PartnerShare DECIMAL(18,10) = NULL,
	@PartnerLogoUrl NVARCHAR(MAX) = NULL,
	@Apis XML = NULL,
	@Webhook NVARCHAR(MAX) = NULL
)
AS
BEGIN
	EXEC ICEUserManagement_TEST.[AZ].[UserIns]
		@VCode = @VCode OUTPUT,
		@Username = @Username,
		@Password = @Password,
		@PasswordSalt = @PasswordSalt,
		@Email = @Email,
		@CellPhone = @CellPhone,
		@IsSubscribed = @IsSubscribed,
		@Name = @Name,
		@FirmRegistrationVCode = @FirmRegistrationVCode,
		@UserTypeVCode = @UserTypeVCode,
		@TrackingCode = @TrackingCode,
		@SourceShare = @SourceShare,
		@IceShare = @IceShare,
		@PartnerShare = @PartnerShare,
		@PartnerLogoUrl = @PartnerLogoUrl,
		@Apis = @Apis,
		@Webhook = @Webhook
END