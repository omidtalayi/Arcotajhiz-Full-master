MERGE AZ.UserHistoryType USING (SELECT 1,'LoginSuccessful') AS Source ([VCode],[Name])
ON (UserHistoryType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name]) VALUES (Source.[VCode],Source.[Name]);
GO
MERGE AZ.UserHistoryType USING (SELECT 2,'LoginFailed') AS Source ([VCode],[Name])
ON (UserHistoryType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name]) VALUES (Source.[VCode],Source.[Name]);
GO
MERGE AZ.UserHistoryType USING (SELECT 3,'UserCreated') AS Source ([VCode],[Name])
ON (UserHistoryType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name]) VALUES (Source.[VCode],Source.[Name]);
GO
MERGE AZ.UserHistoryType USING (SELECT 4,'SetUsername') AS Source ([VCode],[Name])
ON (UserHistoryType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name]) VALUES (Source.[VCode],Source.[Name]);
GO
MERGE AZ.UserHistoryType USING (SELECT 5,'SetPassword') AS Source ([VCode],[Name])
ON (UserHistoryType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name]) VALUES (Source.[VCode],Source.[Name]);
GO
MERGE AZ.UserHistoryType USING (SELECT 6,'CellPhoneSubmitted') AS Source ([VCode],[Name])
ON (UserHistoryType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name]) VALUES (Source.[VCode],Source.[Name]);
GO
MERGE AZ.UserHistoryType USING (SELECT 7,'CellPhoneVerificationCodeGenerated') AS Source ([VCode],[Name])
ON (UserHistoryType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name]) VALUES (Source.[VCode],Source.[Name]);
GO
MERGE AZ.UserHistoryType USING (SELECT 8,'CellPhoneVerificationSent') AS Source ([VCode],[Name])
ON (UserHistoryType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name]) VALUES (Source.[VCode],Source.[Name]);
GO
MERGE AZ.UserHistoryType USING (SELECT 9,'CellPhoneVerified') AS Source ([VCode],[Name])
ON (UserHistoryType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name]) VALUES (Source.[VCode],Source.[Name]);
GO
MERGE AZ.UserHistoryType USING (SELECT 10,'EmailSubmitted') AS Source ([VCode],[Name])
ON (UserHistoryType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name]) VALUES (Source.[VCode],Source.[Name]);
GO
MERGE AZ.UserHistoryType USING (SELECT 11,'EmailVerificationCodeGenerated') AS Source ([VCode],[Name])
ON (UserHistoryType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name]) VALUES (Source.[VCode],Source.[Name]);
GO
MERGE AZ.UserHistoryType USING (SELECT 12,'EmailVerificationCodeSent') AS Source ([VCode],[Name])
ON (UserHistoryType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name]) VALUES (Source.[VCode],Source.[Name]);
GO
MERGE AZ.UserHistoryType USING (SELECT 13,'EmailVerified') AS Source ([VCode],[Name])
ON (UserHistoryType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name]) VALUES (Source.[VCode],Source.[Name]);
GO
MERGE AZ.UserHistoryType USING (SELECT 14,'UserLocked') AS Source ([VCode],[Name])
ON (UserHistoryType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name]) VALUES (Source.[VCode],Source.[Name]);
GO
MERGE AZ.UserHistoryType USING (SELECT 15,'UserUnlocked') AS Source ([VCode],[Name])
ON (UserHistoryType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name]) VALUES (Source.[VCode],Source.[Name]);
GO
MERGE AZ.UserHistoryType USING (SELECT 16,'UserAproved') AS Source ([VCode],[Name])
ON (UserHistoryType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name]) VALUES (Source.[VCode],Source.[Name]);
GO
MERGE AZ.UserHistoryType USING (SELECT 17,'ResetPasswordCellPhoneSubmitted') AS Source ([VCode],[Name])
ON (UserHistoryType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name]) VALUES (Source.[VCode],Source.[Name]);
GO
MERGE AZ.UserHistoryType USING (SELECT 18,'ResetPasswordCellPhoneConfirmationCodeGenerated') AS Source ([VCode],[Name])
ON (UserHistoryType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name]) VALUES (Source.[VCode],Source.[Name]);
GO
MERGE AZ.UserHistoryType USING (SELECT 19,'ResetPasswordCellPhoneConfirmationCodeSent') AS Source ([VCode],[Name])
ON (UserHistoryType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name]) VALUES (Source.[VCode],Source.[Name]);
GO
MERGE AZ.UserHistoryType USING (SELECT 20,'ResetPasswordEmailSubmitted') AS Source ([VCode],[Name])
ON (UserHistoryType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name]) VALUES (Source.[VCode],Source.[Name]);
GO
MERGE AZ.UserHistoryType USING (SELECT 21,'ResetPasswordEmailConfirmationCodeGenerated') AS Source ([VCode],[Name])
ON (UserHistoryType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name]) VALUES (Source.[VCode],Source.[Name]);
GO
MERGE AZ.UserHistoryType USING (SELECT 22,'ResetPasswordEmailConfirmationCodeSent') AS Source ([VCode],[Name])
ON (UserHistoryType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name]) VALUES (Source.[VCode],Source.[Name]);
GO
MERGE AZ.UserHistoryType USING (SELECT 23,'ResetPasswordApproved') AS Source ([VCode],[Name])
ON (UserHistoryType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name]) VALUES (Source.[VCode],Source.[Name]);
GO
MERGE AZ.UserHistoryType USING (SELECT 24,'EmailDeleted') AS Source ([VCode],[Name])
ON (UserHistoryType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name]) VALUES (Source.[VCode],Source.[Name]);
GO
MERGE AZ.UserHistoryType USING (SELECT 25,'UnverifiedCellPhoneLost') AS Source ([VCode],[Name])
ON (UserHistoryType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name]) VALUES (Source.[VCode],Source.[Name]);
GO
MERGE AZ.UserHistoryType USING (SELECT 26,'UnverifiedEmailLost') AS Source ([VCode],[Name])
ON (UserHistoryType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name]) VALUES (Source.[VCode],Source.[Name]);
GO
MERGE AZ.UserHistoryType USING (SELECT 27,'RetrieveUsernameByCellphone') AS Source ([VCode],[Name])
ON (UserHistoryType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name]) VALUES (Source.[VCode],Source.[Name]);
GO
MERGE AZ.UserHistoryType USING (SELECT 28,'RetrieveUsernameByEmail') AS Source ([VCode],[Name])
ON (UserHistoryType.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Name] = Source.[Name]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Name]) VALUES (Source.[VCode],Source.[Name]);