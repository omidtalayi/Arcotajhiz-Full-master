CREATE PROCEDURE [AZ].[IdentificationReportLinkUpd](
	@IdentificationVCode BIGINT,
	@ReportLink NVARCHAR(MAX) 
)
AS
BEGIN
	IF EXISTS(SELECT 1 FROM AZ.Identification WHERE VCode = @IdentificationVCode AND ReportLink IS NULL)
	BEGIN
		UPDATE AZ.Identification SET ReportLink = @ReportLink WHERE  VCode = @IdentificationVCode
	END
END