MERGE AZ.IdentificationState USING (SELECT 1,1,N'منتظر تایید شخص','WAITING_INDIVIDUAL_CONFIRMATION') AS Source ([VCode],[Code],[Name],[EnumName])
ON (IdentificationState.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.IdentificationState USING (SELECT 2,2,N'منتظر تایید شاهکار','WAITING_SHAHKAR_CONFIRMATION') AS Source ([VCode],[Code],[Name],[EnumName])
ON (IdentificationState.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.IdentificationState USING (SELECT 3,3,N'تایید شده','CONFIRMED') AS Source ([VCode],[Code],[Name],[EnumName])
ON (IdentificationState.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.IdentificationState USING (SELECT 4,4,N'مشاهده شده','SEEN') AS Source ([VCode],[Code],[Name],[EnumName])
ON (IdentificationState.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.IdentificationState USING (SELECT 5,5,N'لینک تایید منقضی شده','VERIFICATION_EXPIRED') AS Source ([VCode],[Code],[Name],[EnumName])
ON (IdentificationState.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.IdentificationState USING (SELECT 6,6,N'رد شده توسط شاهکار','SHAHKAR_REJECTED') AS Source ([VCode],[Code],[Name],[EnumName])
ON (IdentificationState.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.IdentificationState USING (SELECT 7,7,N'رد شده توسط شخص','CONFIRMATION_DENIED') AS Source ([VCode],[Code],[Name],[EnumName])
ON (IdentificationState.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.IdentificationState USING (SELECT 8,8,N'موبایل یافت نشد','MOBILE_NOT_FOUND') AS Source ([VCode],[Code],[Name],[EnumName])
ON (IdentificationState.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.IdentificationState USING (SELECT 9,9,N'شخص فاقد اطلاعات اعتباری می باشد','INDIVIDUAL_DOES_NOT_HAVE_INFORMATION') AS Source ([VCode],[Code],[Name],[EnumName])
ON (IdentificationState.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.IdentificationState USING (SELECT 10,10,N'گزارش منقضی شده','REPORT_EXPIRED') AS Source ([VCode],[Code],[Name],[EnumName])
ON (IdentificationState.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.IdentificationState USING (SELECT 11,11,N'پرداخت شده','PAID') AS Source ([VCode],[Code],[Name],[EnumName])
ON (IdentificationState.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.IdentificationState USING (SELECT 12,12,N'شاهکار تایید شده','SHAHKAR_VERIFIED') AS Source ([VCode],[Code],[Name],[EnumName])
ON (IdentificationState.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.IdentificationState USING (SELECT 13,13,N'قابل مشاهده','READY_FOR_SEE') AS Source ([VCode],[Code],[Name],[EnumName])
ON (IdentificationState.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.IdentificationState USING (SELECT 14,14,N'عدم تایید توسط شخص','CONFIRMATION_NOT_RESPONDED') AS Source ([VCode],[Code],[Name],[EnumName])
ON (IdentificationState.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.IdentificationState USING (SELECT 15,15,N'عدم پرداخت توسط شخص','NOT_PAID') AS Source ([VCode],[Code],[Name],[EnumName])
ON (IdentificationState.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.IdentificationState USING (SELECT 16,16,N'رد شده به دلیل ورود کد ملی اشتباه','CONFIRMATION_DENIED_INCORRECT_NATIONAL_CODE') AS Source ([VCode],[Code],[Name],[EnumName])
ON (IdentificationState.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.IdentificationState USING (SELECT 17,17,N'لینک صفحه تایید شماره همراه دریافت شده','ICS24_OTP_LINK_RECEIVED') AS Source ([VCode],[Code],[Name],[EnumName])
ON (IdentificationState.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.IdentificationState USING (SELECT 18,18,N'تایید شده توسط ICS24','ICS24_CONFIRMED') AS Source ([VCode],[Code],[Name],[EnumName])
ON (IdentificationState.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.IdentificationState USING (SELECT 19,19,N'قفل شده به دلیل ورود OTP اشتباه','OTP_LOCK') AS Source ([VCode],[Code],[Name],[EnumName])
ON (IdentificationState.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO