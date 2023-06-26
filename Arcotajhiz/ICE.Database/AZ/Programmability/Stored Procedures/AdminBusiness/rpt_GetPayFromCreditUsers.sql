CREATE PROCEDURE AZ.rpt_GetPayFromCreditUsers	
AS
BEGIN
	SELECT 'همه' Username,0 UserVCode
	UNION ALL
	SELECT	DISTINCT U.[Name] Username,
			U.VCode UserVCode
	FROM AZ.IdentificationInvoice (NOLOCK) II
	LEFT JOIN AZ.IdentificationInvoiceState (NOLOCK) IIS ON IIS.VCode = II.IdentificationInvoiceStateVCode
	LEFT JOIN AZ.Identification (NOLOCK) I ON I.VCode = II.IdentificationVCode
	LEFT JOIN AZ.IdentificationInvoiceBatch (NOLOCK) IIB ON IIB.VCode = II.BatchVCode
	LEFT JOIN AZ.CreditRiskReport (NOLOCK) CRR ON CRR.IdentificationVCode = II.IdentificationVCode
	LEFT JOIN [$(ICEUserManagement)].AZ.[User] (NOLOCK) U ON U.VCode = II.UserVCode
	LEFT JOIN AZ.TransferFileShebaDetail (NOLOCK) TFSD ON TFSD.VCode = IIB.TransferFileShebaDetailVCode
	LEFT JOIN AZ.TransferFileSheba (NOLOCK) TFS ON TFS.VCode = TFSD.TransferFileShebaVCode
END