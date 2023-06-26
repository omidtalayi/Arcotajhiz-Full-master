MERGE AZ.GroupOfPersonsState USING (SELECT 1,1,N'ثبت شده','SUBMITTED') AS Source ([VCode],[Code],[Name],[EnumName])
ON (GroupOfPersonsState.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.GroupOfPersonsState USING (SELECT 2,2,N'ثبت شده','PAID') AS Source ([VCode],[Code],[Name],[EnumName])
ON (GroupOfPersonsState.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.GroupOfPersonsState USING (SELECT 3,3,N'تایید شده','ACCEPTED') AS Source ([VCode],[Code],[Name],[EnumName])
ON (GroupOfPersonsState.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.GroupOfPersonsState USING (SELECT 4,4,N'قابل مشاهده','READY_FOR_SEE') AS Source ([VCode],[Code],[Name],[EnumName])
ON (GroupOfPersonsState.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.GroupOfPersonsState USING (SELECT 5,5,N'مشاهده شده','SEEN') AS Source ([VCode],[Code],[Name],[EnumName])
ON (GroupOfPersonsState.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.GroupOfPersonsState USING (SELECT 6,6,N'رد شده توسط مشتری','REJECTED') AS Source ([VCode],[Code],[Name],[EnumName])
ON (GroupOfPersonsState.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.GroupOfPersonsState USING (SELECT 7,7,N'حذف شده توسط درخواست کننده','DELETED') AS Source ([VCode],[Code],[Name],[EnumName])
ON (GroupOfPersonsState.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO