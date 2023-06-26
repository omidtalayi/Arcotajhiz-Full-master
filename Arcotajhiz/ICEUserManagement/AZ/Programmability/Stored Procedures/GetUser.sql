CREATE PROCEDURE [AZ].[GetUser](
	@UserVCode INT = NULL,
	@Username NVARCHAR(100) = NULL,
	@CellPhone VARCHAR(20) = NULL,
	@Email VARCHAR(100) = NULL,
	@UserCode VARCHAR(20) = NULL,
	@Token VARCHAR(1000) = NULL,
	@TokenState SMALLINT = 0 OUTPUT,
	@SubSystemVCode SMALLINT = 1,
	@DeviceId NVARCHAR(200) = NULL
)
AS BEGIN
	DECLARE @WhereCondition NVARCHAR(MAX) = ''
	IF @UserVCode IS NOT NULL SET @WhereCondition = CAST('WHERE [User].VCode = @UserVCode' AS NVARCHAR(MAX))
	ELSE IF @Username IS NOT NULL SET @WhereCondition = CAST('WHERE LOWER([User].Username) LIKE LOWER(@Username)' AS NVARCHAR(MAX))
	ELSE IF @CellPhone IS NOT NULL SET @WhereCondition = CAST('WHERE RIGHT([User].CellPhone,10) LIKE RIGHT(@CellPhone,10)' AS NVARCHAR(MAX))
	ELSE IF @Email IS NOT NULL SET @WhereCondition = CAST('WHERE LOWER([User].Email) LIKE LOWER(@Email)' AS NVARCHAR(MAX))
	ELSE IF @UserCode IS NOT NULL SET @WhereCondition = CAST('WHERE LOWER([User].Code) LIKE LOWER(@UserCode)' AS NVARCHAR(MAX))
	ELSE IF @Token IS NOT NULL SET @WhereCondition = CAST('WHERE LOWER([UserToken].Token) LIKE LOWER(@Token)' AS NVARCHAR(MAX))

	IF @UserVCode IS NULL
	BEGIN
		SELECT @UserVCode = VCode FROM Az.[User] (NOLOCK) WHERE Username = @UserName
	END

	IF @Token IS NOT NULL AND NOT EXISTS(SELECT 1 FROM AZ.UserToken (NOLOCK) WHERE Token = @Token)
	BEGIN
		SET @TokenState = 2
	END
	ELSE
	BEGIN
		IF @Token IS NOT NULL AND EXISTS(SELECT 1 FROM AZ.UserToken (NOLOCK) WHERE Token = @Token AND TokenExpirationDate < GETDATE())
		BEGIN
			SET @TokenState = 3
		END
		ELSE
		BEGIN
			SET @TokenState = 1
		END
	END

	DECLARE @Statement NVARCHAR(MAX) = CAST('
	SELECT	[User].VCode UserVCode,
			[User].Username,
			[User].CellPhone,
			[User].Email,
			[User].Password,
			[User].PasswordSalt,
			[User].IsApproved,
			[User].IsLock,
			[User].LastLoginDate,
			[User].LastFailedAttemptDate,
			[User].FailedAttemptCount,
			[User].EntryDate,
			[User].LastModifiedDate,
			[User].SendLinkUrlSms,
			[User].IsSubscribed,
			[User].TokenExpirationTime,
			[User].UserTypeVCode,
			[User].UserPaymentTypeVCode,
			[User].[Name],
			[User].[FirmRegistrationVCode],
			[User].[DocApproved],
			[User].VerificationCode,
			[User].VerificationExpireDate,
			[User].VerificationCodeTryCount,
			[UserToken].Token,
			[UserToken].TokenExpirationDate,
			[UserToken].SecretCode,
			(''<APIs>'' + (SELECT * FROM AZ.Api (NOLOCK) A WHERE A.VCode IN (SELECT UA.ApiVCode FROM AZ.UserApi (NOLOCK) UA WHERE UA.UserVCode = [User].VCode) FOR XML PATH(''API'')) + ''</APIs>'') API,
			(''<Roles>'' + 
				(
					SELECT	UR.VCode,
							UR.UserVCode,
							UR.RoleVCode,
							R.SubSystemVCode,
							UR.EntryDate
					FROM AZ.UserRole (NOLOCK) UR 
					INNER JOIN AZ.[Role] (NOLOCK) R ON R.VCode = UR.RoleVCode 
					WHERE UR.UserVCode = [User].VCode FOR XML PATH(''Role'')
				) + 
			''</Roles>'') Role
	FROM AZ.[User] (NOLOCK)
	LEFT JOIN AZ.[UserToken] (NOLOCK) ON [User].VCode = [UserToken].UserVCode AND ISNULL(@DeviceId,0) = ISNULL([UserToken].DeviceId,0)
	LEFT JOIN AZ.[UserSubSystem] (NOLOCK) ON [User].VCode = [UserSubSystem].UserVCode' AS NVARCHAR(MAX))

	EXEC sp_executesql @stmt = @Statement, @params = N'@UserVCode INT,@Username NVARCHAR(100),@CellPhone VARCHAR(20),@Email VARCHAR(100),@UserCode VARCHAR(20),@Token VARCHAR(1000),@SubSystemVCode SMALLINT,@DeviceId NVARCHAR(200)',
		@UserVCode = @UserVCode,@Username = @Username,@CellPhone = @CellPhone,@Email = @Email,@UserCode = @UserCode,@Token = @Token,@SubSystemVCode = @SubSystemVCode,@DeviceId = @DeviceId
END
GO

