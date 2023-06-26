MERGE AZ.FirmRegistrationState USING (SELECT 1,1,N'در حال بررسی','PENDING') AS Source ([VCode],[Code],[Name],[EnumName])
ON (FirmRegistrationState.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.FirmRegistrationState USING (SELECT 2,2,N'تایید شده','CONFIRMED') AS Source ([VCode],[Code],[Name],[EnumName])
ON (FirmRegistrationState.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.FirmRegistrationState USING (SELECT 3,3,N'رد شده','REJECTED') AS Source ([VCode],[Code],[Name],[EnumName])
ON (FirmRegistrationState.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.FirmRegistrationState USING (SELECT 4,4,N'در حال بررسی','PENDING') AS Source ([VCode],[Code],[Name],[EnumName])
ON (FirmRegistrationState.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO