CREATE PROCEDURE [AZ].[GetMostSendSMSPeygiriIdentification]
AS
BEGIN
	SELECT	1 R,
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
	INTO #T FROM AZ.Identification (NOLOCK) I
	WHERE I.IdentificationStateVCode = 3
	AND I.EntryDate  >= '2019-12-17 00:00:00.00'
	AND DATEPART(HOUR,GETDATE()) Between 10 And 21
	AND ReportExpirationDate >= GETDATE()
	AND DATEDIFF(MINUTE,I.EntryDate,GETDATE()) > 11 
	AND NOT EXISTS(SELECT * FROM AZ.Identification (NOLOCK) IT WHERE I.Cellphone = IT.Cellphone AND I.NationalCode = IT.NationalCode AND IT.IdentificationStateVCode IN (11,4,13,17))
	AND I.SendSmsPeygiri = 0
	AND I.UserVCode = 449
	AND NOT EXISTS (SELECT * FROM AZ.SMSLog (NOLOCK) SL Where SL.SendToNumber = I.Cellphone AND  SL.SMSLogTypeVCode = 6)
	UPDATE AZ.Identification SET SendSmsPeygiri = 1 WHERE VCode IN (SELECT	VCode FROM #T)
	SELECT * FROM
	(
		SELECT ROW_NUMBER() OVER(PARTITION BY Cellphone ORDER BY VCode DESC) R1,* 
		FROM #T 
	) K WHERE K.R1 = 1
	DROP TABLE #T

END
GO

