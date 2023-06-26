MERGE AZ.RequestRegistrationState USING (SELECT 1,1,N'پرداخت نشده','NOT_PAID') AS Source ([VCode],[Code],[Name],[EnumName])
ON (RequestRegistrationState.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.RequestRegistrationState USING (SELECT 2,2,N'تایید شده','CONFIRMED') AS Source ([VCode],[Code],[Name],[EnumName])
ON (RequestRegistrationState.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.RequestRegistrationState USING (SELECT 3,3,N'رد شده','REJECTED') AS Source ([VCode],[Code],[Name],[EnumName])
ON (RequestRegistrationState.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.RequestRegistrationState USING (SELECT 4,4,N'در حال بررسی','PENDING') AS Source ([VCode],[Code],[Name],[EnumName])
ON (RequestRegistrationState.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.RequestRegistrationState USING (SELECT 5,5,N'پرداخت شده','PAID') AS Source ([VCode],[Code],[Name],[EnumName])
ON (RequestRegistrationState.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.RequestRegistrationState USING (SELECT 6,6,N'در انتظار انتقال به محیط لایو','PENDING_LIVE') AS Source ([VCode],[Code],[Name],[EnumName])
ON (RequestRegistrationState.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.RequestRegistrationState USING (SELECT 7,7,N'اتمام مراحل','DONE') AS Source ([VCode],[Code],[Name],[EnumName])
ON (RequestRegistrationState.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO