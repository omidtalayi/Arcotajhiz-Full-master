MERGE AZ.ICSReportSourceType USING (SELECT 1,1,N'بانک','BANK') AS Source ([VCode],[Code],[Name],[EnumName])
ON (ICSReportSourceType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSourceType USING (SELECT 2,2,N'موسسه اعتباری','CREDIT_INSTITUTION') AS Source ([VCode],[Code],[Name],[EnumName])
ON (ICSReportSourceType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSourceType USING (SELECT 3,3,N'لیزینگ','LEASING') AS Source ([VCode],[Code],[Name],[EnumName])
ON (ICSReportSourceType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSourceType USING (SELECT 4,4,N'کارگزاری','BROKERAGE') AS Source ([VCode],[Code],[Name],[EnumName])
ON (ICSReportSourceType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSourceType USING (SELECT 5,5,N'سازمان','ORGANIZATION') AS Source ([VCode],[Code],[Name],[EnumName])
ON (ICSReportSourceType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSourceType USING (SELECT 6,6,N'صندوق اعتباری','CREDIT_UNION') AS Source ([VCode],[Code],[Name],[EnumName])
ON (ICSReportSourceType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSourceType USING (SELECT 7,7,N'بیمه','INSURANCE') AS Source ([VCode],[Code],[Name],[EnumName])
ON (ICSReportSourceType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO