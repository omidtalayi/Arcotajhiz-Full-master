MERGE AZ.UserType USING (SELECT 1,1,N'شریک','PARTNER') AS Source ([VCode],[Code],[Name],[EnumName])
ON (UserType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.UserType USING (SELECT 2,2,N'کاربر عادی','NORMAL_USER') AS Source ([VCode],[Code],[Name],[EnumName])
ON (UserType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.UserType USING (SELECT 3,3,N'کاربر رهگیر','TRACKING') AS Source ([VCode],[Code],[Name],[EnumName])
ON (UserType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO