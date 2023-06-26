CREATE PROCEDURE [AZ].[CreateCredit](
	@UserVCode INT,
	@Amount Decimal,
	@OnlinePaymentVCode INT
)
AS
BEGIN
	BEGIN TRAN
    BEGIN TRY
		EXEC AZ.AccountingIns @UserVCode = @UserVCode,@AccountingTypeVCode = 1,@Bed = 0,@Bes = @Amount,@OnlinePaymentVCode = @OnlinePaymentVCode

		COMMIT TRAN
	END TRY
	BEGIN CATCH
		DECLARE @ErrNo INT,@ErrMsg NVARCHAR(4000),@ErrSev INT,@ErrStt INT
		SELECT @ErrMsg = '(' + CONVERT(NVARCHAR(4000),ERROR_NUMBER()) + ') ' + ERROR_MESSAGE(),@ErrSev = ERROR_SEVERITY(),@ErrStt = ERROR_STATE()
		IF @@TRANCOUNT > 0 ROLLBACK TRAN
		RAISERROR(@ErrMsg,@ErrSev,@ErrStt)
	END CATCH
END