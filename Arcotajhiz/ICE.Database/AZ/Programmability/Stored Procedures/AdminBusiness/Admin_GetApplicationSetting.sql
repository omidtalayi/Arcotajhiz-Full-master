CREATE PROCEDURE [AZ].[Admin_GetApplicationSetting]
AS
BEGIN
	SELECT * FROM AZ.ApplicationSetting A
	WHERE A.[Key] = 'SMSCenterNumber' OR A.[Key] = 'ParsaSmsUrl' OR A.[Key] = 'DisableReceiverCellphone' OR A.[Key] = 'OnlyCallActive' OR A.[Key] = 'DisableReport' OR A.[Key] = 'ApiIsDisabled' OR A.[Key] = 'FinnotechChequeIsActive' OR A.[Key] = 'ShowSystemIsDisabled'
END
GO

