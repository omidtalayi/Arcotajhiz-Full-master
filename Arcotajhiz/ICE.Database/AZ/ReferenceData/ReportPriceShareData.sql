MERGE AZ.ReportPriceShare USING (SELECT 1,1,1,60,24,16) AS Source ([VCode],[UserVCode],[ReportSourceVCode],[SourceShare],[IceShare],[PartnerShare])
ON (ReportPriceShare.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ReportSourceVCode] = Source.[ReportSourceVCode],[SourceShare] = Source.[SourceShare],[IceShare] = Source.[IceShare],[PartnerShare] = Source.[PartnerShare]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[UserVCode],[ReportSourceVCode],[SourceShare],[IceShare],[PartnerShare]) VALUES (Source.[VCode],Source.[UserVCode],Source.[ReportSourceVCode],Source.[SourceShare],Source.[IceShare],Source.[PartnerShare]);
GO