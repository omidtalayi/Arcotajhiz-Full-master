MERGE AZ.ICSReportSource USING (SELECT 1,1,N'بانک اقتصادنوین','BANK_EGHTESAD_NOVIN') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 2,1,N'بانک انصار','BANK_ANSAR') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 3,1,N'بانک ایران زمین','BANK_IRAN_ZAMIN') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 4,1,N'بانک ایران و ونزوئلا','BANK_IRAN_VENEZUELA') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 5,1,N'بانک آینده','BANK_AYANDEH') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 6,1,N'بانک پارسیان','BANK_PARSIAN') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 7,1,N'بانک پست بانک','BANK_POST_BANK') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 8,1,N'بانک تجارت','BANK_TEJARAT') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 9,1,N'بانک توسعه تعاون','BANK_TOSE_TAAVON') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 10,1,N'بانک توسعه صادرات ایران','BANK_TOSE_SADERAT_IRAN') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 11,1,N'بانک حکمت ایرانیان','BANK_HEKMAT_IRANIAN') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 12,1,N'بانک دی','BANK_DEY') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 13,1,N'بانک رفاه کارگران','BANK_REFAH_KARGARAN') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 14,1,N'بانک سامان','BANK_SAMAN') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 15,1,N'بانک سپه','BANK_SEPAH') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 16,1,N'بانک سرمايه','BANK_SARMAYEH') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 17,1,N'بانک سینا','BANK_SINA') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 18,1,N'بانک شهر','BANK_SHAHR') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 19,1,N'بانک صادرات ایران','BANK_SADERAT_IRAN') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 20,1,N'بانک صنعت و معدن','BANK_SANAT_VA_MADAN') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 21,1,N'بانک قرض الحسنه مهر ایران','BANK_GHARZOALHASANE_MEHR_IRAN') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 22,1,N'بانک قوامین','BANK_GHAVAMIN') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 23,1,N'بانک کارآفرین','BANK_KARAFARIN') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 24,1,N'بانک کشاورزی','BANK_KESHAVARZI') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 25,1,N'بانک گردشگری','BANK_GARDESHGARI') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 26,1,N'بانک مسکن','BANK_MASKAN') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 27,1,N'بانک ملت','BANK_MELLAT') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 28,1,N'بانک ملی','BANK_MELLI') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 29,2,N'موسسه اعتباری تعاونی اعتبار ثامن','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 30,2,N'موسسه اعتباری توسعه','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 31,2,N'موسسه اعتباری عسکریه','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 32,2,N'موسسه اعتباری کاسپین','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 33,2,N'موسسه مالی و اعتباری کوثر','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 34,3,N'لیزینگ ارزش آفرین گلرنگ','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 35,3,N'لیزینگ اقتصاد نوین','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 36,3,N'لیزینگ امید','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 37,3,N'لیزینگ انصار','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 38,3,N'لیزینگ ایران','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 39,3,N'لیزینگ ایران و شرق','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 40,3,N'لیزینگ ایرانیان','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 41,3,N'لیزینگ آتیه الوند','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 42,3,N'لیزینگ آتیه صبا','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 43,3,N'لیزینگ آریا دانا','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 44,3,N'لیزینگ بهمن لیزینگ','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 45,3,N'لیزینگ جامع سینا','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 46,3,N'لیزینگ خودرو غدیر','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 47,3,N'لیزینگ دی','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 48,3,N'لیزینگ رایان سایپا','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 49,3,N'لیزینگ سپهر صادرات','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 50,3,N'لیزینگ شهر','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 51,3,N'لیزینگ صنعت ‌و معدن','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 52,3,N'لیزینگ عظیم خودرو','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 53,3,N'لیزینگ گلدیران نوین','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 54,3,N'لیزینگ ماشین آلات سنگین ایرانیان','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 55,3,N'لیزینگ ماشین آلات و تجهیزات پاسارگاد','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 56,3,N'لیزینگ مشرق زمین','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 57,3,N'لیزینگ واسپاری سپهر پارس','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 58,3,N'لیزینگ واسپاری ملت','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 59,4,N'کارگزاری امین سهم','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 60,4,N'کارگزاری پارس نمودگر','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 61,5,N'اتاق بازرگانی، صنایع، معادن','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 62,5,N'دادستانی کل کشور','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 63,5,N'سازمان جمع آوری و فروش اموال تملیکی','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 64,5,N'سازمان خصوصی سازی ایران','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 65,5,N'سامان سرمایه نانو','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 66,5,N'شرکت پرشیا خودرو','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 67,5,N'شرکت پیشگام الکترونیک قویم','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 68,5,N'شرکت خدمات رایان اقتصادنوین','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 69,5,N'شرکت داده گستر عصر نوین (های وب)','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 70,5,N'شرکت سنجش فناوری خاورمیانه','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 71,5,N'شرکت کاریزان خودرو','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 72,5,N'طرح های فنی و اعتباری امور صنایع و اقتصادی وزارت صنعت، معدن و تجارت','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 73,5,N'گمرک جمهوری اسلامی ایران','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 74,5,N'وزارت صنعت معدن تجارت','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 75,7,N'بیمه البرز','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 76,7,N'بیمه تجارت نو','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 77,7,N'بیمه ملت','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 78,6,N'شرکت سنجش فناوری خاورمیانه','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 79,6,N'صندوق بیمه سرمایه گذاری فعالیت های معدنی','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 80,6,N'صندوق پژوهش و فناوری استان فارس','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 81,6,N'صندوق پژوهش و فناوری اصفهان','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 82,6,N'صندوق پژوهش و فناوری پرشین داروی البرز','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 83,6,N'صندوق پژوهش و فناوری دانشگاه تهران','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 84,6,N'صندوق پژوهش و فناوری صنعت برق و انرژی','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 85,6,N'صندوق پژوهش و فناوری غیردولتی تجهیزات پزشکی','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 86,6,N'صندوق پژوهش و فناوری غیردولتی توسعه صادرات و فناوری شریف','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 87,6,N'صندوق پژوهش و فناوری غیردولتی توسعه صادرات و فناوری نانو','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 88,6,N'صندوق پژوهش و فناوری یزد','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 89,6,N'صندوق پژوهشی و فناوری غیر دولتی توسعه فناوری ایران','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 90,6,N'صندوق پژوهشی و فناوری غیردولتی توسعه فناوری ایرانیان','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 91,6,N'صندوق توسعه صنایع دریایی','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 92,6,N'صندوق توسعه فناوری های نوین','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 93,6,N'صندوق حمایت از پژوهشگران و فناوران کشور','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 94,6,N'صندوق حمایت از تحقیقات و توسعه صنایع الکترونیک (صحا)','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 95,6,N'صندوق حمایت از سرمایه گذاری زیست فناوری','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 96,6,N'صندوق رفاه دانشجویان وزارت بهداشت','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 97,6,N'صندوق رفاه دانشجویان وزارت علوم، تحقیقات و فناوری','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 98,6,N'صندوق ضمانت سرمایه گذاری تعاون','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 99,6,N'صندوق ضمانت سرمایه گذاری صنایع کوچک','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 100,6,N'صندوق ضمانت صادرات ایران','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 101,6,N'صندوق غیردولتی پژوهش و فناوری کریمه','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 102,6,N'صندوق قرض الحسنه تک شعبه ای پارسه آیریک','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 103,6,N'صندوق کارآفرینی امید','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 104,6,N'صندوق مالی توسعه تکنولوژی ایران','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ICSReportSource USING (SELECT 105,6,N'صندوق نوآوری و شکوفایی','') AS Source ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName])
ON (ICSReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ICSReportSourceTypeVCode] = Source.[ICSReportSourceTypeVCode],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ICSReportSourceTypeVCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[ICSReportSourceTypeVCode],Source.[Name],Source.[EnumName]);
GO