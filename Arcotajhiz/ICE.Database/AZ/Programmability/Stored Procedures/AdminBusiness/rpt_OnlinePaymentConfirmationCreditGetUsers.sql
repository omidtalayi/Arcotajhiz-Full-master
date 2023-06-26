CREATE PROCEDURE [AZ].[rpt_OnlinePaymentConfirmationCreditGetUsers]
AS
BEGIN	
	SELECT	DISTINCT U.[Name],U.[VCode]
	FROM AZ.OnlinePaymentConfirmation (NOLOCK) OPC
	INNER JOIN AZ.OnlinePaymentReceived (NOLOCK) OPR ON OPC.RefID = OPR.SaleRefID
	INNER JOIN AZ.OnlinePayment (NOLOCK) OP ON OP.VCode = OPR.OnlinePaymentVCode AND OP.IdentificationVCode IS NULL
	LEFT JOIN AZ.BankPortal (NOLOCK) BP ON BP.VCode = OP.BankPortalVCode
	LEFT JOIN [$(ICEUserManagement)].AZ.[User] (NOLOCK) U ON U.VCode = OP.UserVCode
	ORDER BY U.[Name]
END