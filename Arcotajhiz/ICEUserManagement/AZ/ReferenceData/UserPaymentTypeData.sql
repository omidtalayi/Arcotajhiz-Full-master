MERGE AZ.UserPaymentType USING (SELECT 1,1,N'پرداخت توسط شخص','PAY_BY_CUSTOMER') AS Source ([VCode],[Code],[Name],[EnumName])
ON (UserPaymentType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.UserPaymentType USING (SELECT 2,2,N'پرداخت از اعتبار','PAY_FROM_CREDIT') AS Source ([VCode],[Code],[Name],[EnumName])
ON (UserPaymentType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.UserPaymentType USING (SELECT 3,3,N'پرداخت توسط دریافت کننده','PAY_BY_RECEIVER') AS Source ([VCode],[Code],[Name],[EnumName])
ON (UserPaymentType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO