CREATE PROCEDURE [AZ].[GetIdentificationSendToOthers](
	@IdentificationVCode BIGINT
)
AS
BEGIN
	SELECT * FROM AZ.IdentificationSendToOthers WHERE IdentificationVCode = @IdentificationVCode
END