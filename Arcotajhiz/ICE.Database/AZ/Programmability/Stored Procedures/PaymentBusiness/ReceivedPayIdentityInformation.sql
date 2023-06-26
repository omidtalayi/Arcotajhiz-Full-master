CREATE PROCEDURE [AZ].[ReceivedPayIdentityInformation](
	@OnlinePaymentID BIGINT,
	@RefID VARCHAR(100),
	@SaleRefID VARCHAR(100),
	@CardholderInfo VARCHAR(100),
	@CardPAN VARCHAR(25),
	@ResCode INT,
	@TraceNo NVARCHAR(500) = NULL,
	@PaymentAmount DECIMAL OUTPUT,
	@IdentityInformationVCode BIGINT OUTPUT,
	@PortalPaymentTypeVCode Smallint OUTPUT, 
	@State INT OUTPUT
)
AS BEGIN
    BEGIN TRY
		DECLARE @OnlinePaymentReceivedVCode BIGINT

		If @OnlinePaymentID IS NULL 
			RAISERROR('"OnlinePaymentID" is missing',16,126)

		IF @RefID IS NULL 
			RAISERROR('Invalid "RefID"',16,126)

		IF EXISTS(SELECT 1 FROM AZ.OnlinePaymentReceived WHERE SaleRefId = @SaleRefID) 
			RAISERROR('SaleRefID Exists',16,126)

		BEGIN TRAN
		SELECT @PortalPaymentTypeVCode = PortalPaymentTypeVCode FROM AZ.OnlinePayment WHERE VCode = @OnlinePaymentID

		IF @PortalPaymentTypeVCode = 1 
		BEGIN
			SELECT @IdentityInformationVCode = IdentityInformationVCode, @PaymentAmount = Amount FROM AZ.OnlinePayment WHERE VCode = @OnlinePaymentID
			If @IdentityInformationVCode IS NULL 
				RAISERROR('"OnlinePaymentID" is invalid',16,126)

			IF NOT EXISTS(SELECT 1 FROM AZ.IdentityInformation WHERE VCode = @IdentityInformationVCode AND IdentityInformationStateVCode = 1) 
			BEGIN
				RAISERROR('IdentityInformation has an invalid state for completing the payment',16,126)
			END
		END
		SELECT @OnlinePaymentReceivedVCode = VCode FROM AZ.OnlinePaymentReceived WHERE OnlinePaymentVCode = @OnlinePaymentID
				
		IF @OnlinePaymentReceivedVCode IS NULL
		BEGIN
			IF @PortalPaymentTypeVCode = 1 
			BEGIN
				INSERT AZ.OnlinePaymentReceived (OnlinePaymentVCode,RefID,SaleRefID,CardholderInfo,CardPAN,ResCode,TraceNo) 
					VALUES (@OnlinePaymentID,@RefID,@SaleRefID,@CardholderInfo,@CardPAN,@ResCode,@TraceNo)
				SET @OnlinePaymentReceivedVCode = SCOPE_IDENTITY()
			END
		END		

		COMMIT TRAN
		SET @State = 1
	END TRY
	BEGIN CATCH
		DECLARE @ErrNo INT,@ErrMsg NVARCHAR(4000),@ErrSev INT,@ErrStt INT
		SELECT @ErrMsg = '(' + CONVERT(NVARCHAR(4000),ERROR_NUMBER()) + ') ' + ERROR_MESSAGE(),@ErrSev = ERROR_SEVERITY(),@ErrStt = ERROR_STATE()
		IF @@TRANCOUNT > 0 ROLLBACK TRAN
		RAISERROR(@ErrMsg,@ErrSev,@ErrStt)
		SET @State = 0
	END CATCH
END