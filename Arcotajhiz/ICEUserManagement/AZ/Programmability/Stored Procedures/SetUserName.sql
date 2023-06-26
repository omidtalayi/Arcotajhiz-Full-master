CREATE PROCEDURE [AZ].[SetUserName](
	@UserVCode int,
	@NewUserName nvarchar(100)
) AS BEGIN
	BEGIN TRY 
		IF NOT EXISTS(SELECT 1 FROM [AZ].[User] WHERE VCode = @UserVCode) BEGIN
			SELECT 2 [State],'User is not found' [Message]
			RETURN 0
		END
		IF EXISTS(SELECT 1 FROM [AZ].[User] WHERE Username = @NewUserName) BEGIN
			SELECT 3 [State],'duplicate username' [Message]
			RETURN 0
		END
		BEGIN TRANSACTION


		UPDATE [AZ].[User] 
		SET [Username] = @NewUserName,
			LastModifiedDate = GETDATE()
		WHERE VCode = @UserVCode
		INSERT [AZ].[UserHistory] (UserVCode,UserHistoryTypeVCode) VALUES (@UserVCode,4)
		SELECT 1 [State],'UserName has been successfully modified' [Message]

		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		DECLARE @ErrNo INT,@ErrMsg NVARCHAR(4000),@ErrSev INT,@ErrStt INT
		SELECT @ErrMsg = '(' + CONVERT(NVARCHAR(4000),ERROR_NUMBER()) + ') ' + ERROR_MESSAGE(),@ErrSev = ERROR_SEVERITY(),@ErrStt = ERROR_STATE()
		IF @@TRANCOUNT > 0 ROLLBACK TRAN
		RAISERROR(@ErrMsg,@ErrSev,@ErrStt)
	END CATCH
END