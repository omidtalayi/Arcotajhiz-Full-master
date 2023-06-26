CREATE PROCEDURE [AZ].[GetIdentificationSentRepairMessage]
AS
BEGIN
    SELECT 1 R,
		I.*,
		NULL UserPaymentTypeName,
		NULL TrackingCode,
		NULL TraceNo,
		NULL PaymentAmount,
		NULL PaymentDate,
		NULL IdentificationState,
		NULL IdentificationType,
		NULL IdentificationSendToOthers,
		NULL ResponseJson
	INTO #T FROM AZ.Identification I(NOLOCK)
	WHERE I.IdentificationStateVCode = 11
	AND I.IdICS24 IS NULL
	AND I.RedirectUrlICS24 IS NULL
	AND I.IsRepairMessageSent = 0
	AND ReportExpirationDate >= GETDATE()
	AND NOT EXISTS(SELECT 1 FROM [$(ICEUserManagement)].AZ.[User] (NOLOCK) U WHERE UserTypeVCode = 2 AND I.UserVCode = U.VCode)
	AND NOT EXISTS(SELECT 1 FROM Az.SMSLog SL(NOLOCK) WHERE SL.IdentificationVCode = I.VCode AND SMSLogTypeVCode = 7)
	SELECT * FROM #T
	UPDATE AZ.Identification SET IsPendingICS24Service = 0 , IsRepairMessageSent = 1 Where VCode in (SELECT VCode FROM #T)
	DROP TABLE #T
END