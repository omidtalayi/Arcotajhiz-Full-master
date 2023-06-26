MERGE AZ.RequestRegistrationType USING (SELECT 1,1,N'ثبت درخواست تغییر مشخصات','REQUEST_CHANGE') AS Source ([VCode],[Code],[Name],[EnumName])
ON (RequestRegistrationType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.RequestRegistrationType USING (SELECT 2,2,N'ثبت درخواست Api','REQUEST_API_PANEL') AS Source ([VCode],[Code],[Name],[EnumName])
ON (RequestRegistrationType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO