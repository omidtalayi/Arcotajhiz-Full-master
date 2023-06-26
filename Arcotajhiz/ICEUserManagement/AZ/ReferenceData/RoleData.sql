MERGE AZ.[Role] USING (SELECT 1,1,N'داشبورد','DASHBOARD',3) AS Source ([VCode],[Code],[Name],[EnumName],[SubSystemVCode])
ON ([Role].VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName],[SubSystemVCode] = Source.[SubSystemVCode]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName],[SubSystemVCode]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName],Source.[SubSystemVCode]);
GO
MERGE AZ.[Role] USING (SELECT 2,2,N'مالی','FINANCIAL',3) AS Source ([VCode],[Code],[Name],[EnumName],[SubSystemVCode])
ON ([Role].VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName],[SubSystemVCode] = Source.[SubSystemVCode]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName],[SubSystemVCode]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName],Source.[SubSystemVCode]);
GO
MERGE AZ.[Role] USING (SELECT 3,3,N'گزارشات','REPORT',3) AS Source ([VCode],[Code],[Name],[EnumName],[SubSystemVCode])
ON ([Role].VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName],[SubSystemVCode] = Source.[SubSystemVCode]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName],[SubSystemVCode]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName],Source.[SubSystemVCode]);
GO
MERGE AZ.[Role] USING (SELECT 6,6,N'محتوا','CONTENT',3) AS Source ([VCode],[Code],[Name],[EnumName],[SubSystemVCode])
ON ([Role].VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName],[SubSystemVCode] = Source.[SubSystemVCode]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName],[SubSystemVCode]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName],Source.[SubSystemVCode]);
GO
MERGE AZ.[Role] USING (SELECT 7,7,N'کاربران','USER',3) AS Source ([VCode],[Code],[Name],[EnumName],[SubSystemVCode])
ON ([Role].VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName],[SubSystemVCode] = Source.[SubSystemVCode]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName],[SubSystemVCode]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName],Source.[SubSystemVCode]);
GO
MERGE AZ.[Role] USING (SELECT 8,8,N'منو','MENU',3) AS Source ([VCode],[Code],[Name],[EnumName],[SubSystemVCode])
ON ([Role].VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName],[SubSystemVCode] = Source.[SubSystemVCode]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName],[SubSystemVCode]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName],Source.[SubSystemVCode]);
GO
MERGE AZ.[Role] USING (SELECT 9,9,N'تنظیمات','SETTING',3) AS Source ([VCode],[Code],[Name],[EnumName],[SubSystemVCode])
ON ([Role].VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName],[SubSystemVCode] = Source.[SubSystemVCode]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName],[SubSystemVCode]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName],Source.[SubSystemVCode]);
GO