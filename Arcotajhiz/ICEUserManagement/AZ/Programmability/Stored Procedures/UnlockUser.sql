CREATE PROCEDURE [AZ].[UnlockUser](
	@UserVCode INT
) 
AS 
BEGIN
	BEGIN TRY
		IF NOT EXISTS(SELECT 1 FROM [AZ].[User] WHERE VCode = @UserVCode) BEGIN 
			SELECT 2 [State],'User not found' [Message]
			RETURN 0
		END

		BEGIN TRANSACTION

		UPDATE [AZ].[User] SET IsLock = 'False',FailedAttemptCount = 0,LastFailedAttemptDate = NULL WHERE VCode = @UserVCode
		INSERT [AZ].[UserHistory] (UserVCode,UserHistoryTypeVCode) VALUES(@UserVCode,15)

		COMMIT TRANSACTION

		SELECT 1 [State],'User has been unlocked' [Message]
	END TRY
	BEGIN CATCH
		DECLARE @ErrNo INT,@ErrMsg NVARCHAR(4000),@ErrSev INT,@ErrStt INT
		SELECT @ErrMsg = '(' + CONVERT(NVARCHAR(4000),ERROR_NUMBER()) + ') ' + ERROR_MESSAGE(),@ErrSev = ERROR_SEVERITY(),@ErrStt = ERROR_STATE()
		RAISERROR(@ErrMsg,@ErrSev,@ErrStt)
	END CATCH
END