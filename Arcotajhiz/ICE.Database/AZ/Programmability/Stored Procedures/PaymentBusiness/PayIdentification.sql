CREATE PROCEDURE [AZ].[PayIdentification] (
	@VCode BIGINT,
	@IpAddress VARCHAR(15),
	@BankPortalVCode INT,
	@JDate CHAR(8),
	@PayFromApp BIT,
	@OnlinePaymentID BIGINT OUTPUT,
	@JTime CHAR(6) OUTPUT,
	@PaymentAmount DECIMAL(28,0) OUTPUT,
	@PortalPaymentTypeVCode Smallint,
	@State SMALLINT OUTPUT,
	@PaymentAmounts NVARCHAR(200) OUTPUT,
	@UserVCode BIGINT = NULL,
	@RequestRegistrationVCode BIGINT = NULL
)
AS BEGIN
    BEGIN TRAN
    BEGIN TRY
		DECLARE @Result INT,
				@SamanIsActiveBuyer BIT,
				@ReportPrice DECIMAL(18,0),
				@TaxPrice DECIMAL(18,0),
				@ChargingCreditEnabled BIT,
				@MinimumCreditAmount DECIMAL(18,0),
				@MaximumCreditAmount DECIMAL(18,0),
				@SourceSharePercentage DECIMAL(18,10),
				@IceSharePercentage DECIMAL(18,10),
				@SourceShare DECIMAL(18,0),
				@IceShare DECIMAL(18,0),
				@PresenterPaymentEnabled BIT,
				@PresenterPaymentAmount DECIMAL(18,0),
				@IdentificationTypeVCode INT = 1,
				@ChequePrice DECIMAL(18,0) = 0,
				@HasChequeReport BIT = 0,
				@ChequePriceTax DECIMAL(18,0) = 0,
				@FirmApiPaymentAmount DECIMAL(18,0)
			
		SELECT @SamanIsActiveBuyer = [Value] FROM AZ.ApplicationSetting WHERE [Key] = 'SamanIsActiveBuyer'
		SELECT @ChargingCreditEnabled = [Value] FROM AZ.ApplicationSetting WHERE [Key] = 'ChargingCreditEnabled'
		SELECT @MinimumCreditAmount = [Value] FROM AZ.ApplicationSetting WHERE [Key] = 'MinimumCreditAmount'
		SELECT @MaximumCreditAmount = [Value] FROM AZ.ApplicationSetting WHERE [Key] = 'MaximumCreditAmount'
		SELECT @PresenterPaymentEnabled = [Value] FROM AZ.ApplicationSetting WHERE [Key] = 'PresenterPaymentEnabled'
		SELECT @PresenterPaymentAmount = [Value] FROM AZ.ApplicationSetting WHERE [Key] = 'PresenterPaymentAmount'
		SELECT @FirmApiPaymentAmount = [Value] FROM AZ.ApplicationSetting WHERE [Key] = 'FirmApiPaymentAmount'
		SELECT @State = 0
		SELECT @IdentificationTypeVCode = IdentificationTypeVCode FROM AZ.Identification WHERE VCode = @VCode

		IF @SamanIsActiveBuyer = 0 AND @BankPortalVCode = 2
		BEGIN
			RAISERROR('Suspicious saman portal is disabled',16,126) 
		END		

		IF @PortalPaymentTypeVCode = 1 
		BEGIN
			IF NOT EXISTS(
				SELECT 1 FROM AZ.[Identification] WHERE VCode = @VCode AND IdentificationStateVCode = 3
			)
			BEGIN
				RAISERROR('Suspicious identificationState is invalid for pay',16,126) 
			END

			IF EXISTS(SELECT 1 FROM AZ.IdentificationCheque WHERE IdentificationVCode = @VCode)
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
			IF @HasChequeReport = 1
			BEGIN
				SELECT @ChequePrice = ISNULL(SUM(ISNULL(Price,0)),0) FROM AZ.PriceList (NOLOCK) WHERE VCode = 5
				SELECT @ChequePriceTax = ISNULL(SUM(ISNULL(Price,0)),0) FROM AZ.PriceList (NOLOCK) WHERE VCode = 6
			END
			SELECT @PaymentAmount = @ReportPrice + @TaxPrice

			SELECT	@SourceSharePercentage = SourceShare,
					@IceSharePercentage = IceShare
			FROM AZ.ReportPriceShare 
			WHERE UserVCode = 4

			SET @SourceShare = @PaymentAmount * @SourceSharePercentage / 100
			SET @IceShare = @PaymentAmount - @SourceShare + @ChequePrice + @ChequePriceTax
			SET @PaymentAmount = @PaymentAmount + @ChequePrice + @ChequePriceTax

			SET @JTime = RIGHT('000000' + CONVERT(VARCHAR(6),DATEPART(HOUR,GETDATE()) * 10000 + DATEPART(MINUTE,GETDATE()) * 100 + DATEPART(SECOND,GETDATE())),6)
			SET @PaymentAmounts = CAST(@IceShare AS NVARCHAR(200)) + ',' + CAST(@SourceShare AS NVARCHAR(200))

			INSERT AZ.OnlinePayment (IdentificationVCode,IpAddress,BankPortalVCode,Amount,LocalDate,LocalTime,PortalPaymentTypeVCode,PaymentAmounts,UserVCode,NotMultiSettle) 
			VALUES (@VCode,@IpAddress,@BankPortalVCode,@PaymentAmount,@JDate,@JTime,@PortalPaymentTypeVCode,@PaymentAmounts,@UserVCode,(Select [value] from AZ.ApplicationSetting WHERE VCode = 39))

			UPDATE AZ.Identification SET PayFromApp = @PayFromApp WHERE VCode = @VCode
			
			SET @OnlinePaymentID = SCOPE_IDENTITY()
		END

		IF @PortalPaymentTypeVCode = 2
		BEGIN
			IF @ChargingCreditEnabled = 1
			BEGIN
				IF @PaymentAmount BETWEEN @MinimumCreditAmount AND @MaximumCreditAmount
				BEGIN
					SET @JTime = RIGHT('000000' + CONVERT(VARCHAR(6),DATEPART(HOUR,GETDATE()) * 10000 + DATEPART(MINUTE,GETDATE()) * 100 + DATEPART(SECOND,GETDATE())),6)
					SET @PaymentAmounts = CAST(@PaymentAmount AS NVARCHAR(200)) + ',0'

					INSERT AZ.OnlinePayment (IdentificationVCode,IpAddress,BankPortalVCode,Amount,LocalDate,LocalTime,PortalPaymentTypeVCode,PaymentAmounts,UserVCode,NotMultiSettle) 
					VALUES (@VCode,@IpAddress,@BankPortalVCode,@PaymentAmount,@JDate,@JTime,@PortalPaymentTypeVCode,@PaymentAmounts,@UserVCode,(Select [value] from AZ.ApplicationSetting WHERE VCode = 39))
					SET @OnlinePaymentID = SCOPE_IDENTITY()
				END
				ELSE
				BEGIN
					RAISERROR('Suspicious Charging credit is out of minimum and maximum amount',16,126)
				END
			END
			ELSE
			BEGIN
				RAISERROR('Suspicious Charging credit while ChargingCredit is Disabled',16,126)
			END
		END

		IF @PortalPaymentTypeVCode = 3
		BEGIN
			IF @PresenterPaymentEnabled = 1
			BEGIN
				SET @PaymentAmount = @PresenterPaymentAmount
				SET @JTime = RIGHT('000000' + CONVERT(VARCHAR(6),DATEPART(HOUR,GETDATE()) * 10000 + DATEPART(MINUTE,GETDATE()) * 100 + DATEPART(SECOND,GETDATE())),6)
				SET @PaymentAmounts = CAST(@PaymentAmount AS NVARCHAR(200)) + ',0'

				INSERT AZ.OnlinePayment (IdentificationVCode,IpAddress,BankPortalVCode,Amount,LocalDate,LocalTime,PortalPaymentTypeVCode,PaymentAmounts,UserVCode,RequestRegistrationVCode,NotMultiSettle) 
				VALUES (@VCode,@IpAddress,@BankPortalVCode,@PaymentAmount,@JDate,@JTime,@PortalPaymentTypeVCode,@PaymentAmounts,@UserVCode,@RequestRegistrationVCode,(Select [value] from AZ.ApplicationSetting WHERE VCode = 39))
				SET @OnlinePaymentID = SCOPE_IDENTITY()
			END
		END

		IF @PortalPaymentTypeVCode = 4
		BEGIN
			SET @PaymentAmount = @FirmApiPaymentAmount
			SET @JTime = RIGHT('000000' + CONVERT(VARCHAR(6),DATEPART(HOUR,GETDATE()) * 10000 + DATEPART(MINUTE,GETDATE()) * 100 + DATEPART(SECOND,GETDATE())),6)
			SET @PaymentAmounts = CAST(@PaymentAmount AS NVARCHAR(200)) + ',0'

			INSERT AZ.OnlinePayment (IdentificationVCode,IpAddress,BankPortalVCode,Amount,LocalDate,LocalTime,PortalPaymentTypeVCode,PaymentAmounts,UserVCode,RequestRegistrationVCode,NotMultiSettle) 
			VALUES (@VCode,@IpAddress,@BankPortalVCode,@PaymentAmount,@JDate,@JTime,@PortalPaymentTypeVCode,@PaymentAmounts,@UserVCode,@RequestRegistrationVCode,(Select [value] from AZ.ApplicationSetting WHERE VCode = 39))
			SET @OnlinePaymentID = SCOPE_IDENTITY()
		END

		COMMIT TRAN

		RETURN 1
	END TRY
	BEGIN CATCH
		DECLARE @ErrNo INT,@ErrMsg NVARCHAR(4000),@ErrSev INT,@ErrStt INT
		SELECT @ErrMsg = '(' + CONVERT(NVARCHAR(4000),ERROR_NUMBER()) + ') ' + ERROR_MESSAGE(),@ErrSev = ERROR_SEVERITY(),@ErrStt = ERROR_STATE()
		IF @@TRANCOUNT > 0 ROLLBACK TRAN
		RAISERROR(@ErrMsg,@ErrSev,@ErrStt)
		RETURN 0
	END CATCH
END