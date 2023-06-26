MERGE AZ.PagesType USING (SELECT 1,1,N'بلاگ','BLOG') AS Source ([VCode],[Code],[Name],[EnumName])
ON (PagesType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.PagesType USING (SELECT 2,2,N'صفحه','PAGE') AS Source ([VCode],[Code],[Name],[EnumName])
ON (PagesType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO