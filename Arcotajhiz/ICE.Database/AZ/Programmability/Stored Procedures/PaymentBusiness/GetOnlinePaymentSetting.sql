CREATE PROCEDURE AZ.GetOnlinePaymentSetting(
	@BankPortalVCode INT
) AS
BEGIN
	SELECT [Key],[Value],[Description] FROM AZ.OnlinePaymentSetting (NOLOCK) WHERE BankPortalVCode = @BankPortalVCode
END