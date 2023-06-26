MERGE AZ.BankPortal USING (SELECT 1,1,N'سامان پرداخت کیش','SAMAN_PARDAKHT_KISH',3,'','') AS Source ([VCode],[Code],[Name],[EnumName],[BankVCode],[AccTafzil],[AccountNumber])
ON (BankPortal.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName],[BankVCode] = Source.[BankVCode],[AccTafzil] = Source.[AccTafzil],[AccountNumber] = Source.[AccountNumber]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName],[BankVCode],[AccTafzil],[AccountNumber]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName],Source.[BankVCode],Source.[AccTafzil],Source.[AccountNumber]);
GO
MERGE AZ.BankPortal USING (SELECT 2,2,N'ایران کیش','IRAN_KISH',4,'','') AS Source ([VCode],[Code],[Name],[EnumName],[BankVCode],[AccTafzil],[AccountNumber])
ON (BankPortal.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName],[BankVCode] = Source.[BankVCode],[AccTafzil] = Source.[AccTafzil],[AccountNumber] = Source.[AccountNumber]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName],[BankVCode],[AccTafzil],[AccountNumber]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName],Source.[BankVCode],Source.[AccTafzil],Source.[AccountNumber]);
GO
MERGE AZ.BankPortal USING (SELECT 3,3,N'همراه کارت','HAMRAH_CARD',5,'','') AS Source ([VCode],[Code],[Name],[EnumName],[BankVCode],[AccTafzil],[AccountNumber])
ON (BankPortal.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName],[BankVCode] = Source.[BankVCode],[AccTafzil] = Source.[AccTafzil],[AccountNumber] = Source.[AccountNumber]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName],[BankVCode],[AccTafzil],[AccountNumber]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName],Source.[BankVCode],Source.[AccTafzil],Source.[AccountNumber]);
GO
MERGE AZ.BankPortal USING (SELECT 4,4,N'نگین','NEGIN',6,'','') AS Source ([VCode],[Code],[Name],[EnumName],[BankVCode],[AccTafzil],[AccountNumber])
ON (BankPortal.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName],[BankVCode] = Source.[BankVCode],[AccTafzil] = Source.[AccTafzil],[AccountNumber] = Source.[AccountNumber]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName],[BankVCode],[AccTafzil],[AccountNumber]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName],Source.[BankVCode],Source.[AccTafzil],Source.[AccountNumber]);
GO
MERGE AZ.BankPortal USING (SELECT 5,5,N'پرداخت الکترونیک سپهر','PAS',7,'','') AS Source ([VCode],[Code],[Name],[EnumName],[BankVCode],[AccTafzil],[AccountNumber])
ON (BankPortal.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName],[BankVCode] = Source.[BankVCode],[AccTafzil] = Source.[AccTafzil],[AccountNumber] = Source.[AccountNumber]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName],[BankVCode],[AccTafzil],[AccountNumber]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName],Source.[BankVCode],Source.[AccTafzil],Source.[AccountNumber]);
GO
MERGE AZ.BankPortal USING (SELECT 6,6,N'تابان آتی پرداز','TOP',8,'','') AS Source ([VCode],[Code],[Name],[EnumName],[BankVCode],[AccTafzil],[AccountNumber])
ON (BankPortal.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName],[BankVCode] = Source.[BankVCode],[AccTafzil] = Source.[AccTafzil],[AccountNumber] = Source.[AccountNumber]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName],[BankVCode],[AccTafzil],[AccountNumber]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName],Source.[BankVCode],Source.[AccTafzil],Source.[AccountNumber]);
GO
MERGE AZ.BankPortal USING (SELECT 7,7,N'مرات','IMERAT',9,'','') AS Source ([VCode],[Code],[Name],[EnumName],[BankVCode],[AccTafzil],[AccountNumber])
ON (BankPortal.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName],[BankVCode] = Source.[BankVCode],[AccTafzil] = Source.[AccTafzil],[AccountNumber] = Source.[AccountNumber]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName],[BankVCode],[AccTafzil],[AccountNumber]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName],Source.[BankVCode],Source.[AccTafzil],Source.[AccountNumber]);
GO
MERGE AZ.BankPortal USING (SELECT 8,8,N'به پرداخت ملت','BehPardakhtMellat',2,'','') AS Source ([VCode],[Code],[Name],[EnumName],[BankVCode],[AccTafzil],[AccountNumber])
ON (BankPortal.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName],[BankVCode] = Source.[BankVCode],[AccTafzil] = Source.[AccTafzil],[AccountNumber] = Source.[AccountNumber]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName],[BankVCode],[AccTafzil],[AccountNumber]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName],Source.[BankVCode],Source.[AccTafzil],Source.[AccountNumber]);
GO