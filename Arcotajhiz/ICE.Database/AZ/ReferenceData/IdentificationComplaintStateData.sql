MERGE AZ.IdentificationComplaintState USING (SELECT 1,N'ثبت شده','SUBMITTED') AS Source ([VCode],[Name],[EnumName])
ON (IdentificationComplaintState.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.IdentificationComplaintState USING (SELECT 2,N'تایید شده','APPROVED') AS Source ([VCode],[Name],[EnumName])
ON (IdentificationComplaintState.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.IdentificationComplaintState USING (SELECT 3,N'مشاهده شده توسط مشاوره رتبه بندی','SEEN') AS Source ([VCode],[Name],[EnumName])
ON (IdentificationComplaintState.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.IdentificationComplaintState USING (SELECT 4,N'ارجاع داده شده به بانک','REFERRED') AS Source ([VCode],[Name],[EnumName])
ON (IdentificationComplaintState.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.IdentificationComplaintState USING (SELECT 5,N'پاسخ داده شده توسط بانک','REPLIED') AS Source ([VCode],[Name],[EnumName])
ON (IdentificationComplaintState.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.IdentificationComplaintState USING (SELECT 6,N'بسته شده','CLOSED') AS Source ([VCode],[Name],[EnumName])
ON (IdentificationComplaintState.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Name],Source.[EnumName]);
GO