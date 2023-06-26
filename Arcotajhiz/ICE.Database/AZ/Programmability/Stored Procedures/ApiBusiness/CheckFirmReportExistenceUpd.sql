CREATE PROCEDURE [AZ].[CheckFirmReportExistenceUpd](
	@UserVCode								BIGINT,
	@CheckFirmReportExistenceVCode			BIGINT,
	@ReportSourceVCode						BIGINT,
	@HasReport								BIT,
	@ReportPrice							DECIMAL(18,0) OUTPUT,
	@TaxPrice								DECIMAL(18,0) OUTPUT,
	@TotalPrice								DECIMAL(18,0) OUTPUT,
	@SourceShare							DECIMAL(18,0) OUTPUT,
	@IceShare								DECIMAL(18,0) OUTPUT,
	@PartnerShare							DECIMAL(18,0) OUTPUT
)
AS
BEGIN
	BEGIN TRY
		DECLARE @SourceSharePercentage DECIMAL(18,10),
				@IceSharePercentage DECIMAL(18,10),
				@PartnerSharePercentage DECIMAL(18,10)

		SELECT @ReportPrice = ISNULL(SUM(ISNULL(Price,0)),0) FROM AZ.PriceList (NOLOCK) WHERE VCode = 3
		SELECT @TaxPrice = ISNULL(SUM(ISNULL(Price,0)),0) FROM AZ.PriceList (NOLOCK) WHERE VCode = 4
		SELECT @TotalPrice = @ReportPrice + @TaxPrice

		SELECT	@SourceSharePercentage = SourceShare,
				@IceSharePercentage = IceShare,
				@PartnerSharePercentage = PartnerShare 
		FROM AZ.ReportPriceShare 
		WHERE UserVCode = @UserVCode AND ReportSourceVCode = @ReportSourceVCode

		IF @SourceSharePercentage IS NULL
		BEGIN
			SELECT @SourceSharePercentage = [Value] FROM AZ.ApplicationSetting WHERE [Key] = 'SourceShare'
		END
		IF @IceSharePercentage IS NULL
		BEGIN
			SELECT @IceSharePercentage = [Value] FROM AZ.ApplicationSetting WHERE [Key] = 'IceShare'
		END
		IF @PartnerSharePercentage IS NULL
		BEGIN
			SELECT @PartnerSharePercentage = [Value] FROM AZ.ApplicationSetting WHERE [Key] = 'PartnerShare'
		END

		SET @SourceShare = @TotalPrice * @SourceSharePercentage / 100
		SET @PartnerShare = @TotalPrice * @PartnerSharePercentage / 100
		SET @IceShare = @TotalPrice - @SourceShare - @PartnerShare

		UPDATE AZ.CheckFirmReportExistenceReportSource
		SET HasReport = @HasReport,
			ReportPrice = @ReportPrice,
			TaxPrice = @TaxPrice,
			TotalPrice = @TotalPrice,
			SourceShare = @SourceShare,
			IceShare = @IceShare,
			PartnerShare = @PartnerShare
		WHERE	CheckFirmReportExistenceVCode = @CheckFirmReportExistenceVCode AND
				ReportSourceVCode = @ReportSourceVCode
	END TRY
		BEGIN CATCH
		DECLARE @ErrNo INT,@ErrMsg NVARCHAR(4000),@ErrSev INT,@ErrStt INT
		SELECT @ErrMsg = '(' + CONVERT(NVARCHAR(4000),ERROR_NUMBER()) + ') ' + ERROR_MESSAGE(),@ErrSev = ERROR_SEVERITY(),@ErrStt = ERROR_STATE()
		IF @@TRANCOUNT > 0 ROLLBACK TRAN
		RAISERROR(@ErrMsg,@ErrSev,@ErrStt)
	END CATCH
END