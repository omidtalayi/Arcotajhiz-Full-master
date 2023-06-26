MERGE AZ.ReportType USING (SELECT 1,1,N'گزارش ساده','BASIC_INDIVIDUAL_REPORT') AS Source ([VCode],[Code],[Name],[EnumName])
ON (ReportType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ReportType USING (SELECT 2,2,N'گزارش استاندارد','STANDARD_INDIVIDUAL_REPORT') AS Source ([VCode],[Code],[Name],[EnumName])
ON (ReportType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ReportType USING (SELECT 3,3,N'گزارش رتبه','SCORING_INDIVIDUAL_REPORT') AS Source ([VCode],[Code],[Name],[EnumName])
ON (ReportType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ReportType USING (SELECT 4,4,N'درخواست رتبه','SCORING_REQUEST') AS Source ([VCode],[Code],[Name],[EnumName])
ON (ReportType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ReportType USING (SELECT 5,5,N'گزارش پیشرفته','ADVANCED_INDIVIDUAL_REPORT') AS Source ([VCode],[Code],[Name],[EnumName])
ON (ReportType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ReportType USING (SELECT 6,6,N'گزارش تاریخچه','HISTORY_INDIVIDUAL_REPORT') AS Source ([VCode],[Code],[Name],[EnumName])
ON (ReportType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ReportType USING (SELECT 7,7,N'گزارش کامل','FULL_INDIVIDUAL_REPORT') AS Source ([VCode],[Code],[Name],[EnumName])
ON (ReportType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO