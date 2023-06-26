MERGE AZ.IdentificationInvoiceState USING (SELECT 1,1,N'صادر شده','ISSUED') AS Source ([VCode],[Code],[Name],[EnumName])
ON (IdentificationInvoiceState.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.IdentificationInvoiceState USING (SELECT 2,2,N'بچ ساخته شده','BATCH_CREATED') AS Source ([VCode],[Code],[Name],[EnumName])
ON (IdentificationInvoiceState.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.IdentificationInvoiceState USING (SELECT 3,3,N'ارسال جهت تسويه','SENT_TO_CLEAR') AS Source ([VCode],[Code],[Name],[EnumName])
ON (IdentificationInvoiceState.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.IdentificationInvoiceState USING (SELECT 4,4,N'تسويه شده','CLEARED') AS Source ([VCode],[Code],[Name],[EnumName])
ON (IdentificationInvoiceState.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO