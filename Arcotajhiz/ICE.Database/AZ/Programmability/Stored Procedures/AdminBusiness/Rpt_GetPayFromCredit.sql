CREATE PROCEDURE [AZ].[Rpt_GetPayFromCredit](
	@FromJDate VARCHAR(8) = NULL,
	@ToJDate VARCHAR(8) = NULL,
	@DocumentNo INT = NULL,
	@PayUserVCode INT = NULL,
	@IdentificationTypeVCode INT = NULL
)
AS
BEGIN
	DECLARE @FromDate DATE = NULL
	DECLARE @ToDate DATE

	IF @FromJDate IS NOT NULL 
	BEGIN
		SET @FromDate = AZ.ConvertFromJalaliDate(@FromJDate)
	END
	IF @ToJDate IS NOT NULL
	BEGIN
		SET @ToDate = AZ.ConvertFromJalaliDate(@ToJDate)
	END

	SELECT	ROW_NUMBER() OVER(ORDER BY II.VCode DESC) [Row],
			I.VCode IdentificationVCode,
			II.VCode IdentificationInvoiceVCode,
			ISNULL(CRR.TrackingCode,0) TrackingCode,
			I.Cellphone,
			I.IdentificationTypeVCode,
			I.NationalCode,
			II.Amount,
			II.IdentificationInvoiceStateVCode,
			IIS.[Name] IdentificationInvoiceState,
			U.[Name] Username,
			'صورت پرداخت شماره ' + CAST(TFS.DocumentNo AS NVARCHAR(10)) DocumentNo,
			II.EntryDate,
			TFSD.TransferNumber,
			TFSD.SettleDate,
			TFSD.SettleTime
	FROM AZ.IdentificationInvoice (NOLOCK) II
	LEFT JOIN AZ.IdentificationInvoiceState (NOLOCK) IIS ON IIS.VCode = II.IdentificationInvoiceStateVCode
	LEFT JOIN AZ.Identification (NOLOCK) I ON I.VCode = II.IdentificationVCode
	LEFT JOIN AZ.IdentificationInvoiceBatch (NOLOCK) IIB ON IIB.VCode = II.BatchVCode
	LEFT JOIN AZ.CreditRiskReport (NOLOCK) CRR ON CRR.IdentificationVCode = II.IdentificationVCode
	LEFT JOIN [$(ICEUserManagement)].AZ.[User] (NOLOCK) U ON U.VCode = II.UserVCode
	LEFT JOIN AZ.TransferFileShebaDetail (NOLOCK) TFSD ON TFSD.VCode = IIB.TransferFileShebaDetailVCode
	LEFT JOIN AZ.TransferFileSheba (NOLOCK) TFS ON TFS.VCode = TFSD.TransferFileShebaVCode
	WHERE CAST(II.EntryDate AS DATE) BETWEEN ISNULL(@FromDate,CAST(II.EntryDate AS DATE)) AND ISNULL(@ToDate,CAST(II.EntryDate AS DATE))
		AND ISNULL(ISNULL(@DocumentNo,TFS.DocumentNo),0) = ISNULL(TFS.DocumentNo,0)
		AND ISNULL(ISNULL(@PayUserVCode,U.VCode),0) = ISNULL(U.VCode,0)
		AND ISNULL(ISNULL(@IdentificationTypeVCode,I.IdentificationTypeVCode),0) = ISNULL(I.IdentificationTypeVCode,0)
	ORDER BY II.VCode DESC
END
GO

