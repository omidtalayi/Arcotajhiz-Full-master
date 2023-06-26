CREATE PROCEDURE [AZ].[GetICSReportSources]
AS
BEGIN
	SELECT	*,
			('<ICSReportSourceTypes>' + (SELECT * FROM AZ.ICSReportSourceType (NOLOCK) WHERE VCode = IRS.ICSReportSourceTYpeVCode FOR XML PATH('ICSReportSourceType')) + '</ICSReportSourceTypes>') ICSReportSourceType
	FROM AZ.ICSReportSource IRS
END