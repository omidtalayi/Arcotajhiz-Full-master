MERGE AZ.PriceListType USING (SELECT 1,1,N'برای خودم','FOR_ME') AS Source ([VCode],[Code],[Name],[EnumName])
ON (PriceListType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.PriceListType USING (SELECT 2,2,N'برای ارسال به دیگران','FOR_SENDING_TO_OTHERS') AS Source ([VCode],[Code],[Name],[EnumName])
ON (PriceListType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO