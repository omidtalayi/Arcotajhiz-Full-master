MERGE AZ.WebHookType USING (SELECT 1,1,N'اعلان ارسال URL','NOTIFY_REPORT_URL_HAS_BEEN_SENT') AS Source ([VCode],[Code],[Name],[EnumName])
ON (WebHookType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.WebHookType USING (SELECT 2,2,N'اعلان مشاهده گزارش','NOTIFY_CUSTOMER_HAS_SEEN_REPORT') AS Source ([VCode],[Code],[Name],[EnumName])
ON (WebHookType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.WebHookType USING (SELECT 3,3,N'اعلان آماده بودن گزارش و ارسال لینک آن','NOTIFY_REPORT_IS_READY_AND_SENT_REPORTLINK') AS Source ([VCode],[Code],[Name],[EnumName])
ON (WebHookType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.WebHookType USING (SELECT 4,4,N'ارسال داده گزارش','SEND_REPORT_DATA') AS Source ([VCode],[Code],[Name],[EnumName])
ON (WebHookType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO