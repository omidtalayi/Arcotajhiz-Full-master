CREATE PROCEDURE [AZ].[ReturnIdentificationCredit](
	@UserVCode INT,
	@IdentificationVCode BIGINT
)
AS
BEGIN
	BEGIN TRAN
    BEGIN TRY
		DECLARE @ReturnAmount DECIMAL(18,0)
		IF EXISTS(SELECT 1 FROM [$(ICEUserManagement)].AZ.[User] WHERE VCode = @UserVCode AND UserTypeVCode = 2)
		BEGIN
			IF EXISTS(SELECT 1 FROM AZ.Identification WHERE VCode = @IdentificationVCode AND (IdentificationStateVCode = 11 OR IdentificationStateVCode = 17))
			BEGIN
				SELECT @ReturnAmount = Bed FROM AZ.Accounting WHERE IdentificationVCode = @IdentificationVCode AND AccountingTypeVCode = 2
				IF @ReturnAmount > 0 AND NOT EXISTS(SELECT 1 FROM AZ.Accounting WHERE IdentificationVCode = @IdentificationVCode AND AccountingTypeVCode = 4)
				BEGIN
					EXEC AZ.AccountingIns @UserVCode = @UserVCode,@AccountingTypeVCode = 4,@Bed = 0,@Bes = @ReturnAmount,@IdentificationVCode = @IdentificationVCode

					IF NOT EXISTS(SELECT 1 FROM AZ.IdentificationHistory WHERE IdentificationVCode = @IdentificationVCode AND IdentificationStateVCode = 14)
					BEGIN
						UPDATE AZ.Identification SET IdentificationStateVCode = 14,LastModifiedDate = GETDATE() WHERE VCode = @IdentificationVCode
						INSERT AZ.IdentificationHistory(IdentificationVCode,IdentificationStateVCode) VALUES(@IdentificationVCode,14)
					END 

					IF EXISTS(SELECT 1 FROM AZ.IdentificationHistory WHERE IdentificationVCode = @IdentificationVCode AND IdentificationStateVCode = 11)
					BEGIN
						DELETE AZ.IdentificationHistory WHERE IdentificationVCode = @IdentificationVCode AND IdentificationStateVCode = 11 
					END

					IF EXISTS(SELECT 1 FROM AZ.IdentificationInvoice WHERE IdentificationVCode = @IdentificationVCode)
					BEGIN
						DELETE FROM AZ.IdentificationInvoiceHistory WHERE IdentificationInvoiceVCode IN (
							SELECT VCode FROM AZ.IdentificationInvoice WHERE IdentificationVCode = @IdentificationVCode
						)
						DELETE FROM AZ.IdentificationInvoice WHERE IdentificationVCode = @IdentificationVCode
					END
				END
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