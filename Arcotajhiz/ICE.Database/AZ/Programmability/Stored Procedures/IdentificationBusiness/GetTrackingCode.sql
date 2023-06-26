CREATE PROCEDURE [AZ].[GetTrackingCode](
	@IdentificationVCode BIGINT
)
AS
BEGIN
	SELECT TrackingCode FROM AZ.CreditRiskReport WHERE IdentificationVCode = @IdentificationVCode
END