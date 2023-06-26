MERGE AZ.AccountingType USING (SELECT 1,1,N'شارژ اعتبار','CREDIT', N'اعتبار') AS Source ([VCode],[Code],[Name],[EnumName],[TypeName])
ON (AccountingType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName],[TypeName] = Source.[TypeName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName],[TypeName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName],Source.[TypeName]);
GO
MERGE AZ.AccountingType USING (SELECT 2,2,N'پرداخت از اعتبار','PAY_CREDIT', N'اعتبار') AS Source ([VCode],[Code],[Name],[EnumName],[TypeName])
ON (AccountingType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName],[TypeName] = Source.[TypeName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName],[TypeName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName],Source.[TypeName]);
GO
MERGE AZ.AccountingType USING (SELECT 3,3,N'هزینه ثبت و تشکیل پرونده','PAY_PRESENTER', N'اعتبار') AS Source ([VCode],[Code],[Name],[EnumName],[TypeName])
ON (AccountingType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName],[TypeName] = Source.[TypeName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName],[TypeName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName],Source.[TypeName]);
GO
MERGE AZ.AccountingType USING (SELECT 4,4,N'برگشت اعتبار','PAY_CREDIT_RETURN', N'اعتبار') AS Source ([VCode],[Code],[Name],[EnumName],[TypeName])
ON (AccountingType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName],[TypeName] = Source.[TypeName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName],[TypeName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName],Source.[TypeName]);
GO
MERGE AZ.AccountingType USING (SELECT 5,5,N'هزینه دریافتی بابت API پنل حقوقی','PAY_API_FIRM', N'اعتبار') AS Source ([VCode],[Code],[Name],[EnumName],[TypeName])
ON (AccountingType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName],[TypeName] = Source.[TypeName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName],[TypeName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName],Source.[TypeName]);
GO