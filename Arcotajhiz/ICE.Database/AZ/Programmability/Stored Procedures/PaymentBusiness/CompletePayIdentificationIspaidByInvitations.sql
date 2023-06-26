CREATE PROCEDURE [AZ].[CompletePayIdentificationIsPaidByInvitations](
	@IdentificationVCode BIGINT
)
AS
BEGIN
	BEGIN TRAN
    BEGIN TRY
		DECLARE @PaymentAmount DECIMAL,
				@SourceShare DECIMAL(18,0),
				@SourceSharePercentage DECIMAL(18,10),
				@ICSUserVCode BIGINT,
				@IdentificationTypeVCode INT = 1,
				@NationalCode NVARCHAR(10),
				@ReportPrice DECIMAL(18,0),
				@TaxPrice DECIMAL(18,0),
				@ChequePrice DECIMAL(18,0) = 0,
				@HasChequeReport BIT = 0,
				@ChequePriceTax DECIMAL(18,0) = 0,
				@IdentificationInvoiceVCode BIGINT

		SELECT @NationalCode = NationalCode,@IdentificationTypeVCode = IdentificationTypeVCode FROM AZ.Identification WHERE VCode = @IdentificationVCode

		CREATE TABLE #IsFreeByInvitations(
			CntPaidInvitation INT,
			CntUsedPayInvitation INT,
			PaymentIsFree BIT
		)

		INSERT #IsFreeByInvitations EXEC [AZ].[IsFreeByInvitations] @NationalCode = @NationalCode
		IF EXISTS(SELECT 1 FROM #IsFreeByInvitations WHERE PaymentIsFree = 0)
		BEGIN
			RAISERROR('User has used its free report',16,126) 
		END

		IF EXISTS(SELECT 1 FROM AZ.IdentificationCheque WHERE IdentificationVCode = @IdentificationVCode)
		BEGIN
			SET @HasChequeReport = 1
		END

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
		SELECT @PaymentAmount = @ReportPrice + @TaxPrice

		UPDATE AZ.[Identification] SET IdentificationStateVCode = 11,IsPaidByInvitations = 1 WHERE VCode = @IdentificationVCode
		INSERT AZ.IdentificationHistory(IdentificationVCode,IdentificationStateVCode) VALUES(@IdentificationVCode,11)

		SELECT	@SourceSharePercentage = SourceShare
		FROM AZ.ReportPriceShare 
		WHERE UserVCode = 4

		SET @SourceShare = @PaymentAmount * @SourceSharePercentage / 100

		IF NOT EXISTS(SELECT 1 FROM AZ.IdentificationInvoice WHERE IdentificationVCode = @IdentificationVCode)
		BEGIN
			SET @ICSUserVCode = 59 /*ICS User*/

			INSERT AZ.IdentificationInvoice(IdentificationVCode,UserVCode,IdentificationInvoiceStateVCode,Amount,SourcePercentage)
			VALUES(@IdentificationVCode,@ICSUserVCode,1,@SourceShare,@SourceSharePercentage)

			SET @IdentificationInvoiceVCode = SCOPE_IDENTITY()

			INSERT AZ.IdentificationInvoiceHistory(IdentificationInvoiceVCode,IdentificationInvoiceStateVCode)
			VALUES(@IdentificationInvoiceVCode,1) 
		END

		DROP TABLE #IsFreeByInvitations

		COMMIT TRAN
	END TRY
	BEGIN CATCH
		DECLARE @ErrNo INT,@ErrMsg NVARCHAR(4000),@ErrSev INT,@ErrStt INT
		SELECT @ErrMsg = '(' + CONVERT(NVARCHAR(4000),ERROR_NUMBER()) + ') ' + ERROR_MESSAGE(),@ErrSev = ERROR_SEVERITY(),@ErrStt = ERROR_STATE()
		IF @@TRANCOUNT > 0 ROLLBACK TRAN
		RAISERROR(@ErrMsg,@ErrSev,@ErrStt)
		RETURN 0
	END CATCH
END