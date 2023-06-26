CREATE PROCEDURE [AZ].[Rpt_PayReportByCreditGetUsers]
AS
BEGIN
	SELECT DISTINCT * FROM
	(
		SELECT  DISTINCT U.[Name],U.[VCode]
		FROM AZ.Accounting (NOLOCK) A
		INNER JOIN [$(ICEUserManagement)].AZ.[User] (NOLOCK) U ON A.UserVCode = U.VCode
		INNER JOIN AZ.Identification (NOLOCK) I ON I.VCode = A.IdentificationVCode 
		WHERE IdentificationVCode IS NOT NULL
			AND NOT EXISTS (SELECT 1 FROM AZ.Accounting (NOLOCK) A_Return WHERE A_Return.IdentificationVCode = A.IdentificationVCode AND A_Return.AccountingTypeVCode = 4)
		UNION ALL
		SELECT	DISTINCT U.[Name],U.[VCode]
		FROM AZ.Identification (NOLOCK) I
		INNER JOIN AZ.IdentificationInvoice (NOLOCK) II ON I.VCode = II.IdentificationVCode
		INNER JOIN AZ.OnlinePayment (NOLOCK) OP ON I.VCode = OP.IdentificationVCode
		INNER JOIN AZ.OnlinePaymentReceived (NOLOCK) OPR ON OP.VCode = OPR.OnlinePaymentVCode AND OPR.RefID = 'OK'
		INNER JOIN [$(ICEUserManagement)].AZ.[User] (NOLOCK) U ON I.UserVCode = U.VCode
		WHERE I.VCode IS NOT NULL AND U.UserTypeVCode = 2 AND I.UserPaymentTypeVCode = 1
	) T
	ORDER BY T.[Name]
END
GO

