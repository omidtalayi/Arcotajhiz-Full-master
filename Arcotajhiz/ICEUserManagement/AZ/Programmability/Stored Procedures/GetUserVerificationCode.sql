CREATE PROCEDURE [AZ].[GetUserVerificationCode](
	@Cellphone VARCHAR(20)
)
AS
BEGIN
	BEGIN TRY
		DECLARE @VerificationCode NVARCHAR(4)
		IF @Cellphone = '' OR @Cellphone IS NULL BEGIN 
			SELECT 0 [State],'User not found' [Message],null VerificationCode
			RETURN 0
		END
		ELSE
		BEGIN
			IF NOT EXISTS(SELECT 1 FROM [AZ].[User] WHERE CellPhone = @Cellphone AND IsApproved = 1 AND IsLock = 0) BEGIN 
				SELECT 0 [State],'User not found' [Message],null VerificationCode
				RETURN 0
			END
			ELSE
			BEGIN
				DECLARE @UserVCode AS BIGINT
				SELECT @UserVCode = VCode FROM [AZ].[User] WHERE CellPhone = @Cellphone
				BEGIN TRANSACTION

				SET @VerificationCode = LEFT(CAST(ROUND(RAND() * 10000,0) AS VARCHAR(4)) + '0000',4)

				UPDATE [AZ].[User] SET VerificationCode = @VerificationCode,VerificationCodeTryCount = VerificationCodeTryCount + 1,VerificationExpireDate = DATEADD(MINUTE,5,GETDATE())  WHERE VCode = @UserVCode
				INSERT [AZ].[UserHistory] (UserVCode,UserHistoryTypeVCode) VALUES(@UserVCode,7)

				COMMIT TRANSACTION

				SELECT 1 [State],'verification code generated' [Message],@VerificationCode VerificationCode
			END
		END
	END TRY
	BEGIN CATCH
		DECLARE @ErrNo INT,@ErrMsg NVARCHAR(4000),@ErrSev INT,@ErrStt INT
		SELECT @ErrMsg = '(' + CONVERT(NVARCHAR(4000),ERROR_NUMBER()) + ') ' + ERROR_MESSAGE(),@ErrSev = ERROR_SEVERITY(),@ErrStt = ERROR_STATE()
		RAISERROR(@ErrMsg,@ErrSev,@ErrStt)
	END CATCH
END
GO

