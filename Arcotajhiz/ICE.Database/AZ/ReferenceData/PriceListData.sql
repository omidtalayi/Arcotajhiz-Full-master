MERGE AZ.PriceList USING (SELECT 1,N'قیمت گزارش','ICS_REPORT_PRICE_FOR_ME',94000,1,1) AS Source ([VCode],[Name],[EnumName],[Price],[PriceListTypeVCode],[IdentificationTypeVCode])
ON (PriceList.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name],[EnumName] = Source.[EnumName],[Price] = Source.[Price],[PriceListTypeVCode] = Source.[PriceListTypeVCode],[IdentificationTypeVCode] = Source.[IdentificationTypeVCode]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name],[EnumName],[Price],[PriceListTypeVCode],[IdentificationTypeVCode]) VALUES (Source.[VCode],Source.[Name],Source.[EnumName],Source.[Price],Source.[PriceListTypeVCode],Source.[IdentificationTypeVCode]);
GO
MERGE AZ.PriceList USING (SELECT 2,N'مالیات بر ارزش افزوده','ICS_TAX_FOR_ME',8460,1,1) AS Source ([VCode],[Name],[EnumName],[Price],[PriceListTypeVCode],[IdentificationTypeVCode])
ON (PriceList.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name],[EnumName] = Source.[EnumName],[Price] = Source.[Price],[PriceListTypeVCode] = Source.[PriceListTypeVCode],[IdentificationTypeVCode] = Source.[IdentificationTypeVCode]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name],[EnumName],[Price],[PriceListTypeVCode],[IdentificationTypeVCode]) VALUES (Source.[VCode],Source.[Name],Source.[EnumName],Source.[Price],Source.[PriceListTypeVCode],Source.[IdentificationTypeVCode]);
GO
MERGE AZ.PriceList USING (SELECT 3,N'قیمت گزارش حقوقی','ICS_REPORT_PRICE_FIRM',150000,1,2) AS Source ([VCode],[Name],[EnumName],[Price],[PriceListTypeVCode],[IdentificationTypeVCode])
ON (PriceList.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name],[EnumName] = Source.[EnumName],[Price] = Source.[Price],[PriceListTypeVCode] = Source.[PriceListTypeVCode],[IdentificationTypeVCode] = Source.[IdentificationTypeVCode]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name],[EnumName],[Price],[PriceListTypeVCode],[IdentificationTypeVCode]) VALUES (Source.[VCode],Source.[Name],Source.[EnumName],Source.[Price],Source.[PriceListTypeVCode],Source.[IdentificationTypeVCode]);
GO
MERGE AZ.PriceList USING (SELECT 4,N'مالیات بر ارزش افزوده حقوقی','ICS_TAX_FIRM',13500,1,2) AS Source ([VCode],[Name],[EnumName],[Price],[PriceListTypeVCode],[IdentificationTypeVCode])
ON (PriceList.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name],[EnumName] = Source.[EnumName],[Price] = Source.[Price],[PriceListTypeVCode] = Source.[PriceListTypeVCode],[IdentificationTypeVCode] = Source.[IdentificationTypeVCode]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name],[EnumName],[Price],[PriceListTypeVCode],[IdentificationTypeVCode]) VALUES (Source.[VCode],Source.[Name],Source.[EnumName],Source.[Price],Source.[PriceListTypeVCode],Source.[IdentificationTypeVCode]);
GO
MERGE AZ.PriceList USING (SELECT 5,N'قیمت گزارش چک برگشتی','FINOTECH_CHEQUE',50000,1,NULL) AS Source ([VCode],[Name],[EnumName],[Price],[PriceListTypeVCode],[IdentificationTypeVCode])
ON (PriceList.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name],[EnumName] = Source.[EnumName],[Price] = Source.[Price],[PriceListTypeVCode] = Source.[PriceListTypeVCode],[IdentificationTypeVCode] = Source.[IdentificationTypeVCode]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name],[EnumName],[Price],[PriceListTypeVCode],[IdentificationTypeVCode]) VALUES (Source.[VCode],Source.[Name],Source.[EnumName],Source.[Price],Source.[PriceListTypeVCode],Source.[IdentificationTypeVCode]);
GO