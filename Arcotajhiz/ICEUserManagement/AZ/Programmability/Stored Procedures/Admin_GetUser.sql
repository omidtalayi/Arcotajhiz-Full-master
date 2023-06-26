CREATE PROCEDURE [AZ].[Admin_GetUser](
	@UserVCode INT = NULL,
	@Username NVARCHAR(100) = NULL,
	@CellPhone VARCHAR(20) = NULL,
	@Email VARCHAR(100) = NULL,
	@UserCode VARCHAR(20) = NULL,
	@Token VARCHAR(1000) = NULL,
	@TokenState SMALLINT = 0 OUTPUT,
	@SubSystemVCode SMALLINT = 1,
	@TrackingCode NVARCHAR(500) = NULL,
	@UserTypeVCode SMALLINT = NULL
)
AS BEGIN
	DECLARE @WhereCondition NVARCHAR(MAX) = 'WHERE 1 = 1'
	IF @UserVCode IS NOT NULL SET @WhereCondition = @WhereCondition + CAST('AND [User].VCode = @UserVCode' AS NVARCHAR(MAX))
	ELSE IF @Username IS NOT NULL SET @WhereCondition = @WhereCondition + CAST('AND LOWER([User].Username) LIKE LOWER(@Username)' AS NVARCHAR(MAX))
	ELSE IF @CellPhone IS NOT NULL SET @WhereCondition = @WhereCondition + CAST('AND RIGHT([User].CellPhone,10) LIKE RIGHT(@CellPhone,10)' AS NVARCHAR(MAX))
	ELSE IF @Email IS NOT NULL SET @WhereCondition = @WhereCondition + CAST('AND LOWER([User].Email) LIKE LOWER(@Email)' AS NVARCHAR(MAX))
	ELSE IF @UserCode IS NOT NULL SET @WhereCondition = @WhereCondition + CAST('AND LOWER([User].Code) LIKE LOWER(@UserCode)' AS NVARCHAR(MAX))
	ELSE IF @Token IS NOT NULL SET @WhereCondition = @WhereCondition + CAST('AND LOWER([UserToken].Token) LIKE LOWER(@Token)' AS NVARCHAR(MAX))
	ELSE IF @TrackingCode IS NOT NULL SET @WhereCondition = @WhereCondition + CAST('AND LOWER([User].TrackingCode) LIKE LOWER(@TrackingCode)' AS NVARCHAR(MAX))
	ELSE IF @UserTypeVCode IS NOT NULL SET @WhereCondition = @WhereCondition + CAST('AND LOWER([User].UserTypeVCode) LIKE LOWER(@UserTypeVCode)' AS NVARCHAR(MAX))

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
	SELECT	DISTINCT 
			[User].VCode UserVCode,
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
			[User].[TrackingCode],
			[User].[PartnerLogoUrl],
			NULL [VerificationCode],
			0 [VerificationCodeTryCount],
			GETDATE() [VerificationExpireDate],
			CASE WHEN EXISTS(SELECT TOP 1 * FROM ICE.AZ.RequestRegistration (NOLOCK) RR WHERE RR.FirmRegistrationVCode = [User].FirmRegistrationVCode AND RR.RequestRegistrationStateVCode = 2) THEN 1 ELSE 0 END HasConfirmedRequest,
			CASE WHEN EXISTS(SELECT TOP 1 * FROM ICE.AZ.Accounting (NOLOCK) A WHERE [User].VCode = A.UserVCode AND A.AccountingTypeVCode = 3) THEN 1 ELSE 0 END PresenterPaymentHasPaid,
			[UserToken].Token,
			[UserToken].TokenExpirationDate,
			[UserToken].SecretCode,
			BA.Credit,
			(''<APIs>'' + (SELECT * FROM AZ.Api (NOLOCK) A WHERE A.VCode IN (SELECT UA.ApiVCode FROM AZ.UserApi (NOLOCK) UA WHERE UA.UserVCode = [User].VCode) FOR XML PATH(''API'')) + ''</APIs>'') API,
			(''<WebHooks>'' + 
				(SELECT * FROM AZ.UserWebHook (NOLOCK) UWH WHERE UWH.UserVCode = [User].VCode FOR XML PATH(''WebHook'')) + 
			''</WebHooks>'') WebHook,
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
			''</Roles>'') Role,
			[User].[TrackingSendLink],
			[User].IsApiFree,
			[User].SelfOtp,
			[User].AvailableUserPaymentTypeVCode,
			[User].ApiExpirationDate
	FROM AZ.[User] (NOLOCK)
	OUTER APPLY (SELECT TOP 1 * FROM AZ.[UserToken] (NOLOCK) UT WHERE [User].VCode = UT.UserVCode ORDER BY UT.VCode DESC) [UserToken]
	LEFT JOIN AZ.[UserSubSystem] (NOLOCK) ON [User].VCode = [UserSubSystem].UserVCode
	OUTER APPLY 
	(
		SELECT ISNULL(SUM(Bes),0) - ISNULL(SUM(Bed),0) Credit 
		FROM ICE.AZ.[Accounting] (NOLOCK) 
		WHERE UserVCode = [User].VCode 
			AND CS = BINARY_CHECKSUM(VCode,UserVCode,IdentificationVCode,AccountingTypeVCode,Bed,Bes,OnlinePaymentVCode,[Description],ExpirationDate,EntryDate)
	) BA
		' AS NVARCHAR(MAX)) + @WhereCondition + CAST(' AND [User].IsLock = 0 AND ISNULL([UserSubSystem].SubSystemVCode,0) = ISNULL(ISNULL(@SubSystemVCode,[UserSubSystem].SubSystemVCode),0) ' AS NVARCHAR(MAX)) + CAST('
	ORDER BY EntryDate DESC ' AS NVARCHAR(MAX))

	EXEC sp_executesql @stmt = @Statement, @params = N'@UserVCode INT,@Username NVARCHAR(100),@CellPhone VARCHAR(20),@Email VARCHAR(100),@UserCode VARCHAR(20),@Token VARCHAR(1000),@SubSystemVCode SMALLINT,@TrackingCode NVARCHAR(500),@UserTypeVCode SMALLINT',
		@UserVCode = @UserVCode,@Username = @Username,@CellPhone = @CellPhone,@Email = @Email,@UserCode = @UserCode,@Token = @Token,@SubSystemVCode = @SubSystemVCode,@TrackingCode = @TrackingCode,@UserTypeVCode = @UserTypeVCode
END
GO

