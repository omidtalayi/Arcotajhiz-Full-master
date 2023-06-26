CREATE PROCEDURE [AZ].[PayGroupOfPersons](
	@GroupOfPersonsVCode BIGINT,
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
	@UserVCode BIGINT = NULL
)
AS
BEGIN
	BEGIN TRAN
    BEGIN TRY
		DECLARE @Result INT,
				@SamanIsActiveBuyer BIT,
				@ReportPrice DECIMAL(18,0),
				@TaxPrice DECIMAL(18,0),
				@SourceSharePercentage DECIMAL(18,10),
				@IceSharePercentage DECIMAL(18,10),
				@SourceShare DECIMAL(18,0),
				@GroupOfPersonsTypeVCode INT = 1
			
		SELECT @SamanIsActiveBuyer = [Value] FROM AZ.ApplicationSetting WHERE [Key] = 'SamanIsActiveBuyer'
		SELECT @State = 0

		IF @SamanIsActiveBuyer = 0 AND @BankPortalVCode = 2
		BEGIN
			RAISERROR('Suspicious saman portal is disabled',16,126) 
		END		

		IF @PortalPaymentTypeVCode = 1 
		BEGIN
			IF NOT EXISTS(
				SELECT 1 FROM AZ.[GroupOfPersons] WHERE VCode = @GroupOfPersonsVCode AND GroupOfPersonsStateVCode = 1
			)
			BEGIN
				RAISERROR('Suspicious identificationState is invalid for pay',16,126) 
			END

			SELECT @GroupOfPersonsTypeVCode = GroupOfPersonsTypeVCode FROM AZ.GroupOfPersons WHERE VCode = @GroupOfPersonsVCode

			IF @GroupOfPersonsTypeVCode = 1
			BEGIN
				SELECT @ReportPrice = 300000
				SELECT @TaxPrice = 0
			END
			IF @GroupOfPersonsTypeVCode = 2
			BEGIN
				SELECT @ReportPrice = 300000
				SELECT @TaxPrice = 0
			END

			SELECT @PaymentAmount = @ReportPrice + @TaxPrice

			SET @JTime = RIGHT('000000' + CONVERT(VARCHAR(6),DATEPART(HOUR,GETDATE()) * 10000 + DATEPART(MINUTE,GETDATE()) * 100 + DATEPART(SECOND,GETDATE())),6)
			SET @PaymentAmounts = CAST(@PaymentAmount AS NVARCHAR(200)) + ',0'

			INSERT AZ.OnlinePayment(GroupOfPersonsVCode,IpAddress,BankPortalVCode,Amount,LocalDate,LocalTime,PortalPaymentTypeVCode,PaymentAmounts,UserVCode,NotMultiSettle) 
			VALUES (@GroupOfPersonsVCode,@IpAddress,@BankPortalVCode,@PaymentAmount,@JDate,@JTime,@PortalPaymentTypeVCode,@PaymentAmounts,@UserVCode,(Select [value] from AZ.ApplicationSetting WHERE VCode = 39))
			
			SET @OnlinePaymentID = SCOPE_IDENTITY()

			UPDATE AZ.GroupOfPersons SET PayFromApp = @PayFromApp WHERE VCode = @GroupOfPersonsVCode
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
GO

