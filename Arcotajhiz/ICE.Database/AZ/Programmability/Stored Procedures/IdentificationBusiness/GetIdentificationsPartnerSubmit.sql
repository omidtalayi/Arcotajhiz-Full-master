CREATE PROCEDURE [AZ].[GetIdentificationsPartnerSubmit]
AS
BEGIN
	SELECT	*,
			1 R,
			NULL UserPaymentTypeName,
			NULL TrackingCode,
			NULL TraceNo,
			NULL PaymentAmount,
			NULL PaymentDate,
			NULL IdentificationState,
			NULL IdentificationType,
			NULL IdentificationSendToOthers,
			NULL ResponseJson
	INTO #T FROM AZ.Identification (NOLOCK)
	WHERE IdICS24 IS NULL 
		AND ReportExpirationDate >= GETDATE()
		AND IdentificationStateVCode = 11
		AND IsPendingICS24Service <> 1
		AND UserVCode IN (SELECT VCode FROM	[$(ICEUserManagement)].AZ.[User] WHERE  UserTypeVCode = 1)
		UPDATE AZ.Identification SET IsPendingICS24Service = 1 WHERE VCode IN (SELECT VCode FROM #T)
		SELECT	* FROM	#T
	DROP TABLE #T
END