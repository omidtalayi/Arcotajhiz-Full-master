MERGE AZ.SMSLogType USING (SELECT 1,1,'OTP') AS Source ([VCode],[Code],[Name])
ON (SMSLogType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name]) VALUES (Source.[VCode],Source.[Code],Source.[Name]);
GO
MERGE AZ.SMSLogType USING (SELECT 2,2,'CONFIRMATION') AS Source ([VCode],[Code],[Name])
ON (SMSLogType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name]) VALUES (Source.[VCode],Source.[Code],Source.[Name]);
GO
MERGE AZ.SMSLogType USING (SELECT 3,3,'MESSAGE') AS Source ([VCode],[Code],[Name])
ON (SMSLogType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name]) VALUES (Source.[VCode],Source.[Code],Source.[Name]);
GO
MERGE AZ.SMSLogType USING (SELECT 4,4,'LINK') AS Source ([VCode],[Code],[Name])
ON (SMSLogType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name]) VALUES (Source.[VCode],Source.[Code],Source.[Name]);
GO
MERGE AZ.SMSLogType USING (SELECT 5,5,'REPAIRLINK') AS Source ([VCode],[Code],[Name])
ON (SMSLogType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name]) VALUES (Source.[VCode],Source.[Code],Source.[Name]);
GO
MERGE AZ.SMSLogType USING (SELECT 6,6,'ADVERTISING') AS Source ([VCode],[Code],[Name])
ON (SMSLogType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name]) VALUES (Source.[VCode],Source.[Code],Source.[Name]);
GO
MERGE AZ.SMSLogType USING (SELECT 7,7,'REPAIRMESSAGE') AS Source ([VCode],[Code],[Name])
ON (SMSLogType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name]) VALUES (Source.[VCode],Source.[Code],Source.[Name]);
GO