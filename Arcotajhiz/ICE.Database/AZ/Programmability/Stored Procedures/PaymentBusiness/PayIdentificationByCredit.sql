CREATE PROCEDURE [AZ].[PayIdentificationByCredit](
	@UserVCode INT,
	@IdentificationVCode BIGINT,
	@PayIdentificationByCreditState SMALLINT OUTPUT
)
AS
BEGIN
	BEGIN TRAN
    BEGIN TRY
		SET @PayIdentificationByCreditState = 0
		DECLARE @ReportPrice DECIMAL(18,0),
				@TaxPrice DECIMAL(18,0),
				@TotalPrice	DECIMAL(18,0),
				@PaymentAmount DECIMAL(28,0),
				@SourceSharePercentage DECIMAL(18,10),
				@SourceShare DECIMAL(18,0),
				@IdentificationInvoiceVCode BIGINT,
				@IdentificationTypeVCode INT,
				@ChequePrice DECIMAL(18,0) = 0,
				@HasChequeReport BIT = 0,
				@ChequePriceTax DECIMAL(18,0) = 0,
				@ICSPaymentAmount DECIMAL(28,0)

		IF EXISTS(SELECT 1 FROM AZ.IdentificationCheque WHERE IdentificationVCode = @IdentificationVCode)
		BEGIN
			SET @HasChequeReport = 1
		END

		CREATE TABLE #UserCredit( 
			Credit DECIMAL(18,0)
		)

		INSERT #UserCredit EXEC [AZ].[GetUserCredit] @UserVCode = @UserVCode

		SELECT @IdentificationTypeVCode = IdentificationTypeVCode FROM AZ.Identification WHERE VCode = @IdentificationVCode
		
		IF @IdentificationTypeVCode = 1
		BEGIN
			SELECT @ReportPrice = ISNULL(SUM(ISNULL(Price,0)),0) FROM AZ.PriceList (NOLOCK) WHERE VCode = 1
			SELECT @TaxPrice = ISNULL(SUM(ISNULL(Price,0)),0) FROM AZ.PriceList (NOLOCK) WHERE VCode = 2
		END
		IF @IdentificationTypeVCode = 2
		BEGIN
			SELECT @ReportPrice = ISNULL(SUM(ISNULL(Price,0)),0) FROM AZ.PriceList (NOLOCK) WHERE VCode = 3
			SELECT @TaxPrice = ISNULL(SUM(ISNULL(Price,0)),0) FROM AZ.PriceList (NOLOCK) WHERE VCode = 4
		END
		IF @HasChequeReport = 1
		BEGIN
			SELECT @ChequePrice = ISNULL(SUM(ISNULL(Price,0)),0) FROM AZ.PriceList (NOLOCK) WHERE VCode = 5
			SELECT @ChequePriceTax = ISNULL(SUM(ISNULL(Price,0)),0) FROM AZ.PriceList (NOLOCK) WHERE VCode = 6
		END

		SELECT @PaymentAmount = @ReportPrice + @TaxPrice + @ChequePrice + @ChequePriceTax
		SELECT @ICSPaymentAmount = @ReportPrice + @TaxPrice

		IF EXISTS(SELECT 1 FROM AZ.Identification WHERE VCode = @IdentificationVCode)
		BEGIN
			IF (SELECT ISNULL(Credit,0) FROM #UserCredit) < @PaymentAmount
			BEGIN
				SET @PayIdentificationByCreditState = 2	
			END
			ELSE
			BEGIN
				SET @PayIdentificationByCreditState = 1
				EXEC AZ.AccountingIns @UserVCode = @UserVCode,@AccountingTypeVCode = 2,@Bed = @PaymentAmount,@Bes = 0,@IdentificationVCode = @IdentificationVCode

				IF NOT EXISTS(SELECT 1 FROM AZ.IdentificationHistory WHERE IdentificationVCode = @IdentificationVCode AND IdentificationStateVCode = 11)
				BEGIN
					UPDATE AZ.[Identification] SET IdentificationStateVCode	= 11 WHERE VCode = @IdentificationVCode
					INSERT AZ.IdentificationHistory(IdentificationVCode,IdentificationStateVCode) VALUES(@IdentificationVCode,11)
				END

				--SELECT @SourceSharePercentage = SourceShare FROM AZ.ReportPriceShare WHERE UserVCode = 4
				--SET @SourceShare = @ICSPaymentAmount * @SourceSharePercentage / 100

				--IF NOT EXISTS(SELECT 1 FROM AZ.IdentificationInvoice WHERE IdentificationVCode = @IdentificationVCode)
				--BEGIN
				--	SET @UserVCode = 59 /*ICS User*/

				--	INSERT AZ.IdentificationInvoice(IdentificationVCode,UserVCode,IdentificationInvoiceStateVCode,Amount,SourcePercentage)
				--	VALUES(@IdentificationVCode,@UserVCode,1,@SourceShare,@SourceSharePercentage)

				--	SET @IdentificationInvoiceVCode = SCOPE_IDENTITY()

				--	INSERT AZ.IdentificationInvoiceHistory(IdentificationInvoiceVCode,IdentificationInvoiceStateVCode)
				--	VALUES(@IdentificationInvoiceVCode,1) 
				--END
			END
		END

		DROP TABLE #UserCredit

		COMMIT TRAN
	END TRY
	BEGIN CATCH
		DECLARE @ErrNo INT,@ErrMsg NVARCHAR(4000),@ErrSev INT,@ErrStt INT
		SELECT @ErrMsg = '(' + CONVERT(NVARCHAR(4000),ERROR_NUMBER()) + ') ' + ERROR_MESSAGE(),@ErrSev = ERROR_SEVERITY(),@ErrStt = ERROR_STATE()
		IF @@TRANCOUNT > 0 ROLLBACK TRAN
		RAISERROR(@ErrMsg,@ErrSev,@ErrStt)
	END CATCH
END