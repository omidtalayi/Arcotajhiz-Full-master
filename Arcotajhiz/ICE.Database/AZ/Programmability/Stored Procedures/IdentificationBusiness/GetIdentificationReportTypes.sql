CREATE PROCEDURE [AZ].[GetIdentificationReportTypes](
	@IdentificationVCode BIGINT,
	@TrackingCode BIGINT
)
AS
BEGIN
	IF @IdentificationVCode = 0
	BEGIN
		SELECT RT.* FROM AZ.IdentificationReportType (NOLOCK) IRT
		INNER JOIN AZ.ReportType (NOLOCK) RT ON IRT.ReportTypeVCode = RT.VCode
		INNER JOIN AZ.CreditRiskReport (NOLOCK) CRR ON CRR.IdentificationVCode = IRT.IdentificationVCode
		WHERE CRR.TrackingCode = @TrackingCode
	END
	ELSE
	BEGIN
		SELECT RT.* FROM AZ.IdentificationReportType (NOLOCK) IRT
		INNER JOIN AZ.ReportType (NOLOCK) RT ON IRT.ReportTypeVCode = RT.VCode
		WHERE IdentificationVCode = @IdentificationVCode
	END
END