MERGE AZ.PortalPaymentType USING (SELECT 1,1,N'پرداخت','PAYMENT') AS Source ([VCode],[Code],[Name],[EnumName])
ON (PortalPaymentType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.PortalPaymentType USING (SELECT 2,2,N'شارژ اعتبار','CREDIT') AS Source ([VCode],[Code],[Name],[EnumName])
ON (PortalPaymentType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.PortalPaymentType USING (SELECT 3,3,N'هزینه بازاریابی','PRESENTER_PAYMENT') AS Source ([VCode],[Code],[Name],[EnumName])
ON (PortalPaymentType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.PortalPaymentType USING (SELECT 4,4,N'هزینه دریافتی بابت API پنل حقوقی','API_FIRM_PAYMENT') AS Source ([VCode],[Code],[Name],[EnumName])
ON (PortalPaymentType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO