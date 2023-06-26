CREATE PROCEDURE [AZ].[UserIns](
	@VCode INT OUTPUT,
	@Username NVARCHAR(100) = NULL,
	@Password NVARCHAR(100) = NULL,
	@PasswordSalt NVARCHAR(60) = NULL,
	@Email NVARCHAR(100) = NULL,
	@CellPhone NVARCHAR(20) = NULL,
	@IsSubscribed BIT = 1,
	@Name NVARCHAR(1000) = NULL,
	@UserTypeVCode SMALLINT = 2,
	@Apis XML = NULL
)
AS 
BEGIN
	BEGIN TRY 
    	DECLARE @IsApproved bit,
				@IsLock bit,
				@userCode VARCHAR(20)

		IF EXISTS(SELECT 1 FROM [AZ].[User] WHERE Username = @Username) 
		BEGIN
			SELECT 4 [State],'Username has already existed' [Message]
			RETURN 0
		END
		IF EXISTS(SELECT 1 FROM [AZ].[User] U WHERE U.CellPhone = @CellPhone AND VCode <> ISNULL(@VCode,0))
		BEGIN
			SELECT 5 [State],'CellPhone has already existed' [Message]
			RETURN 0
		END
		IF EXISTS(SELECT 1 FROM [AZ].[User] U WHERE CASE WHEN U.Email = '' THEN NULL ELSE U.Email END = '' AND VCode <> ISNULL(@VCode,0))
		BEGIN
			SELECT 3 [State],'Email has already existed' [Message]
			RETURN 0
		END

		IF LEN(@Username) < 5
		BEGIN
			SELECT 9 [State],'Username Is Weak' [Message]
			RETURN 0
		END

		BEGIN TRANSACTION

		IF @UserTypeVCode IS NULL
		BEGIN
			SET @UserTypeVCode = 2
		END

		SET @userCode = LEFT(CAST(NEWID() AS VARCHAR(MAX)),6)
		WHILE EXISTS(SELECT 1 FROM AZ.[User] WHERE Code = @userCode)
		BEGIN
			SET @userCode = LEFT(CAST(NEWID() AS VARCHAR(MAX)),6)
		END

		INSERT INTO [AZ].[User]([Username],[Password],[PasswordSalt],[IsApproved],[IsLock],[Code],[Email],[Cellphone],[IsSubscribed],[Name],[UserTypeVCode]) 
		VALUES(@Username,@Password,@PasswordSalt,1,0,@UserCode,@Email,@Cellphone,@IsSubscribed,@Name,@UserTypeVCode)

		SET @VCode = SCOPE_IDENTITY()

		INSERT [AZ].[UserHistory] (UserVCode,UserHistoryTypeVCode) VALUES (@VCode,3)

		INSERT [AZ].[UserSubSystem](UserVCode,SubSystemVCode) VALUES(@VCode,1)

		IF @UserTypeVCode = 1 
		BEGIN
			INSERT AZ.UserApi(UserVCode,ApiVCode) VALUES(@VCode,1)
			INSERT AZ.UserApi(UserVCode,ApiVCode) VALUES(@VCode,2)

			INSERT AZ.UserSubSystem(UserVCode,SubSystemVCode) VALUES(@VCode,1)
		
		END

		IF @UserTypeVCode = 2
		BEGIN
			INSERT AZ.UserApi(UserVCode,ApiVCode) VALUES(@VCode,1)
			INSERT AZ.UserApi(UserVCode,ApiVCode) VALUES(@VCode,2)

			IF @Apis IS NOT NULL
			BEGIN
				SELECT Field.value('@AV','INT') ApiVCode
				INTO #Apis FROM @Apis.nodes('A') Details(Field)

				INSERT AZ.UserApi(UserVCode,ApiVCode)
				SELECT @VCode,ApiVCode FROM #Apis

				DROP TABLE #Apis
			END
		END

		IF @UserTypeVCode = 3
		BEGIN
			INSERT AZ.UserRole(UserVCode,RoleVCode) VALUES(@VCode,1)

			INSERT AZ.UserApi(UserVCode,ApiVCode) VALUES(@VCode,1)
			INSERT AZ.UserApi(UserVCode,ApiVCode) VALUES(@VCode,2)
		END

		IF @IsLock = 1
		BEGIN
			INSERT [AZ].[UserHistory] (UserVCode,UserHistoryTypeVCode) VALUES (@VCode,14)
		END
		ELSE
		BEGIN
			INSERT [AZ].[UserHistory] (UserVCode,UserHistoryTypeVCode) VALUES (@VCode,15)
		END

		IF @IsApproved = 1
		BEGIN
			INSERT [AZ].[UserHistory] (UserVCode,UserHistoryTypeVCode) VALUES (@VCode,16)
		END
		
		COMMIT TRANSACTION
		
		SELECT 1 [State],'User has been successfully submitted' [Message]

	END TRY
	BEGIN CATCH
		DECLARE @ErrNo INT,@ErrMsg NVARCHAR(4000),@ErrSev INT,@ErrStt INT
		SELECT @ErrMsg = '(' + CONVERT(NVARCHAR(4000),ERROR_NUMBER()) + ') ' + ERROR_MESSAGE(),@ErrSev = ERROR_SEVERITY(),@ErrStt = ERROR_STATE()
		IF @@TRANCOUNT > 0 ROLLBACK TRAN
		RAISERROR(@ErrMsg,@ErrSev,@ErrStt)
	END CATCH
END
GO

