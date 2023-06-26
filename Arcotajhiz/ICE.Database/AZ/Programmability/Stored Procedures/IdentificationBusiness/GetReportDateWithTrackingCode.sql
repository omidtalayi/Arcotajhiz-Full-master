CREATE PROCEDURE [AZ].[GetReportDateWithTrackingCode](
	@TrackingCode BIGINT
)
AS
BEGIN
	SELECT EntryDate FROM AZ.CreditRiskReport WHERE TrackingCode = @TrackingCode
END