CREATE PROCEDURE [AZ].[ClearIdentificationInvoice](
	@IdentificationVCode BIGINT,
	@IdentificationInvoiceVCode BIGINT
)
AS
BEGIN
	BEGIN TRAN
	BEGIN TRY
		
		DECLARE @BatchVCode BIGINT,
				@TransferFileShebaVCode BIGINT

		IF NOT EXISTS(SELECT 1 FROM AZ.IdentificationInvoice WHERE VCode = @IdentificationInvoiceVCode AND IdentificationVCode = @IdentificationVCode)
		BEGIN
			RAISERROR('(Suspicious) IdentificationVCode does not belong to this IdentificationInvoice',16,126)
		END

		IF EXISTS(SELECT 1 FROM AZ.IdentificationInvoice WHERE VCode = @IdentificationInvoiceVCode AND IdentificationInvoiceStateVCode = 3)
		BEGIN
			UPDATE AZ.IdentificationInvoice 
			SET IdentificationInvoiceStateVCode = 4 
			WHERE VCode = @IdentificationInvoiceVCode

			INSERT AZ.IdentificationInvoiceHistory(IdentificationInvoiceVCode,IdentificationInvoiceStateVCode)
			VALUES(@IdentificationInvoiceVCode,4) 

			SELECT @BatchVCode = BatchVCode FROM AZ.IdentificationInvoice WHERE VCode = @IdentificationInvoiceVCode 

			IF NOT EXISTS(SELECT 1 FROM AZ.IdentificationInvoice WHERE BatchVCode = @BatchVCode AND IdentificationInvoiceStateVCode = 3)
			BEGIN
				IF EXISTS(SELECT 1 FROM AZ.IdentificationInvoiceBatch WHERE VCode = @BatchVCode AND StateVCode = 3)
				BEGIN
					UPDATE AZ.IdentificationInvoiceBatch
					SET StateVCode = 4 
					WHERE VCode = @BatchVCode

					INSERT AZ.IdentificationInvoiceBatchHistory(IdentificationInvoiceBatchVCode,StateVCode)
					VALUES(@BatchVCode,4)
				END				
			END

			SET @TransferFileShebaVCode = (
				SELECT TOP 1 TransferFileShebaVCode FROM AZ.TransferFileShebaDetail 
				WHERE VCode IN (
					SELECT TransferFileShebaDetailVCode FROM AZ.IdentificationInvoiceBatch WHERE VCode = @BatchVCode
				)
			)

			UPDATE AZ.TransferFileSheba SET StateVCode = 4 
			WHERE  AZ.TransferFileSheba.VCode = @TransferFileShebaVCode  
				AND NOT EXISTS(
					SELECT 1 FROM AZ.TransferFileShebaDetail TFSD
					INNER JOIN AZ.IdentificationInvoiceBatch IIB ON IIB.TransferFileShebaDetailVCode = TFSD.VCode
					WHERE TFSD.TransferFileShebaVCode = AZ.TransferFileSheba.VCode AND IIB.StateVCode NOT IN (4)  
				)
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