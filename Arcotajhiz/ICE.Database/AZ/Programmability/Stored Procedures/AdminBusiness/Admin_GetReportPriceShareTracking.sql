CREATE PROCEDURE [AZ].[Admin_GetReportPriceShareTracking](
	@UserVCode BIGINT
)
AS
BEGIN
	SELECT	VCode,
			UserVCode,
			TrackingShare,
			IdentificationCountStart,
			ISNULL(CAST(IdentificationCountEnd AS NVARCHAR(10)),'~') IdentificationCountEnd
	FROM AZ.ReportPriceShareTracking WHERE UserVCode = @UserVCode
END