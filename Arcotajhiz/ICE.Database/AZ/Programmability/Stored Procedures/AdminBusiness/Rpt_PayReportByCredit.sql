CREATE PROCEDURE [AZ].[Rpt_PayReportByCredit](
	@FromJDate VARCHAR(8) = NULL,
	@ToJDate VARCHAR(8) = NULL,
	@UserVCode BIGINT = NULL
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

	SELECT ROW_NUMBER() OVER(ORDER BY T.VCode DESC) [Row],* FROM
	(
		SELECT  I.VCode,
				U.Username,
				U.[Name],
				I.Cellphone,
				I.NationalCode,
				A.Bed Amount,
				I.EntryDate 
		FROM AZ.Accounting (NOLOCK) A
		INNER JOIN [$(ICEUserManagement)].AZ.[User] (NOLOCK) U ON A.UserVCode = U.VCode
		INNER JOIN AZ.Identification (NOLOCK) I ON I.VCode = A.IdentificationVCode 
		WHERE IdentificationVCode IS NOT NULL
			AND CAST(I.EntryDate AS DATE) BETWEEN ISNULL(@FromDate,CAST(I.EntryDate AS DATE)) AND ISNULL(@ToDate,CAST(I.EntryDate AS DATE))
			AND U.[VCode] = ISNULL(@UserVCode,U.[VCode])
			AND NOT EXISTS (SELECT 1 FROM AZ.Accounting (NOLOCK) A_Return WHERE A_Return.IdentificationVCode = A.IdentificationVCode AND A_Return.AccountingTypeVCode = 4)
		UNION ALL
		SELECT	I.VCode,
				U.Username,
				U.[Name],
				I.Cellphone,
				I.NationalCode,
				OP.Amount Amount,
				I.EntryDate 
		FROM AZ.Identification (NOLOCK) I
		INNER JOIN AZ.IdentificationInvoice (NOLOCK) II ON I.VCode = II.IdentificationVCode
		INNER JOIN AZ.OnlinePayment (NOLOCK) OP ON I.VCode = OP.IdentificationVCode
		INNER JOIN AZ.OnlinePaymentReceived (NOLOCK) OPR ON OP.VCode = OPR.OnlinePaymentVCode AND OPR.RefID = 'OK'
		INNER JOIN [$(ICEUserManagement)].AZ.[User] (NOLOCK) U ON I.UserVCode = U.VCode
		WHERE I.VCode IS NOT NULL
			AND CAST(I.EntryDate AS DATE) BETWEEN ISNULL(@FromDate,CAST(I.EntryDate AS DATE)) AND ISNULL(@ToDate,CAST(I.EntryDate AS DATE))
			AND U.UserTypeVCode = 2 AND I.UserPaymentTypeVCode = 1
			AND U.[VCode] = ISNULL(@UserVCode,U.[VCode])
	) T
	ORDER BY T.Vcode DESC
END