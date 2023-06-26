CREATE PROCEDURE [AZ].[SetIdentificationReadyForSee](
	@IdentificationVCode BIGINT
)
AS
BEGIN
	BEGIN TRAN
	BEGIN TRY
		DECLARE @ICSUserVCode INT,
				@ReportPrice DECIMAL(18,0),
				@TaxPrice DECIMAL(18,0),
				@TotalPrice	DECIMAL(18,0),
				@SourceSharePercentage DECIMAL(18,10),
				@SourceShare DECIMAL(18,0),
				@ICSPaymentAmount DECIMAL(28,0),
				@IdentificationInvoiceVCode BIGINT,
				@IdentificationTypeVCode INT

		UPDATE AZ.[Identification] SET IdentificationStateVCode	= 13 WHERE VCode = @IdentificationVCode
		IF NOT EXISTS(SELECT 1 FROM AZ.IdentificationHistory WHERE IdentificationVCode = @IdentificationVCode AND IdentificationStateVCode = 13)
		BEGIN
			INSERT AZ.IdentificationHistory(IdentificationVCode,IdentificationStateVCode) VALUES(@IdentificationVCode,13)
		END

		IF EXISTS(SELECT 1 FROM AZ.Identification WHERE VCode = @IdentificationVCode AND UserPaymentTypeVCode = 2 /*پرداخت از اعتبار*/)
		BEGIN
			IF NOT EXISTS(SELECT 1 FROM AZ.IdentificationInvoice WHERE IdentificationVCode = @IdentificationVCode)
			BEGIN
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

				SET @ICSUserVCode = 59 /*ICS User*/
				SELECT @ICSPaymentAmount = @ReportPrice + @TaxPrice
				SELECT @SourceSharePercentage = SourceShare FROM AZ.ReportPriceShare WHERE UserVCode = 4
				SET @SourceShare = @ICSPaymentAmount * @SourceSharePercentage / 100

				INSERT AZ.IdentificationInvoice(IdentificationVCode,UserVCode,IdentificationInvoiceStateVCode,Amount,SourcePercentage)
				VALUES(@IdentificationVCode,@ICSUserVCode,1,@SourceShare,@SourceSharePercentage)

				SET @IdentificationInvoiceVCode = SCOPE_IDENTITY()

				INSERT AZ.IdentificationInvoiceHistory(IdentificationInvoiceVCode,IdentificationInvoiceStateVCode)
				VALUES(@IdentificationInvoiceVCode,1) 
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