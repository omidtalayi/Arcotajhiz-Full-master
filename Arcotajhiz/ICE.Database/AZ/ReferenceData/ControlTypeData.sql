MERGE AZ.ControlType USING (SELECT 1,N'چک باکس','checkbox') AS Source ([VCode],[Name],[EnumName])
ON (ControlType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ControlType USING (SELECT 2,N'دیت تایم','datetime') AS Source ([VCode],[Name],[EnumName])
ON (ControlType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ControlType USING (SELECT 3,N'ایمیل','checkbox') AS Source ([VCode],[Name],[EnumName])
ON (ControlType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ControlType USING (SELECT 4,N'تکست','checkbox') AS Source ([VCode],[Name],[EnumName])
ON (ControlType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Name],Source.[EnumName]);
GO
MERGE AZ.ControlType USING (SELECT 5,N'تلفن','checkbox') AS Source ([VCode],[Name],[EnumName])
ON (ControlType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Name],Source.[EnumName]);
GO