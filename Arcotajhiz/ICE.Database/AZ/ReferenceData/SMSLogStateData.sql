MERGE AZ.SMSLogState USING (SELECT 1,0,N'وضعيت دريافت نشده(يا پيامک در صف ارسال قرار دارد)',Null) AS Source ([VCode],[Code],[Name],[EnumName])
ON (SMSLogState.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code]= Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) 
	VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.SMSLogState USING (SELECT 2,1,N'رسيده به گوشي',Null) AS Source ([VCode],[Code],[Name],[EnumName])
ON (SMSLogState.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code]= Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) 
	VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.SMSLogState USING (SELECT 3,2,N'نرسيده به گوشي',Null) AS Source ([VCode],[Code],[Name],[EnumName])
ON (SMSLogState.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code]= Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) 
	VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.SMSLogState USING (SELECT 4,8,N'رسيده به مخابرات',Null) AS Source ([VCode],[Code],[Name],[EnumName])
ON (SMSLogState.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code]= Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) 
	VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.SMSLogState USING (SELECT 5,16,N'نرسيده به مخابرات',Null) AS Source ([VCode],[Code],[Name],[EnumName])
ON (SMSLogState.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code]= Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) 
	VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.SMSLogState USING (SELECT 6,27,N'شماره گيرنده جزو ليست سياه مي باشد',Null) AS Source ([VCode],[Code],[Name],[EnumName])
ON (SMSLogState.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code]= Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) 
	VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.SMSLogState USING (SELECT 7,-1,N'شناسه ارسال شده اشتباه است',Null) AS Source ([VCode],[Code],[Name],[EnumName])
ON (SMSLogState.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code]= Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) 
	VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO