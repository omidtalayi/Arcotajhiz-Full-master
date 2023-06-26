CREATE PROCEDURE [AZ].[GetIdentificationSimple](
	@VCode BIGINT = NULL
)
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
	FROM AZ.Identification (NOLOCK) I
	WHERE I.VCode = @VCode
END
GO