CREATE PROCEDURE [AZ].[rpt_PartnerPaymentConfirmation](	
	@FromJDate VARCHAR(8) = NULL,
	@ToJDate VARCHAR(8) = NULL,
	@PartnerVCode BIGINT = NULL,
	@ConflictedRecords BIT = 0
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

	SELECT * FROM 
	(
		SELECT	ROW_NUMBER() OVER(ORDER BY I.VCode DESC) [Row],
				U.Username,
				ISNULL(CRR.TrackingCode,0) TrackingCode,
				I.Cellphone,
				I.NationalCode,
				IPT.SaleAmount Amount,
				AZ.ConvertToJalaliDate(I.EntryDate) JDate,
	 			OPC.ConfirmedJDate,
				OPC.RefID SaleRefID,
				OPC.Amount ConfirmedAmount,
				IPT.EntryDate,
				BP.[Name] BankPortal
		FROM AZ.Identification (NOLOCK) I
		LEFT JOIN AZ.IdentificationPayment (NOLOCK) IPT ON IPT.IdentificationVCode = I.VCode
		LEFT JOIN AZ.CreditRiskReport (NOLOCK) CRR ON CRR.IdentificationVCode = I.VCode
		LEFT JOIN [$(ICEUserManagement)].AZ.[User] (NOLOCK) U ON U.VCode = I.UserVCode
		LEFT JOIN [$(ICEUserManagement)].AZ.[UserPaymentType] (NOLOCK) UPT ON UPT.VCode = I.UserPaymentTypeVCode
		LEFT JOIN AZ.OnlinePaymentConfirmation (NOLOCK) OPC ON OPC.RefID = IPT.SaleRefID
		LEFT JOIN AZ.BankPortal (NOLOCK) BP ON BP.VCode = OPC.BankPortalVCode
		WHERE	U.VCode IN (SELECT VCode FROM [$(ICEUserManagement)].AZ.[User] WHERE UserTypeVCode = 1 AND VCode NOT IN (4)) 
				AND CAST(I.EntryDate AS DATE) BETWEEN ISNULL(@FromDate,CAST(I.EntryDate AS DATE)) AND ISNULL(@ToDate,CAST(I.EntryDate AS DATE))
				AND U.VCode = ISNULL(@PartnerVCode,U.VCode)
				AND (@ConflictedRecords = 0 OR (@ConflictedRecords = 1 AND OPC.RefID IS NULL))
	) T
	ORDER BY T.[Row]
END
GO

