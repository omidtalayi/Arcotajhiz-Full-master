CREATE PROCEDURE [AZ].[CompletePayIdentification](
	@IdentificationVCode BIGINT,
	@OnlinePaymentID BIGINT = NULL,
	@PortalPaymentTypeVCode Smallint,
	@State SMALLINT OUTPUT,
	@PaymentAmount DECIMAL OUTPUT
)
AS BEGIN
    BEGIN TRAN
    BEGIN TRY
		DECLARE @Amount DECIMAL,
				@IsComplete BIT,
				@ChargingCreditEnabled BIT,
				@IsFromApp BIT,
				@UserVCode BIGINT,
				@PresenterPaymentEnabled BIT,
				@RequestRegistrationVCode BIGINT,
				@IdentificationInvoiceVCode BIGINT,
				@SourceShare DECIMAL(18,0),
				@SourceSharePercentage DECIMAL(18,10),
				@IdentificationCount INT,
				@ResCode INT,
				@ICSUserVCode BIGINT
		
		SELECT	@IsComplete = 1,
				@State = 0,
				@PaymentAmount = 0

		SELECT @ResCode = ResCode FROM AZ.OnlinePaymentReceived WHERE OnlinePaymentVCode = @OnlinePaymentID
		
		IF @ResCode <> 0
		BEGIN
			SET @IsComplete = 0
		END

		SELECT @ChargingCreditEnabled = Value FROM AZ.ApplicationSetting WHERE [Key] = 'ChargingCreditEnabled'
		SELECT @PresenterPaymentEnabled = Value FROM AZ.ApplicationSetting WHERE [Key] = 'PresenterPaymentEnabled'
				
		IF @PortalPaymentTypeVCode = 1 
		BEGIN
			SELECT @PaymentAmount = Amount FROM AZ.OnlinePayment WHERE VCode = @OnlinePaymentID
			
			IF @IsComplete = 1
			BEGIN
				UPDATE AZ.[Identification] SET IdentificationStateVCode = 11 WHERE VCode = @IdentificationVCode
				INSERT AZ.IdentificationHistory(IdentificationVCode,IdentificationStateVCode) VALUES(@IdentificationVCode,11)

				SELECT @UserVCode = UserVCode FROM AZ.Identification WHERE VCode = @IdentificationVCode

				IF EXISTS(SELECT 1 FROM [$(ICEUserManagement)].AZ.[User] WHERE VCode = @UserVCode AND UserTypeVCode = 2)
				BEGIN
					SELECT	@SourceSharePercentage = SourceShare
					FROM AZ.ReportPriceShare 
					WHERE UserVCode = 4

					SET @SourceShare = @PaymentAmount * @SourceSharePercentage / 100

					IF NOT EXISTS(SELECT 1 FROM AZ.IdentificationInvoice WHERE IdentificationVCode = @IdentificationVCode)
					BEGIN
						IF EXISTS(SELECT 1 FROM AZ.Identification WHERE VCode = @IdentificationVCode AND UserPaymentTypeVCode = 2)
						BEGIN
							SET @ICSUserVCode = 59 /*ICS User*/

							INSERT AZ.IdentificationInvoice(IdentificationVCode,UserVCode,IdentificationInvoiceStateVCode,Amount,SourcePercentage)
							VALUES(@IdentificationVCode,@ICSUserVCode,1,@SourceShare,@SourceSharePercentage)

							SET @IdentificationInvoiceVCode = SCOPE_IDENTITY()

							INSERT AZ.IdentificationInvoiceHistory(IdentificationInvoiceVCode,IdentificationInvoiceStateVCode)
							VALUES(@IdentificationInvoiceVCode,1) 
						END
					END
				END

				IF EXISTS(SELECT 1 FROM [$(ICEUserManagement)].AZ.[User] WHERE VCode = @UserVCode AND UserTypeVCode = 3)
				BEGIN
					SELECT @IdentificationCount = ISNULL(COUNT(1),0) + 1 FROM AZ.IdentificationInvoice 
					WHERE UserVCode = @UserVCode AND CAST(EntryDate AS DATE) = CAST(GETDATE() AS DATE) 

					SET @SourceSharePercentage = 
					(
						SELECT TOP 1 TrackingShare FROM AZ.ReportPriceShareTracking
						WHERE UserVCode = @UserVCode 
							AND @IdentificationCount BETWEEN ISNULL(IdentificationCountStart,@IdentificationCount) AND ISNULL(IdentificationCountEnd,@IdentificationCount)
					)

					SET @SourceShare = @PaymentAmount * @SourceSharePercentage / 100

					IF ISNULL(@SourceShare,0) > 0 
					BEGIN
						INSERT AZ.IdentificationInvoice(IdentificationVCode,UserVCode,IdentificationInvoiceStateVCode,Amount,SourcePercentage)
						VALUES(@IdentificationVCode,@UserVCode,1,@SourceShare,@SourceSharePercentage)

						SET @IdentificationInvoiceVCode = SCOPE_IDENTITY()

						INSERT AZ.IdentificationInvoiceHistory(IdentificationInvoiceVCode,IdentificationInvoiceStateVCode)
						VALUES(@IdentificationInvoiceVCode,1) 
					END
				END
			END 
		END

		IF @PortalPaymentTypeVCode = 2
		BEGIN
			IF @ChargingCreditEnabled = 1
			BEGIN
				SELECT @Amount = Amount,@UserVCode = UserVCode FROM AZ.OnlinePayment WHERE VCode = @OnlinePaymentID 
				EXEC [AZ].[CreateCredit] @UserVCode = @UserVCode,@Amount = @Amount,@OnlinePaymentVCode = @OnlinePaymentID
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
				SELECT @RequestRegistrationVCode = RequestRegistrationVCode FROM AZ.OnlinePayment WHERE VCode = @OnlinePaymentID
				IF EXISTS(SELECT 1 FROM AZ.RequestRegistration WHERE VCode = @RequestRegistrationVCode AND RequestRegistrationStateVCode = 1)
				BEGIN
					SELECT @Amount = Amount,@UserVCode = UserVCode FROM AZ.OnlinePayment WHERE VCode = @OnlinePaymentID 
					EXEC [AZ].[CreateCredit] @UserVCode = @UserVCode,@Amount = @Amount,@OnlinePaymentVCode = @OnlinePaymentID

					EXEC AZ.AccountingIns @UserVCode = @UserVCode,@AccountingTypeVCode = 3,@Bed = @Amount,@Bes = 0,@IdentificationVCode = NULL,@RequestRegistrationVCode = @RequestRegistrationVCode

					UPDATE AZ.RequestRegistration SET RequestRegistrationStateVCode = 4 WHERE VCode = @RequestRegistrationVCode

					IF NOT EXISTS(SELECT 1 FROM AZ.RequestRegistrationHistory WHERE RequestRegistrationVCode = @RequestRegistrationVCode AND RequestRegistrationStateVCode = 4)
					BEGIN
						INSERT AZ.RequestRegistrationHistory(RequestRegistrationVCode,RequestRegistrationStateVCode) VALUES(@RequestRegistrationVCode,4)
					END
				END
			END
		END

		IF @PortalPaymentTypeVCode = 4
		BEGIN
			SELECT @RequestRegistrationVCode = RequestRegistrationVCode FROM AZ.OnlinePayment WHERE VCode = @OnlinePaymentID

			SELECT @Amount = Amount,@UserVCode = UserVCode FROM AZ.OnlinePayment WHERE VCode = @OnlinePaymentID 

			EXEC [AZ].[CreateCredit] @UserVCode = @UserVCode,@Amount = @Amount,@OnlinePaymentVCode = @OnlinePaymentID

			EXEC AZ.AccountingIns @UserVCode = @UserVCode,@AccountingTypeVCode = 5,@Bed = @Amount,@Bes = 0,@IdentificationVCode = NULL,@RequestRegistrationVCode = NULL

			IF EXISTS(SELECT 1 FROM AZ.RequestRegistration WHERE VCode = @RequestRegistrationVCode AND RequestRegistrationStateVCode = 1) 
			BEGIN
				UPDATE AZ.RequestRegistration SET RequestRegistrationStateVCode = 5 WHERE VCode = @RequestRegistrationVCode

				IF NOT EXISTS(SELECT 1 FROM AZ.RequestRegistrationHistory WHERE RequestRegistrationVCode = @RequestRegistrationVCode AND RequestRegistrationStateVCode = 5)
				BEGIN
					INSERT AZ.RequestRegistrationHistory(RequestRegistrationVCode,RequestRegistrationStateVCode) VALUES(@RequestRegistrationVCode,5)
				END
			END
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

