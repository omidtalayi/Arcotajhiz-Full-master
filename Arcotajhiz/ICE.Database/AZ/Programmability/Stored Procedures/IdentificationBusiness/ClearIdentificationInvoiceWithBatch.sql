CREATE PROCEDURE [AZ].[ClearIdentificationInvoiceWithBatch](
	@TransferFileShebaVCode BIGINT,
	@TransferNumber NVARCHAR(100),
	@SettleDate VARCHAR(8),
	@SettleTime VARCHAR(6)
)
AS
BEGIN
	BEGIN TRAN
	BEGIN TRY

		IF EXISTS(SELECT 1 FROM AZ.TransferFileSheba WHERE VCode = @TransferFileShebaVCode AND StateVCode = 3)
		BEGIN
			UPDATE AZ.TransferFileSheba SET StateVCode = 4 WHERE VCode = @TransferFileShebaVCode

			UPDATE AZ.TransferFileShebaDetail 
			SET	TransferNumber = @TransferNumber,
				SettleDate = @SettleDate,
				SettleTime = REPLACE(@SettleTime,':','')
			WHERE TransferFileShebaVCode = @TransferFileShebaVCode

			SELECT IIB.VCode BatchVCode,II.VCode InvoiceVCode 
			INTO #Batches FROM AZ.IdentificationInvoiceBatch IIB
			INNER JOIN AZ.IdentificationInvoice II ON II.BatchVCode = IIB.VCode
			WHERE EXISTS (SELECT 1 FROM AZ.TransferFileShebaDetail WHERE TransferFileShebaVCode = @TransferFileShebaVCode AND VCode = IIB.TransferFileShebaDetailVCode)

			UPDATE AZ.IdentificationInvoice 
			SET IdentificationInvoiceStateVCode = 4 
			WHERE EXISTS(SELECT 1 FROM #Batches WHERE InvoiceVCode = AZ.IdentificationInvoice.VCode)

			INSERT AZ.IdentificationInvoiceHistory(IdentificationInvoiceVCode,IdentificationInvoiceStateVCode)
			SELECT DISTINCT InvoiceVCode,4 FROM #Batches

			UPDATE AZ.IdentificationInvoiceBatch
			SET StateVCode = 4 
			WHERE EXISTS(SELECT 1 FROM #Batches WHERE BatchVCode = AZ.IdentificationInvoiceBatch.VCode)

			INSERT AZ.IdentificationInvoiceBatchHistory(IdentificationInvoiceBatchVCode,StateVCode)
			SELECT DISTINCT BatchVCode,4 FROM #Batches

			DROP TABLE #Batches
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