CREATE PROCEDURE [AZ].[GetMostRepairedIdentifications]
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
			NULL ResponseXml,
			NULL ResponseJson
	FROM AZ.Identification (NOLOCK) I
	WHERE I.IdentificationStateVCode = 18 AND
	--NOT EXISTS(SELECT 1 FROM [$(ICEUserManagement)].AZ.[User] (NOLOCK) U WHERE UserTypeVCode = 2 AND I.UserVCode = U.VCode) AND
	--(RedirectUrlICS24 IS NULL AND AppIcs24HashCode IS NULL) OR (AppIcs24HashCode IS Not NULL AND ReportLink IS NULL)AND
	ReportLink IS NULL AND
	DATEDIFF(MINUTE,I.EntryDate,GETDATE()) > 5 AND 
	ReportExpirationDate >= GETDATE()
END
GO