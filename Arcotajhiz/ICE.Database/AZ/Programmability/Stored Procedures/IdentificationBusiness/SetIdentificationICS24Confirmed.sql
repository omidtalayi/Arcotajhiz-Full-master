CREATE PROCEDURE [AZ].[SetIdentificationICS24Confirmed](
	@IdentificationVCode BIGINT
)
AS
BEGIN
BEGIN TRAN
	BEGIN TRY
		UPDATE AZ.Identification SET IdentificationStateVCode = 18,LastModifiedDate = GETDATE() WHERE VCode = @IdentificationVCode
		IF NOT EXISTS(SELECT 1 FROM AZ.IdentificationHistory WHERE IdentificationVCode = @IdentificationVCode AND IdentificationStateVCode = 18)
		BEGIN
			INSERT AZ.IdentificationHistory(IdentificationVCode,IdentificationStateVCode) VALUES(@IdentificationVCode,18)
		END

		COMMIT TRAN
	END TRY
		BEGIN CATCH
		DECLARE @ErrNo INT,@ErrMsg NVARCHAR(4000),@ErrSev INT,@ErrStt INT
		SELECT @ErrMsg = '(' + CONVERT(NVARCHAR(4000),ERROR_NUMBER()) + ') ' + ERROR_MESSAGE(),@ErrSev = ERROR_SEVERITY(),@ErrStt = ERROR_STATE()
		IF @@TRANCOUNT > 0 ROLLBACK TRAN
		RAISERROR(@ErrMsg,@ErrSev,@ErrStt)
	END CATCH
END