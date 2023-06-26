MERGE AZ.LocationType USING (SELECT 1,N'کشور','COUNTRY') AS Source ([VCode],[Name],[EnumName])
ON (LocationType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.LocationType USING (SELECT 2,N'استان','PROVINCE') AS Source ([VCode],[Name],[EnumName])
ON (LocationType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.LocationType USING (SELECT 3,N'شهرستان','COUNTY') AS Source ([VCode],[Name],[EnumName])
ON (LocationType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.LocationType USING (SELECT 4,N'شهر','CITY') AS Source ([VCode],[Name],[EnumName])
ON (LocationType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.LocationType USING (SELECT 5,N'منطقه','DISTRICT') AS Source ([VCode],[Name],[EnumName])
ON (LocationType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.LocationType USING (SELECT 6,N'ناحيه','AREA') AS Source ([VCode],[Name],[EnumName])
ON (LocationType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.LocationType USING (SELECT 7,N'محله','BLOCK') AS Source ([VCode],[Name],[EnumName])
ON (LocationType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Name],Source.[EnumName]);
GO
