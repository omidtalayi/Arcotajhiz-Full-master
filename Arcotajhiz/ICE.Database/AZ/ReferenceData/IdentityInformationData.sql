MERGE AZ.IdentityInformationState USING (SELECT 1,1,N'ثبت شده','SUBMITTED') AS Source ([VCode],[Code],[Name],[EnumName])
ON (IdentityInformationState.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.IdentityInformationState USING (SELECT 2,2,N'ثبت شده','PAID') AS Source ([VCode],[Code],[Name],[EnumName])
ON (IdentityInformationState.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.IdentityInformationState USING (SELECT 3,3,N'تایید شده','ACCEPTED') AS Source ([VCode],[Code],[Name],[EnumName])
ON (IdentityInformationState.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.IdentityInformationState USING (SELECT 4,4,N'قابل مشاهده','READY_FOR_SEE') AS Source ([VCode],[Code],[Name],[EnumName])
ON (IdentityInformationState.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.IdentityInformationState USING (SELECT 5,5,N'مشاهده شده','SEEN') AS Source ([VCode],[Code],[Name],[EnumName])
ON (IdentityInformationState.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO