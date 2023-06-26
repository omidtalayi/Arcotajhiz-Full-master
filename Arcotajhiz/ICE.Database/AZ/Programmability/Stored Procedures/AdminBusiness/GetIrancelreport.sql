CREATE PROCEDURE [AZ].[GetIrancelReport](@ReportJDate CHAR(8))
AS
BEGIN
	DECLARE @Date DATETIME = (SELECT AZ.ConvertFromJalaliDate(@ReportJDate))

	SELECT	'' OrderID,
			JDate [Date],
			CAST(AZ.ConvertFromJalaliDate(JDate) AS DATE) [Date],
			JTime [Time],
			'' CustomerFullName,
			'98' + SUBSTRING(Cellphone,2,11) Cellphone,
			'SUCCESSFUL' PaymentTransactionStatus,
			'' PaymentID,
			'ONLINE' PaymentType,
			Amount Price,
			'' DiscountAmount,
			'' TransactionFee,
			'' CommissionRule,
			'' UserType,
			ISNULL(IrancellShare,19520) IrancellShare
	FROM
	(
		SELECT	AZ.ConvertToJalaliDate(I.EntryDate) JDate,
				FORMAT(DATEPART(HOUR,I.EntryDate),'00') + ':' + FORMAT(DATEPART(MINUTE,I.EntryDate),'00') + ':' + FORMAT(DATEPART(SECOND,I.EntryDate), '00') JTime,
				OP.Amount Amount,
				I.UserVCode userVCode,
				IT.[Name] ReportType,
				I.Cellphone,
				I.NationalCode,
				IIE.Amount IrancellShare,
				OPR.SaleRefID,
				RefId
		FROM AZ.Identification (NOLOCK) I
		INNER JOIN AZ.OnlinePayment (NOLOCK) OP ON I.VCode = OP.IdentificationVCode
		INNER JOIN AZ.OnlinePaymentReceived (NOLOCK) OPR ON OP.VCode = OPR.OnlinePaymentVCode AND ((BankPortalVCode = 1 AND OPR.RefID = 'OK') OR (BankPortalVCode = 8 AND ResCode = 0))
		INNER JOIN AZ.IdentificationHistory (NOLOCK) IH ON IH.IdentificationVCode = I.VCode AND IH.IdentificationStateVCode = 11
		INNER JOIN AZ.IdentificationType (NOLOCK) IT ON IT.VCode = I.IdentificationTypeVCode
		LEFT JOIN AZ.IdentificationInvoice (NOLOCK) IIE ON IIE.IdentificationVCode = I.VCode
		LEFT JOIN [$(ICEUserManagement)].AZ.[User] U ON U.VCode = I.UserVCode
		WHERE CAST(I.EntryDate AS DATE) = @Date
	) K
	WHERE userVCode = 2040
	ORDER BY JDate,JTime
END
GO

