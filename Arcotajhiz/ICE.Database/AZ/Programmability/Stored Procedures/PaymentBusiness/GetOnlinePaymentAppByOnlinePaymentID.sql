CREATE PROCEDURE [AZ].[GetOnlinePaymentAppByOnlinePaymentID](
	@OnlinePaymentID AS BIGINT,
	@IP AS VARCHAR(100),
	@IdentificationVCode BIGINT OUTPUT,
	@BankPortalVCode INT OUTPUT,
	@Amount DECIMAL(18,0) OUTPUT,
	@LocalDate CHAR(8) OUTPUT,
	@LocalTime CHAR(6) OUTPUT,
	@SaleRefID VARCHAR(100) OUTPUT,
	@AdditionalData VARCHAR(500) OUTPUT,
	@PortalPaymentTypeVCode INT OUTPUT,
	@PaymentAmounts NVARCHAR(200) OUTPUT
)
AS
BEGIN
	IF @IP IS NOT NULL
	BEGIN
		IF NOT EXISTS(SELECT 1 FROM AZ.OnlinePayment WHERE VCode = @OnlinepaymentID AND IpAddress = @IP)
			RAISERROR('Suspicious "OnlinePaymentID" does not belong to this IP',16,	126)
	END

	SELECT	@IdentificationVCode = IdentificationVCode,
			@BankPortalVCode = BankPortalVCode,
			@Amount = Amount,
			@LocalDate = LocalDate,
			@LocalTime = LocalTime,
			@SaleRefID = SaleRefID,
			@AdditionalData = AdditionalData,
			@PortalPaymentTypeVCode = PortalPaymentTypeVCode,
			@PaymentAmounts = PaymentAmounts
	FROM AZ.OnlinePayment WHERE VCode = @OnlinePaymentID AND IpAddress = ISNULL(@IP,IpAddress)
END