CREATE PROCEDURE [AZ].[GetIdentificationReportIsCorrupted]
AS
BEGIN
    SELECT 1 R,
		I.*,
		NULL UserPaymentTypeName,
		NULL TrackingCode,
		NULL TraceNo,
		NULL PaymentAmount,
		NULL PaymentDate,
		('<IdentificationStates>' + (SELECT * FROM AZ.IdentificationState (NOLOCK) WHERE VCode = I.IdentificationStateVCode FOR XML PATH('IdentificationState')) + '</IdentificationStates>') IdentificationState,
		NULL IdentificationType,
		NULL IdentificationSendToOthers,
		NULL ResponseJson
	INTO #T FROM AZ.Identification I(NOLOCK)
	WHERE I.IdentificationStateVCode IN (11,17,18,19)
			AND I.ReportIsCorrupted = 1 And I.UserVCode = 4

	UPDATE AZ.Identification SET ReportExpirationDate = DATEADD(DAY,1,GETDATE()) WHERE VCode IN (SELECT VCode FROM #T WHERE IdentificationStateVCode = 11)
	--UPDATE AZ.Identification SET ReportIsCorrupted = 0 WHERE VCode IN (SELECT VCode FROM #T)
	UPDATE AZ.Identification SET ReportIsCorrupted = 0 WHERE ReportIsCorrupted = 1

	SELECT * FROM #T

	DROP TABLE #T
END
GO