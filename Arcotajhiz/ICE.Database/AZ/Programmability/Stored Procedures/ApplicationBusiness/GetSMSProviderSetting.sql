CREATE PROCEDURE [AZ].[GetSMSProviderSetting]
AS BEGIN
	SELECT SMSUserName [Username],SMSPassword [Password],SMSCenterNumber CenterNumbers,SMSAPIKey FROM
	(
		SELECT [Key],[Value] FROM [AZ].[ApplicationSetting] (NOLOCK) WHERE [Key] IN ('SMSUserName','SMSPassword','SMSCenterNumber','SMSAPIKey')
	) Main PIVOT (MIN(Value) FOR [Key] IN ([SMSUserName],[SMSPassword],[SMSCenterNumber],[SMSAPIKey])) PV
END
