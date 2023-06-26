MERGE AZ.ApiErrorCode USING (SELECT 1,1001,N'شناسه کاربری یا رمز عبور اشتباه می باشد.','USERNAME_OR_PASSWORD_IS_INVALID') AS Source ([VCode],[ErrorCode],[ErrorMessage],[ErrorEnglishMessage])
ON (ApiErrorCode.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ErrorCode] = Source.[ErrorCode],[ErrorMessage] = Source.[ErrorMessage],[ErrorEnglishMessage] = Source.[ErrorEnglishMessage]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ErrorCode],[ErrorMessage],[ErrorEnglishMessage]) VALUES (Source.[VCode],Source.[ErrorCode],Source.[ErrorMessage],Source.[ErrorEnglishMessage]);
GO
MERGE AZ.ApiErrorCode USING (SELECT 2,1002,N'شناسه کاربری قفل شده است.','USER_IS_LOCKED') AS Source ([VCode],[ErrorCode],[ErrorMessage],[ErrorEnglishMessage])
ON (ApiErrorCode.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ErrorCode] = Source.[ErrorCode],[ErrorMessage] = Source.[ErrorMessage],[ErrorEnglishMessage] = Source.[ErrorEnglishMessage]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ErrorCode],[ErrorMessage],[ErrorEnglishMessage]) VALUES (Source.[VCode],Source.[ErrorCode],Source.[ErrorMessage],Source.[ErrorEnglishMessage]);
GO
MERGE AZ.ApiErrorCode USING (SELECT 3,1003,N'شناسه کاربری تایید نمی باشد.','USER_IS_NOT_APPROVED') AS Source ([VCode],[ErrorCode],[ErrorMessage],[ErrorEnglishMessage])
ON (ApiErrorCode.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ErrorCode] = Source.[ErrorCode],[ErrorMessage] = Source.[ErrorMessage],[ErrorEnglishMessage] = Source.[ErrorEnglishMessage]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ErrorCode],[ErrorMessage],[ErrorEnglishMessage]) VALUES (Source.[VCode],Source.[ErrorCode],Source.[ErrorMessage],Source.[ErrorEnglishMessage]);
GO
MERGE AZ.ApiErrorCode USING (SELECT 4,1004,N'کد ملی نامعتبر است.','NATIONAL_CODE_IS_INVALID') AS Source ([VCode],[ErrorCode],[ErrorMessage],[ErrorEnglishMessage])
ON (ApiErrorCode.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ErrorCode] = Source.[ErrorCode],[ErrorMessage] = Source.[ErrorMessage],[ErrorEnglishMessage] = Source.[ErrorEnglishMessage]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ErrorCode],[ErrorMessage],[ErrorEnglishMessage]) VALUES (Source.[VCode],Source.[ErrorCode],Source.[ErrorMessage],Source.[ErrorEnglishMessage]);
GO
MERGE AZ.ApiErrorCode USING (SELECT 5,1005,N'شماره موبایل نامعتبر است.','CELLPHONE_IS_INVALID') AS Source ([VCode],[ErrorCode],[ErrorMessage],[ErrorEnglishMessage])
ON (ApiErrorCode.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ErrorCode] = Source.[ErrorCode],[ErrorMessage] = Source.[ErrorMessage],[ErrorEnglishMessage] = Source.[ErrorEnglishMessage]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ErrorCode],[ErrorMessage],[ErrorEnglishMessage]) VALUES (Source.[VCode],Source.[ErrorCode],Source.[ErrorMessage],Source.[ErrorEnglishMessage]);
GO
MERGE AZ.ApiErrorCode USING (SELECT 6,1006,N'منبع اطلاعاتی درخواستی نامعتبر است.','REPORT_SOURCE_IS_INVALID') AS Source ([VCode],[ErrorCode],[ErrorMessage],[ErrorEnglishMessage])
ON (ApiErrorCode.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ErrorCode] = Source.[ErrorCode],[ErrorMessage] = Source.[ErrorMessage],[ErrorEnglishMessage] = Source.[ErrorEnglishMessage]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ErrorCode],[ErrorMessage],[ErrorEnglishMessage]) VALUES (Source.[VCode],Source.[ErrorCode],Source.[ErrorMessage],Source.[ErrorEnglishMessage]);
GO
MERGE AZ.ApiErrorCode USING (SELECT 7,1007,N'ارسال اس ام اس با خطا مواجه شد.','SENDING_SMS_HAS_FAILED') AS Source ([VCode],[ErrorCode],[ErrorMessage],[ErrorEnglishMessage])
ON (ApiErrorCode.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ErrorCode] = Source.[ErrorCode],[ErrorMessage] = Source.[ErrorMessage],[ErrorEnglishMessage] = Source.[ErrorEnglishMessage]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ErrorCode],[ErrorMessage],[ErrorEnglishMessage]) VALUES (Source.[VCode],Source.[ErrorCode],Source.[ErrorMessage],Source.[ErrorEnglishMessage]);
GO
MERGE AZ.ApiErrorCode USING (SELECT 8,1008,N'اعتبار کافی نیست.','CREDIT_IS_NOT_ENOUGH') AS Source ([VCode],[ErrorCode],[ErrorMessage],[ErrorEnglishMessage])
ON (ApiErrorCode.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ErrorCode] = Source.[ErrorCode],[ErrorMessage] = Source.[ErrorMessage],[ErrorEnglishMessage] = Source.[ErrorEnglishMessage]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ErrorCode],[ErrorMessage],[ErrorEnglishMessage]) VALUES (Source.[VCode],Source.[ErrorCode],Source.[ErrorMessage],Source.[ErrorEnglishMessage]);
GO
MERGE AZ.ApiErrorCode USING (SELECT 9,1009,N'گزارش در 24 ساعت گذشته گرفته شده است.','REPORT_ALREADY_EXIST_IN_LAST_24HOURS') AS Source ([VCode],[ErrorCode],[ErrorMessage],[ErrorEnglishMessage])
ON (ApiErrorCode.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ErrorCode] = Source.[ErrorCode],[ErrorMessage] = Source.[ErrorMessage],[ErrorEnglishMessage] = Source.[ErrorEnglishMessage]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ErrorCode],[ErrorMessage],[ErrorEnglishMessage]) VALUES (Source.[VCode],Source.[ErrorCode],Source.[ErrorMessage],Source.[ErrorEnglishMessage]);
GO
MERGE AZ.ApiErrorCode USING (SELECT 10,1010,N'درخواست گزارش قبلا ثبت شده است.','REPORT_REQUEST_ALREADY_SUBMITTED') AS Source ([VCode],[ErrorCode],[ErrorMessage],[ErrorEnglishMessage])
ON (ApiErrorCode.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ErrorCode] = Source.[ErrorCode],[ErrorMessage] = Source.[ErrorMessage],[ErrorEnglishMessage] = Source.[ErrorEnglishMessage]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ErrorCode],[ErrorMessage],[ErrorEnglishMessage]) VALUES (Source.[VCode],Source.[ErrorCode],Source.[ErrorMessage],Source.[ErrorEnglishMessage]);
GO
MERGE AZ.ApiErrorCode USING (SELECT 11,1011,N'API ها غیر فعال میباشد','API_IS_DISABLED') AS Source ([VCode],[ErrorCode],[ErrorMessage],[ErrorEnglishMessage])
ON (ApiErrorCode.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ErrorCode] = Source.[ErrorCode],[ErrorMessage] = Source.[ErrorMessage],[ErrorEnglishMessage] = Source.[ErrorEnglishMessage]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ErrorCode],[ErrorMessage],[ErrorEnglishMessage]) VALUES (Source.[VCode],Source.[ErrorCode],Source.[ErrorMessage],Source.[ErrorEnglishMessage]);
GO
MERGE AZ.ApiErrorCode USING (SELECT 12,1012,N'شناسه ملی شرکت معتبر نمی باشد.','COMPANY_NATIONAL_ID_IS_INVALID') AS Source ([VCode],[ErrorCode],[ErrorMessage],[ErrorEnglishMessage])
ON (ApiErrorCode.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ErrorCode] = Source.[ErrorCode],[ErrorMessage] = Source.[ErrorMessage],[ErrorEnglishMessage] = Source.[ErrorEnglishMessage]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ErrorCode],[ErrorMessage],[ErrorEnglishMessage]) VALUES (Source.[VCode],Source.[ErrorCode],Source.[ErrorMessage],Source.[ErrorEnglishMessage]);
GO
MERGE AZ.ApiErrorCode USING (SELECT 13,1013,N'شناسه گزارش معتبر نمیباشد.','REPORT_ID_IS_INVALID') AS Source ([VCode],[ErrorCode],[ErrorMessage],[ErrorEnglishMessage])
ON (ApiErrorCode.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ErrorCode] = Source.[ErrorCode],[ErrorMessage] = Source.[ErrorMessage],[ErrorEnglishMessage] = Source.[ErrorEnglishMessage]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ErrorCode],[ErrorMessage],[ErrorEnglishMessage]) VALUES (Source.[VCode],Source.[ErrorCode],Source.[ErrorMessage],Source.[ErrorEnglishMessage]);
GO
MERGE AZ.ApiErrorCode USING (SELECT 14,1014,N'شناسه پرداخت خالی است.','SALES_REFID_IS_INVALID') AS Source ([VCode],[ErrorCode],[ErrorMessage],[ErrorEnglishMessage])
ON (ApiErrorCode.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ErrorCode] = Source.[ErrorCode],[ErrorMessage] = Source.[ErrorMessage],[ErrorEnglishMessage] = Source.[ErrorEnglishMessage]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ErrorCode],[ErrorMessage],[ErrorEnglishMessage]) VALUES (Source.[VCode],Source.[ErrorCode],Source.[ErrorMessage],Source.[ErrorEnglishMessage]);
GO
MERGE AZ.ApiErrorCode USING (SELECT 15,1015,N'مبلغ پرداختی صحیح نمیباشد.','SALES_AMOUNT_IS_INVALID') AS Source ([VCode],[ErrorCode],[ErrorMessage],[ErrorEnglishMessage])
ON (ApiErrorCode.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [ErrorCode] = Source.[ErrorCode],[ErrorMessage] = Source.[ErrorMessage],[ErrorEnglishMessage] = Source.[ErrorEnglishMessage]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[ErrorCode],[ErrorMessage],[ErrorEnglishMessage]) VALUES (Source.[VCode],Source.[ErrorCode],Source.[ErrorMessage],Source.[ErrorEnglishMessage]);
GO