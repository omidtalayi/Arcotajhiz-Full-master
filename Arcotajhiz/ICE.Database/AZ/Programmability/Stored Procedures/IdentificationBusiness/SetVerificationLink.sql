CREATE PROCEDURE [AZ].[SetVerificationLink](
	@IdentificationVCode BIGINT,
	@VerificationLink NVARCHAR(MAX)
)
AS
BEGIN
	BEGIN TRAN
	BEGIN TRY
		UPDATE AZ.[Identification] SET VerificationLink = @VerificationLink 
		WHERE VCode = @IdentificationVCode
			--AND IdentificationTypeVCode = 2
			AND IdentificationStateVCode = 1
		COMMIT TRAN
	END TRY
		BEGIN CATCH
		DECLARE @ErrNo INT,@ErrMsg NVARCHAR(4000),@ErrSev INT,@ErrStt INT
		SELECT @ErrMsg = '(' + CONVERT(NVARCHAR(4000),ERROR_NUMBER()) + ') ' + ERROR_MESSAGE(),@ErrSev = ERROR_SEVERITY(),@ErrStt = ERROR_STATE()
		IF @@TRANCOUNT > 0 ROLLBACK TRAN
		RAISERROR(@ErrMsg,@ErrSev,@ErrStt)
	END CATCH
END
