MERGE AZ.IdentificationComplaintCategory USING (SELECT 1,N'مشخصات فردي','personInformation',NULL,1,NULL) AS Source ([VCode],[Name],[EnumName],[ParentVCode],[LVL],[ControlTypeVCode])
ON (IdentificationComplaintCategory.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name],[EnumName] = Source.[EnumName],[ParentVCode] = Source.[ParentVCode],[LVL] = Source.[LVL],[ControlTypeVCode] = Source.[ControlTypeVCode]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name],[EnumName],[ParentVCode],[LVL],[ControlTypeVCode]) VALUES (Source.[VCode],Source.[Name],Source.[EnumName],Source.[ParentVCode],Source.[LVL],Source.[ControlTypeVCode]);
GO
MERGE AZ.IdentificationComplaintCategory USING (SELECT 2,N'آدرس و تماس','contacts',NULL,1,NULL) AS Source ([VCode],[Name],[EnumName],[ParentVCode],[LVL],[ControlTypeVCode])
ON (IdentificationComplaintCategory.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name],[EnumName] = Source.[EnumName],[ParentVCode] = Source.[ParentVCode],[LVL] = Source.[LVL],[ControlTypeVCode] = Source.[ControlTypeVCode]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name],[EnumName],[ParentVCode],[LVL],[ControlTypeVCode]) VALUES (Source.[VCode],Source.[Name],Source.[EnumName],Source.[ParentVCode],Source.[LVL],Source.[ControlTypeVCode]);
GO
MERGE AZ.IdentificationComplaintCategory USING (SELECT 3,N'چک برگشتي','returnedCheck',NULL,1,NULL) AS Source ([VCode],[Name],[EnumName],[ParentVCode],[LVL],[ControlTypeVCode])
ON (IdentificationComplaintCategory.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name],[EnumName] = Source.[EnumName],[ParentVCode] = Source.[ParentVCode],[LVL] = Source.[LVL],[ControlTypeVCode] = Source.[ControlTypeVCode]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name],[EnumName],[ParentVCode],[LVL],[ControlTypeVCode]) VALUES (Source.[VCode],Source.[Name],Source.[EnumName],Source.[ParentVCode],Source.[LVL],Source.[ControlTypeVCode]);
GO
MERGE AZ.IdentificationComplaintCategory USING (SELECT 4,N'وام در جريان','directContracts',NULL,1,NULL) AS Source ([VCode],[Name],[EnumName],[ParentVCode],[LVL],[ControlTypeVCode])
ON (IdentificationComplaintCategory.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name],[EnumName] = Source.[EnumName],[ParentVCode] = Source.[ParentVCode],[LVL] = Source.[LVL],[ControlTypeVCode] = Source.[ControlTypeVCode]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name],[EnumName],[ParentVCode],[LVL],[ControlTypeVCode]) VALUES (Source.[VCode],Source.[Name],Source.[EnumName],Source.[ParentVCode],Source.[LVL],Source.[ControlTypeVCode]);
GO
MERGE AZ.IdentificationComplaintCategory USING (SELECT 5,N'ضامن','indirectContracts',NULL,1,NULL) AS Source ([VCode],[Name],[EnumName],[ParentVCode],[LVL],[ControlTypeVCode])
ON (IdentificationComplaintCategory.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name],[EnumName] = Source.[EnumName],[ParentVCode] = Source.[ParentVCode],[LVL] = Source.[LVL],[ControlTypeVCode] = Source.[ControlTypeVCode]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name],[EnumName],[ParentVCode],[LVL],[ControlTypeVCode]) VALUES (Source.[VCode],Source.[Name],Source.[EnumName],Source.[ParentVCode],Source.[LVL],Source.[ControlTypeVCode]);
GO
MERGE AZ.IdentificationComplaintCategory USING (SELECT 6,N'اعلام عدم وجود وام','missingContracts',NULL,1,NULL) AS Source ([VCode],[Name],[EnumName],[ParentVCode],[LVL],[ControlTypeVCode])
ON (IdentificationComplaintCategory.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name],[EnumName] = Source.[EnumName],[ParentVCode] = Source.[ParentVCode],[LVL] = Source.[LVL],[ControlTypeVCode] = Source.[ControlTypeVCode]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name],[EnumName],[ParentVCode],[LVL],[ControlTypeVCode]) VALUES (Source.[VCode],Source.[Name],Source.[EnumName],Source.[ParentVCode],Source.[LVL],Source.[ControlTypeVCode]);
GO
MERGE AZ.IdentificationComplaintCategory USING (SELECT 7,N'نام','FirstName',1,2,4) AS Source ([VCode],[Name],[EnumName],[ParentVCode],[LVL],[ControlTypeVCode])
ON (IdentificationComplaintCategory.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name],[EnumName] = Source.[EnumName],[ParentVCode] = Source.[ParentVCode],[LVL] = Source.[LVL],[ControlTypeVCode] = Source.[ControlTypeVCode]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name],[EnumName],[ParentVCode],[LVL],[ControlTypeVCode]) VALUES (Source.[VCode],Source.[Name],Source.[EnumName],Source.[ParentVCode],Source.[LVL],Source.[ControlTypeVCode]);
GO
MERGE AZ.IdentificationComplaintCategory USING (SELECT 8,N'نام خانوادگي','LastName',1,2,4) AS Source ([VCode],[Name],[EnumName],[ParentVCode],[LVL],[ControlTypeVCode])
ON (IdentificationComplaintCategory.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name],[EnumName] = Source.[EnumName],[ParentVCode] = Source.[ParentVCode],[LVL] = Source.[LVL],[ControlTypeVCode] = Source.[ControlTypeVCode]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name],[EnumName],[ParentVCode],[LVL],[ControlTypeVCode]) VALUES (Source.[VCode],Source.[Name],Source.[EnumName],Source.[ParentVCode],Source.[LVL],Source.[ControlTypeVCode]);
GO
MERGE AZ.IdentificationComplaintCategory USING (SELECT 9,N'نام پدر','FatherName',1,2,4) AS Source ([VCode],[Name],[EnumName],[ParentVCode],[LVL],[ControlTypeVCode])
ON (IdentificationComplaintCategory.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name],[EnumName] = Source.[EnumName],[ParentVCode] = Source.[ParentVCode],[LVL] = Source.[LVL],[ControlTypeVCode] = Source.[ControlTypeVCode]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name],[EnumName],[ParentVCode],[LVL],[ControlTypeVCode]) VALUES (Source.[VCode],Source.[Name],Source.[EnumName],Source.[ParentVCode],Source.[LVL],Source.[ControlTypeVCode]);
GO
MERGE AZ.IdentificationComplaintCategory USING (SELECT 10,N'جنسيت','Gender',1,2,4) AS Source ([VCode],[Name],[EnumName],[ParentVCode],[LVL],[ControlTypeVCode])
ON (IdentificationComplaintCategory.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name],[EnumName] = Source.[EnumName],[ParentVCode] = Source.[ParentVCode],[LVL] = Source.[LVL],[ControlTypeVCode] = Source.[ControlTypeVCode]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name],[EnumName],[ParentVCode],[LVL],[ControlTypeVCode]) VALUES (Source.[VCode],Source.[Name],Source.[EnumName],Source.[ParentVCode],Source.[LVL],Source.[ControlTypeVCode]);
GO
MERGE AZ.IdentificationComplaintCategory USING (SELECT 11,N'تاريخ تولد','BirthDate',1,2,2) AS Source ([VCode],[Name],[EnumName],[ParentVCode],[LVL],[ControlTypeVCode])
ON (IdentificationComplaintCategory.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name],[EnumName] = Source.[EnumName],[ParentVCode] = Source.[ParentVCode],[LVL] = Source.[LVL],[ControlTypeVCode] = Source.[ControlTypeVCode]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name],[EnumName],[ParentVCode],[LVL],[ControlTypeVCode]) VALUES (Source.[VCode],Source.[Name],Source.[EnumName],Source.[ParentVCode],Source.[LVL],Source.[ControlTypeVCode]);
GO
MERGE AZ.IdentificationComplaintCategory USING (SELECT 12,N'محل تولد','BirthPlace',1,2,2) AS Source ([VCode],[Name],[EnumName],[ParentVCode],[LVL],[ControlTypeVCode])
ON (IdentificationComplaintCategory.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name],[EnumName] = Source.[EnumName],[ParentVCode] = Source.[ParentVCode],[LVL] = Source.[LVL],[ControlTypeVCode] = Source.[ControlTypeVCode]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name],[EnumName],[ParentVCode],[LVL],[ControlTypeVCode]) VALUES (Source.[VCode],Source.[Name],Source.[EnumName],Source.[ParentVCode],Source.[LVL],Source.[ControlTypeVCode]);
GO
MERGE AZ.IdentificationComplaintCategory USING (SELECT 13,N'وضعيت تاهل','MaritalStatus',1,2,1) AS Source ([VCode],[Name],[EnumName],[ParentVCode],[LVL],[ControlTypeVCode])
ON (IdentificationComplaintCategory.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name],[EnumName] = Source.[EnumName],[ParentVCode] = Source.[ParentVCode],[LVL] = Source.[LVL],[ControlTypeVCode] = Source.[ControlTypeVCode]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name],[EnumName],[ParentVCode],[LVL],[ControlTypeVCode]) VALUES (Source.[VCode],Source.[Name],Source.[EnumName],Source.[ParentVCode],Source.[LVL],Source.[ControlTypeVCode]);
GO
MERGE AZ.IdentificationComplaintCategory USING (SELECT 14,N'تلفن','personageTelephones',2,2,5) AS Source ([VCode],[Name],[EnumName],[ParentVCode],[LVL],[ControlTypeVCode])
ON (IdentificationComplaintCategory.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name],[EnumName] = Source.[EnumName],[ParentVCode] = Source.[ParentVCode],[LVL] = Source.[LVL],[ControlTypeVCode] = Source.[ControlTypeVCode]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name],[EnumName],[ParentVCode],[LVL],[ControlTypeVCode]) VALUES (Source.[VCode],Source.[Name],Source.[EnumName],Source.[ParentVCode],Source.[LVL],Source.[ControlTypeVCode]);
GO
MERGE AZ.IdentificationComplaintCategory USING (SELECT 15,N'تلفن همراه','personageCellphones',2,2,5) AS Source ([VCode],[Name],[EnumName],[ParentVCode],[LVL],[ControlTypeVCode])
ON (IdentificationComplaintCategory.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name],[EnumName] = Source.[EnumName],[ParentVCode] = Source.[ParentVCode],[LVL] = Source.[LVL],[ControlTypeVCode] = Source.[ControlTypeVCode]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name],[EnumName],[ParentVCode],[LVL],[ControlTypeVCode]) VALUES (Source.[VCode],Source.[Name],Source.[EnumName],Source.[ParentVCode],Source.[LVL],Source.[ControlTypeVCode]);
GO
MERGE AZ.IdentificationComplaintCategory USING (SELECT 16,N'آدرس دائمي','addresses',2,2,4) AS Source ([VCode],[Name],[EnumName],[ParentVCode],[LVL],[ControlTypeVCode])
ON (IdentificationComplaintCategory.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name],[EnumName] = Source.[EnumName],[ParentVCode] = Source.[ParentVCode],[LVL] = Source.[LVL],[ControlTypeVCode] = Source.[ControlTypeVCode]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name],[EnumName],[ParentVCode],[LVL],[ControlTypeVCode]) VALUES (Source.[VCode],Source.[Name],Source.[EnumName],Source.[ParentVCode],Source.[LVL],Source.[ControlTypeVCode]);
GO
MERGE AZ.IdentificationComplaintCategory USING (SELECT 17,N'وضعيت منفي قرارداد','negativeContractStatus',4,2,4) AS Source ([VCode],[Name],[EnumName],[ParentVCode],[LVL],[ControlTypeVCode])
ON (IdentificationComplaintCategory.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name],[EnumName] = Source.[EnumName],[ParentVCode] = Source.[ParentVCode],[LVL] = Source.[LVL],[ControlTypeVCode] = Source.[ControlTypeVCode]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name],[EnumName],[ParentVCode],[LVL],[ControlTypeVCode]) VALUES (Source.[VCode],Source.[Name],Source.[EnumName],Source.[ParentVCode],Source.[LVL],Source.[ControlTypeVCode]);
GO
MERGE AZ.IdentificationComplaintCategory USING (SELECT 18,N'مبلغ بدهي سررسيد شده پرداخت نشده','overdueAmount',4,2,4) AS Source ([VCode],[Name],[EnumName],[ParentVCode],[LVL],[ControlTypeVCode])
ON (IdentificationComplaintCategory.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name],[EnumName] = Source.[EnumName],[ParentVCode] = Source.[ParentVCode],[LVL] = Source.[LVL],[ControlTypeVCode] = Source.[ControlTypeVCode]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name],[EnumName],[ParentVCode],[LVL],[ControlTypeVCode]) VALUES (Source.[VCode],Source.[Name],Source.[EnumName],Source.[ParentVCode],Source.[LVL],Source.[ControlTypeVCode]);
GO
MERGE AZ.IdentificationComplaintCategory USING (SELECT 19,N'چک برگشتی دارم','haveReturnedCheck',3,2,1) AS Source ([VCode],[Name],[EnumName],[ParentVCode],[LVL],[ControlTypeVCode])
ON (IdentificationComplaintCategory.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name],[EnumName] = Source.[EnumName],[ParentVCode] = Source.[ParentVCode],[LVL] = Source.[LVL],[ControlTypeVCode] = Source.[ControlTypeVCode]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name],[EnumName],[ParentVCode],[LVL],[ControlTypeVCode]) VALUES (Source.[VCode],Source.[Name],Source.[EnumName],Source.[ParentVCode],Source.[LVL],Source.[ControlTypeVCode]);
GO
MERGE AZ.IdentificationComplaintCategory USING (SELECT 20,N'ضامن','notIndirectContracts',5,2,1) AS Source ([VCode],[Name],[EnumName],[ParentVCode],[LVL],[ControlTypeVCode])
ON (IdentificationComplaintCategory.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name],[EnumName] = Source.[EnumName],[ParentVCode] = Source.[ParentVCode],[LVL] = Source.[LVL],[ControlTypeVCode] = Source.[ControlTypeVCode]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name],[EnumName],[ParentVCode],[LVL],[ControlTypeVCode]) VALUES (Source.[VCode],Source.[Name],Source.[EnumName],Source.[ParentVCode],Source.[LVL],Source.[ControlTypeVCode]);
GO
MERGE AZ.IdentificationComplaintCategory USING (SELECT 21,N'اعلام عدم وجود وام','haveMissingContracts',6,2,4) AS Source ([VCode],[Name],[EnumName],[ParentVCode],[LVL],[ControlTypeVCode])
ON (IdentificationComplaintCategory.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name],[EnumName] = Source.[EnumName],[ParentVCode] = Source.[ParentVCode],[LVL] = Source.[LVL],[ControlTypeVCode] = Source.[ControlTypeVCode]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name],[EnumName],[ParentVCode],[LVL],[ControlTypeVCode]) VALUES (Source.[VCode],Source.[Name],Source.[EnumName],Source.[ParentVCode],Source.[LVL],Source.[ControlTypeVCode]);
GO