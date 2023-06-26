CREATE PROCEDURE [AZ].[rpt_OnlinePaymentConfirmation](
	@FromJDate VARCHAR(8) = NULL,
	@ToJDate VARCHAR(8) = NULL,
	@FromConfirmedJDate VARCHAR(8) = NULL,
	@ToConfirmedJDate VARCHAR(8) = NULL,
	@IdentificationTypeVCode SMALLINT = NULL,
	@OrderByVCode INT = 1,
	@BankPortalVCode INT = NULL
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

	DECLARE @Statement NVARCHAR(MAX) = N'
		SELECT	' + 
				CASE WHEN @OrderByVCode = 1 THEN ' ROW_NUMBER() OVER(ORDER BY OPC.VCode) [Row], '
					 WHEN @OrderByVCode = 2 THEN ' ROW_NUMBER() OVER(ORDER BY OPC.Amount) [Row], '
				END + '
				ISNULL(CRR.TrackingCode,0) TrackingCode,
				I.Cellphone,
				I.NationalCode,
				OPC.CardPAN,
				OPC.Amount,
				BP.[Name] BankPortalName,
				OPC.ConfirmedJDate,
				OPR.RefID,
				OPC.EntryDate,
				OPC.TerminalID
		FROM AZ.OnlinePaymentConfirmation (NOLOCK) OPC
		LEFT JOIN AZ.OnlinePaymentReceived (NOLOCK) OPR ON OPC.RefID = OPR.SaleRefID
		LEFT JOIN AZ.OnlinePayment (NOLOCK) OP ON OP.VCode = OPR.OnlinePaymentVCode AND OP.IdentificationVCode IS NOT NULL
		LEFT JOIN AZ.BankPortal (NOLOCK) BP ON BP.VCode = OP.BankPortalVCode
		LEFT JOIN AZ.Identification (NOLOCK) I ON I.VCode = OP.IdentificationVCode
		LEFT JOIN AZ.CreditRiskReport (NOLOCK) CRR ON CRR.IdentificationVCode = OP.IdentificationVCode
		LEFT JOIN [ICEUserManagement].AZ.[User] (NOLOCK) U ON U.VCode = I.UserVCode
		LEFT JOIN [ICEUserManagement].AZ.[UserPaymentType] (NOLOCK) UPT ON UPT.VCode = I.UserPaymentTypeVCode
		WHERE	CAST(OPC.EntryDate AS DATE) BETWEEN ISNULL(@FromDate,CAST(OPC.EntryDate AS DATE)) AND ISNULL(@ToDate,CAST(OPC.EntryDate AS DATE)) AND
				OPC.ConfirmedJDate BETWEEN ISNULL(@FromConfirmedJDate,OPC.ConfirmedJDate) AND ISNULL(@ToConfirmedJDate,OPC.ConfirmedJDate) AND OPC.TerminalID IN (5313133,11579950,12773764) AND
				I.IdentificationTypeVCode = ISNULL(@IdentificationTypeVCode,I.IdentificationTypeVCode) ' + 
		CASE WHEN @BankPortalVCode IS NOT NULL THEN ' AND BP.VCode = @BankPortalVCode ' ELSE ' ' END +
		CASE WHEN @OrderByVCode = 1 THEN ' ORDER BY OPC.VCode '
			 WHEN @OrderByVCode = 2 THEN ' ORDER BY OPC.Amount '
		END
		
	
	EXEC sp_executesql @stmt = @Statement,@params = N'@FromDate AS DATETIME,@ToDate DATETIME,@FromConfirmedJDate VARCHAR(8),@ToConfirmedJDate VARCHAR(8),@IdentificationTypeVCode INT,@BankPortalVCode SMALLINT',
	@FromDate = @FromDate,@ToDate = @ToDate,@FromConfirmedJDate = @FromConfirmedJDate,@ToConfirmedJDate = @ToConfirmedJDate,@IdentificationTypeVCode = @IdentificationTypeVCode,@BankPortalVCode = @BankPortalVCode
END
GO