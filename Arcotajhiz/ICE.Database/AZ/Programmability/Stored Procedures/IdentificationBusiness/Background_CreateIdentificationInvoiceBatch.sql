CREATE PROCEDURE [AZ].[Background_CreateIdentificationInvoiceBatch]
AS
BEGIN
	BEGIN TRY
		DECLARE @BatchValueThreshold INT = 1000,
				@Yesterday CHAR(8)
		DECLARE @IdentificationInvoices TABLE (VCode BIGINT PRIMARY KEY,IVCode BIGINT,UVCode INT,Share DECIMAL(28,0))

		SET @Yesterday = AZ.ConvertToJalaliDate(DATEADD(DAY,-1,GETDATE()))

		INSERT @IdentificationInvoices(VCode,IVCode,UVCode,Share)
			SELECT DISTINCT II.VCode,I.VCode,II.UserVCode,II.Amount FROM AZ.IdentificationInvoice II
			INNER JOIN AZ.Identification I ON II.IdentificationVCode = I.VCode AND II.IdentificationInvoiceStateVCode = 1
			INNER JOIN [$(ICEUserManagement)].AZ.[User] U ON II.UserVCode = U.VCode
			WHERE CAST(II.EntryDate AS DATE) = CAST(DATEADD(DAY,-1,GETDATE()) AS DATE)
		
		DELETE @IdentificationInvoices 
		WHERE UVCode IN (SELECT UVCode FROM @IdentificationInvoices GROUP BY UVCode HAVING (SUM(Share) < @BatchValueThreshold) AND (SUM(Share) > 0))

		IF EXISTS(SELECT 1 FROM @IdentificationInvoices) 
		BEGIN
			BEGIN TRAN

			INSERT AZ.IdentificationInvoiceBatch(JDate,UserVCode,StateVCode,TotalShare) 
			SELECT @Yesterday,UVCode,2,SUM(Share) FROM @IdentificationInvoices GROUP BY UVCode
			
			INSERT AZ.IdentificationInvoiceBatchHistory(IdentificationInvoiceBatchVCode,StateVCode) 
			SELECT DISTINCT IIB.VCode,2 FROM @IdentificationInvoices II
			INNER JOIN AZ.IdentificationInvoiceBatch IIB ON IIB.UserVCode = II.UVCode AND IIB.JDate = @Yesterday AND IIB.StateVCode = 2

			UPDATE AZ.IdentificationInvoice SET IdentificationInvoiceStateVCode = 2 ,BatchVCode = IIB.VCode
			FROM @IdentificationInvoices II 
			INNER JOIN AZ.IdentificationInvoiceBatch IIB ON IIB.UserVCode = II.UVCode AND IIB.JDate = @Yesterday AND IIB.StateVCode = 2
			WHERE II.VCode = AZ.IdentificationInvoice.VCode 

			INSERT AZ.IdentificationInvoiceHistory(IdentificationInvoiceVCode,IdentificationInvoiceStateVCode)
			SELECT VCode,2 FROM @IdentificationInvoices

			COMMIT TRAN
			RETURN 1
		END

		RETURN 0
	END TRY
	BEGIN CATCH
		DECLARE @ErrNo INT,@ErrMsg NVARCHAR(4000),@ErrSev INT,@ErrStt INT
		SELECT @ErrMsg = '(' + CONVERT(NVARCHAR(4000),ERROR_NUMBER()) + ') ' + ERROR_MESSAGE(),@ErrSev = ERROR_SEVERITY(),@ErrStt = ERROR_STATE()
		IF @@TRANCOUNT > 0 ROLLBACK TRAN
		RAISERROR(@ErrMsg,@ErrSev,@ErrStt)
	END CATCH
END
GO

