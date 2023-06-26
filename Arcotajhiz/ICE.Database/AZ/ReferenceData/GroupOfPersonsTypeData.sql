MERGE AZ.IdentificationType USING (SELECT 1,1,N'شخص حقیقی','INDIVIDUAL') AS Source ([VCode],[Code],[Name],[EnumName])
ON (IdentificationType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.IdentificationType USING (SELECT 2,2,N'شخص حقوقی','FIRM') AS Source ([VCode],[Code],[Name],[EnumName])
ON (IdentificationType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO