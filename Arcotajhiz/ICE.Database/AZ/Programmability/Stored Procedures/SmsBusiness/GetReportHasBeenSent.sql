CREATE PROCEDURE [AZ].[GetReportHasBeenSent](
	@identificationVCode BIGINT
)
AS
BEGIN
	IF EXISTS(SELECT 1 FROM AZ.SMSLog (NOLOCK) WHERE IdentificationVCode = @identificationVCode AND SendState = 1 AND SMSLogTypeVCode = 3)
	BEGIN
		SELECT 1
	END
	ELSE
	BEGIN
		SELECT 0
	END
END