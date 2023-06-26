CREATE PROCEDURE [AZ].[rpt_OnlinePaymentConfirmationCredit](
	@FromJDate VARCHAR(8) = NULL,
	@ToJDate VARCHAR(8) = NULL,
	@FromConfirmedJDate VARCHAR(8) = NULL,
	@ToConfirmedJDate VARCHAR(8) = NULL,
	@UserVCode BIGINT = NULL,
	@BankPortalVCode SMALLINT = NULL,
	@APIPaymentCharge VARCHAR(1) = NULL
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
		SELECT	ROW_NUMBER() OVER(ORDER BY OP.VCode DESC) [Row],
				U.[Username],
				U.[Name],
				OPC.CardPAN,
				ISNULL(OPC.Amount,A.Bes) Amount,
				BP.[Name] BankPortalName,
				OPC.ConfirmedJDate,
				OPR.RefID,
				ISNULL(OPC.EntryDate,A.EntryDate) EntryDate,
				CASE WHEN OP.RequestRegistrationVCode IS NOT NULL THEN 1 ELSE 0 END APIPaymentCharge
		FROM AZ.OnlinePayment (NOLOCK) OP
		INNER JOIN AZ.Accounting (NOLOCK) A ON A.OnlinePaymentVCode = OP.VCode AND A.AccountingTypeVCode = 1
		INNER JOIN AZ.OnlinePaymentReceived (NOLOCK) OPR ON OP.VCode = OPR.OnlinePaymentVCode
		LEFT JOIN AZ.OnlinePaymentConfirmation (NOLOCK) OPC ON OP.VCode = OPR.OnlinePaymentVCode AND OP.IdentificationVCode IS NULL AND OPC.RefID = OPR.SaleRefID
		LEFT JOIN AZ.BankPortal (NOLOCK) BP ON BP.VCode = OP.BankPortalVCode
		LEFT JOIN [$(ICEUserManagement)].AZ.[User] (NOLOCK) U ON U.VCode = OP.UserVCode
		WHERE	CAST(OP.EntryDate AS DATE) BETWEEN ISNULL(@FromDate,CAST(OP.EntryDate AS DATE)) AND ISNULL(@ToDate,CAST(OP.EntryDate AS DATE)) 
				AND
				ISNULL(OPC.ConfirmedJDate,'') BETWEEN ISNULL(@FromConfirmedJDate,ISNULL(OPC.ConfirmedJDate,'')) AND ISNULL(@ToConfirmedJDate,ISNULL(OPC.ConfirmedJDate,'')) 
				AND
				U.[VCode] = ISNULL(@UserVCode,U.[VCode]) 
				AND 
				ISNULL(OPC.[BankPortalVCode],0) = ISNULL(@BankPortalVCode,ISNULL(OPC.[BankPortalVCode],0)) 
				AND
				(CASE WHEN OP.RequestRegistrationVCode IS NOT NULL THEN 1 ELSE 0 END) = ISNULL(@APIPaymentCharge,(CASE WHEN OP.RequestRegistrationVCode IS NOT NULL THEN 1 ELSE 0 END))
		) T
	Order By T.[Row] DESC
END
GO

