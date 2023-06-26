CREATE PROCEDURE [AZ].[Admin_OnlinePaymentConfirmationIns](
	@ConfirmationDetail XML
)
AS 
BEGIN
    BEGIN TRAN
    BEGIN TRY
		SELECT Field.value('@ID','VARCHAR(100)') as RID,
			   Field.value('@PAN','VARCHAR(25)') as CPAN,
			   Field.value('@TID','VARCHAR(100)') as TID,
			   Field.value('@A','DECIMAL(18,0)') as Amount,
			   Field.value('@CJD','VARCHAR(100)') as CreatJD,
			   Field.value('@S','NVARCHAR(25)') as [State],
			   Field.value('@B','INT') as BankPortalVCode
		INTO #Data FROM @ConfirmationDetail.nodes('OPC') Details(Field) WHERE NOT EXISTS (SELECT 1 FROM AZ.OnlinePaymentConfirmation WHERE RefID = Field.value('@ID','VARCHAR(100)'))

		INSERT INTO AZ.OnlinePaymentConfirmation(RefID,CardPAN,TerminalID,Amount,ConfirmedJDate,[State],BankPortalVCode) 
			SELECT DISTINCT D.RID,D.CPAN,D.TID,D.Amount,CASE WHEN D.CreatJD = '0' THEN CASE WHEN IPT.EntryDate IS NULL THEN '' ELSE AZ.ConvertToJalaliDate(IPT.EntryDate) END ELSE D.CreatJD END,D.[State],D.BankPortalVCode 
			FROM #Data D LEFT JOIN AZ.IdentificationPayment IPT ON IPT.SaleRefID = D.RID

        UPDATE [AZ].[OnlinePaymentReceived] SET IsConfirmed = 1 FROM #Data WHERE SaleRefID = RID
		
		DROP TABLE #Data		

		COMMIT TRAN
	END TRY
	BEGIN CATCH
		DECLARE @ErrNo INT,@ErrMsg NVARCHAR(4000),@ErrSev INT,@ErrStt INT
		SELECT @ErrMsg = '(' + CONVERT(NVARCHAR(4000),ERROR_NUMBER()) + ') ' + ERROR_MESSAGE(),@ErrSev = ERROR_SEVERITY(),@ErrStt = ERROR_STATE()
		IF @@TRANCOUNT > 0 ROLLBACK TRAN
		RAISERROR(@ErrMsg,@ErrSev,@ErrStt)
	END CATCH
END