MERGE AZ.ReportSource USING (SELECT 1,1,N'مشاوره رتبه بندی ایران','ICS') AS Source ([VCode],[Code],[Name],[EnumName])
ON (ReportSource.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Code] = Source.[Code],[Name] = Source.[Name],[EnumName] = Source.[EnumName]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Code],[Name],[EnumName]) VALUES (Source.[VCode],Source.[Code],Source.[Name],Source.[EnumName]);
GO