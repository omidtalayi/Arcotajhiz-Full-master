MERGE AZ.RequestRegistrationFileType USING (SELECT 1,1,N'کارت ملی','NATIONAL_CARD') AS Source ([VCode],[Code],[Name],[EnumName])
ON (RequestRegistrationFileType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.RequestRegistrationFileType USING (SELECT 2,2,N'جواز کسب و کار یا روزنامه رسمی','OFFICIAL_PAPER_OR_WORKING_PERMIT') AS Source ([VCode],[Code],[Name],[EnumName])
ON (RequestRegistrationFileType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO