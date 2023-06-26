CREATE PROCEDURE AZ.rpt_GetTransferFileShebaTotal(
	@FromJDate VARCHAR(8) = NULL,
	@ToJDate VARCHAR(8) = NULL
)
AS
BEGIN	
	SELECT	'صورت پرداخت شماره ' + CAST(TFS.DocumentNo AS NVARCHAR(10)) DocumentNo,
			TFS.JDate,
			TFS.TotalRow,
			TFS.TotalPrice,
			IIS.[Name] StateName
	FROM AZ.TransferFileSheba (NOLOCK) TFS
	INNER JOIN AZ.IdentificationInvoiceState (NOLOCK) IIS ON TFS.StateVCode = IIS.VCode
	WHERE TFS.JDate BETWEEN ISNULL(@FromJDate,TFS.JDate) AND ISNULL(@ToJDate,TFS.JDate)
END