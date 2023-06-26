CREATE PROCEDURE [AZ].[LoginFailed](@UserVCode int) 
AS 
BEGIN
	BEGIN TRANSACTION
	BEGIN TRY
		DECLARE @FailedLoginAttemptCount INT 
		SELECT @FailedLoginAttemptCount = FailedLoginAttemptCount FROM AZ.[Application]

		IF EXISTS(SELECT 1 FROM [AZ].[User] WHERE FailedAttemptCount >= @FailedLoginAttemptCount) 
		BEGIN
			UPDATE [AZ].[User] SET IsLock = 1 WHERE VCode = @UserVCode
			INSERT [AZ].[UserHistory] (UserVCode,UserHistoryTypeVCode) VALUES (@UserVCode,14)
		END 
		ELSE 
		BEGIN
			UPDATE [AZ].[User] SET FailedAttemptCount = FailedAttemptCount + 1,LastFailedAttemptDate = GETDATE() WHERE VCode = @UserVCode
			INSERT [AZ].[UserHistory] (UserVCode,UserHistoryTypeVCode) VALUES (@UserVCode,2)
		END
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		DECLARE @ErrNo INT,@ErrMsg NVARCHAR(4000),@ErrSev INT,@ErrStt INT
		SELECT @ErrMsg = '(' + CONVERT(NVARCHAR(4000),ERROR_NUMBER()) + ') ' + ERROR_MESSAGE(),@ErrSev = ERROR_SEVERITY(),@ErrStt = ERROR_STATE()
		RAISERROR(@ErrMsg,@ErrSev,@ErrStt)
	END CATCH
END