CREATE PROCEDURE [AZ].[LoginSuccessful](
	@UserVCode INT
) 
AS 
BEGIN
	BEGIN TRANSACTION

	BEGIN TRY 
		UPDATE [AZ].[User] SET LastLoginDate = GETDATE(),FailedAttemptCount = 0,LastFailedAttemptDate = NULL WHERE VCode = @UserVCode
		INSERT [AZ].[UserHistory] (UserVCode,UserHistoryTypeVCode) VALUES (@UserVCode,1)
		COMMIT TRANSACTION
	END TRY

	BEGIN CATCH
		DECLARE @ErrNo INT,@ErrMsg NVARCHAR(4000),@ErrSev INT,@ErrStt INT
		SELECT @ErrMsg = '(' + CONVERT(NVARCHAR(4000),ERROR_NUMBER()) + ') ' + ERROR_MESSAGE(),@ErrSev = ERROR_SEVERITY(),@ErrStt = ERROR_STATE()
		RAISERROR(@ErrMsg,@ErrSev,@ErrStt)
	END CATCH
END