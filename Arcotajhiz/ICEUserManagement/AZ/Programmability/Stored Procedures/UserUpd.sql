CREATE PROCEDURE [AZ].[UserUpd](
	@VCode INT OUTPUT,
	@Email NVARCHAR(100) = NULL,
	@CellPhone NVARCHAR(20) = NULL,
	@TrackingCode NVARCHAR(500) = NULL
) 
AS 
BEGIN
	BEGIN TRY 
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

		BEGIN TRANSACTION

		UPDATE AZ.[User]
		SET Email = ISNULL(@Email,Email),
			Cellphone = ISNULL(@Cellphone,Cellphone),
			TrackingCode = ISNULL(@TrackingCode,TrackingCode)
		WHERE VCode = @VCode

		COMMIT TRANSACTION
		
		SELECT 1 [State],'User has been successfully updated' [Message]

	END TRY
	BEGIN CATCH
		DECLARE @ErrNo INT,@ErrMsg NVARCHAR(4000),@ErrSev INT,@ErrStt INT
		SELECT @ErrMsg = '(' + CONVERT(NVARCHAR(4000),ERROR_NUMBER()) + ') ' + ERROR_MESSAGE(),@ErrSev = ERROR_SEVERITY(),@ErrStt = ERROR_STATE()
		IF @@TRANCOUNT > 0 ROLLBACK TRAN
		RAISERROR(@ErrMsg,@ErrSev,@ErrStt)
	END CATCH
END
GO

