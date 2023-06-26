CREATE PROCEDURE [AZ].[GetIdentificationsICS24]
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
	FROM AZ.Identification (NOLOCK) 
	WHERE IdICS24 IS NOT NULL 
		AND ReportExpirationDate >= GETDATE()
		AND IdentificationStateVCode IN (11,12,13,6,17)
		AND UserVCode Not In (SELECT VCode FROM [$(ICEUserManagement)].AZ.[User] Where UserTypeVCode=  2 AND FirmRegistrationVCode IS NOT NULL)
END