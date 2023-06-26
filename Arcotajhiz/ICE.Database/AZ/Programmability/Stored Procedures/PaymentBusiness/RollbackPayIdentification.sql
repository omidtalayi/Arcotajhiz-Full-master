CREATE PROCEDURE [AZ].[RollbackPayIdentification](
	@PortalPaymentTypeVCode SMALLINT,
	@IdentificationVCode BIGINT
)
AS
BEGIN
	BEGIN TRAN
    BEGIN TRY
		IF @PortalPaymentTypeVCode = 1 
		BEGIN
			IF EXISTS(SELECT 1 FROM AZ.IdentificationHistory WHERE IdentificationVCode = @IdentificationVCode AND IdentificationStateVCode = 11)
			BEGIN
				UPDATE AZ.Identification SET IdentificationStateVCode = 3 WHERE VCode = @IdentificationVCode
				DELETE AZ.IdentificationHistory WHERE IdentificationVCode = @IdentificationVCode AND IdentificationStateVCode = 11
			END
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