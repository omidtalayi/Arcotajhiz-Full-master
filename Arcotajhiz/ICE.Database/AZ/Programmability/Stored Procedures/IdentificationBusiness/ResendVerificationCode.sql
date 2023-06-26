CREATE PROCEDURE [AZ].[ResendVerificationCode](
	@IdentificationVCode BIGINT,
	@VerificationCode VARCHAR(6) OUTPUT
)
AS
BEGIN
	BEGIN TRAN
	BEGIN TRY
		DECLARE @SMSCodeDuration INT = 2
		SET @VerificationCode = LEFT(CAST(ROUND(RAND() * 10000,0) AS VARCHAR(4)) + '0000',4)
		UPDATE AZ.Identification SET VerificationCode = @VerificationCode,ExpirationDate = DATEADD(MINUTE,@SMSCodeDuration,GETDATE()) WHERE VCode = @IdentificationVCode
	COMMIT TRAN
	END TRY
		BEGIN CATCH
		DECLARE @ErrNo INT,@ErrMsg NVARCHAR(4000),@ErrSev INT,@ErrStt INT
		SELECT @ErrMsg = '(' + CONVERT(NVARCHAR(4000),ERROR_NUMBER()) + ') ' + ERROR_MESSAGE(),@ErrSev = ERROR_SEVERITY(),@ErrStt = ERROR_STATE()
		IF @@TRANCOUNT > 0 ROLLBACK TRAN
		RAISERROR(@ErrMsg,@ErrSev,@ErrStt)
	END CATCH
END