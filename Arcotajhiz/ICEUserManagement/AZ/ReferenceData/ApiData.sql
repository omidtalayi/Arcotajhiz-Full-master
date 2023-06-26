MERGE AZ.Api USING (SELECT 1,1,N'دریافت توکن','AUTHENTICATE') AS Source ([VCode],[Code],[Name],[EnumName])
ON (Api.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.Api USING (SELECT 2,2,N'بررسی وجود گزارش مشتری حقیقی','INDIVIDUAL_REPORT_EXISTENCE') AS Source ([VCode],[Code],[Name],[EnumName])
ON (Api.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.Api USING (SELECT 3,3,N'بررسی وجود گزارش مشتری حقوقی','FIRM_REPORT_EXISTENCE') AS Source ([VCode],[Code],[Name],[EnumName])
ON (Api.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.Api USING (SELECT 4,4,N'دریافت اطلاعات مشتری حقیقی','GET_INDIVIDUAL_INFORMATION') AS Source ([VCode],[Code],[Name],[EnumName])
ON (Api.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.Api USING (SELECT 5,5,N'ارسال لینک به مشتری','SEND_REPORT_URL_TO_CUSTOMER') AS Source ([VCode],[Code],[Name],[EnumName])
ON (Api.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.Api USING (SELECT 6,6,N'بررسی صحت اطلاعات مشتری','KYC_CHECK') AS Source ([VCode],[Code],[Name],[EnumName])
ON (Api.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.Api USING (SELECT 7,7,N'ارسال اس ام اس','SEND_SMS') AS Source ([VCode],[Code],[Name],[EnumName])
ON (Api.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.Api USING (SELECT 8,8,N'دریافت منابع اطلاعاتی ICS','GET_ICS_REPORT_SOURCES') AS Source ([VCode],[Code],[Name],[EnumName])
ON (Api.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.Api USING (SELECT 9,9,N'ثبت درخواست از پنل','ADD_IDENTIFICATION_FROM_FIRM') AS Source ([VCode],[Code],[Name],[EnumName])
ON (Api.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.Api USING (SELECT 10,10,N'دریافت داده گزارش','GET_REPORT_DATA') AS Source ([VCode],[Code],[Name],[EnumName])
ON (Api.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.Api USING (SELECT 11,11,N'ثبت درخواست گزارش اعتباری همکار','SET_INDIVIDUAL_INFORMATION_REQUEST') AS Source ([VCode],[Code],[Name],[EnumName])
ON (Api.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.Api USING (SELECT 12,12,N'ارسال تایید شماره همراه','VERIFY_CELLPHONE') AS Source ([VCode],[Code],[Name],[EnumName])
ON (Api.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.Api USING (SELECT 13,13,N'دریافت وضعیت گزارش و لینک','GET_REPORT_STATUS') AS Source ([VCode],[Code],[Name],[EnumName])
ON (Api.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.Api USING (SELECT 14,14,N'دریافت لیست گزارشات','GET_IDENTIFICATION_LIST') AS Source ([VCode],[Code],[Name],[EnumName])
ON (Api.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.Api USING (SELECT 15,15,N'دریافت گزارش گروه اشخاص','GROUP_OF_PERSONS') AS Source ([VCode],[Code],[Name],[EnumName])
ON (Api.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.Api USING (SELECT 16,16,N'دریافت گزارش صورت های مالی','JAAM') AS Source ([VCode],[Code],[Name],[EnumName])
ON (Api.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.Api USING (SELECT 17,17,N'دریافت گزارش استعلام شرکت','COMPANY_PERSON') AS Source ([VCode],[Code],[Name],[EnumName])
ON (Api.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO