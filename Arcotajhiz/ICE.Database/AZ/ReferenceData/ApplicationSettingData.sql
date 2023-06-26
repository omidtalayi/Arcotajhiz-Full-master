MERGE AZ.ApplicationSetting USING (SELECT 1,'LinkExpirationHour','24',N'مدت زمان انقضا لینک به ساعت') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 2,'UserCreditRequestPerHour','5',N'تعداد درخواست مشاهده رتبه اعتباری در ساعت') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 3,'SMSUserName','h.g.m',N'نام کاربري SMS') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 4,'SMSPassword','day3199166',N'کلمه عبور SMS') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 5,'SMSCenterNumber','100007300',N'شماره مرکز ارسال SMS') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 6,'SMSAPIKey','OKV1R+pRt6Q+50+S1ORvj5vA1Fd2iqPULXP3+kM+gVU',N'توکن SMS') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 7,'IceShare','20',N'سهم آیس') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 8,'PartnerShare','20',N'سهم شریک') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 9,'ApiToken','',N'توکن فراخوانی API') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 10,'SandboxToken','',N'توکن Sandbox') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 11,'SourceShare','60',N'سهم  منبع') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 12,'TestEnabled','0',N'فعال سازی محیط تست') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 13,'ApiUsername','Ice',N'نام کاربری API') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 14,'ApiPassword','3050858',N'رمز عبور API') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 15,'SamanIsActive','1',N'فعالسازی درگاه مشتری سامان') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 16,'ParsaSmsUrl','https://api.smsapp.ir/v2/sms/send/simple',N'یوارال اس ام اس') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 17,'ChargingCreditEnabled','1',N'فعال سازی شارژ اعتبار') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 18,'MinimumCreditAmount','1000',N'کف شارژ اعتبار') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 19,'MaximumCreditAmount','1000000000',N'سقف شارژ اعتبار') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 20,'EmailAddress','mail.icescoring.com',N'آدرس سرور ایمیل') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 21,'EmailPassword','H&D@Ice1579',N'رمز عبور ایمیل') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 22,'EmailUserName','noreply@icescoring.com',N'نام کاربری ایمیل') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 23,'EmailSenderAddress','noreply@icescoring.com',N'از طرف ایمیل') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 24,'EmailPort','25',N'پورت ایمیل') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 25,'PresenterPaymentEnabled','1',N'فعال سازی دریافت هزینه بازاریابی') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 26,'PresenterPaymentAmount','300000',N'مبلغ هزینه بازاریابی') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 27,'ShebaNumberIce','IR740190000000100663926004',N'شماره شبا آیس') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 28,'CALLAPIKey','eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMDExMCIsImV4cCI6MTg3NDc0ODMyNSwiaXNzIjoiQml0ZWwiLCJhdWQiOiJCaXRlbCJ9.Q0ZscbfZCbZ1y6Gq2fsbIyBZAC6ZDGTmqzDe-KYwTns',N'کد api bitel') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 29,'CALLVoiceID','ebc36e4769db4792ad972f43d1a8d0fd',N'کد صدا bitel') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 30,'CALLAPIUrl','https://api.bitel.rest/api/v1',N'آدرس Api bitel') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 31,'DisableReceiverCellphone',0,N'غیرفعال کردن دریافت گزارش دیگران') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 32,'OnlyCallActive',0,N'بجای sms کد را با تماس بگویید') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 33,'DisableReport',0,N'غيرفعال کردن سيستم گزارش گیری') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 34,'FirmPassword','H&D@Firmpanel159753',N'رمز عبور پنل های آیس') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 35,'ApiIsDisabled','false',N'غیرفعال سازی API') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 36,'ICSNewApi','0',N'استفاده از Api جدید ICS') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 37,'DisableFirmSubmit',0,N'غیرفعال کردن دریافت گزارش حقوقی') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 38,'ICS24_Server_Not_Respond',0,N'سرور ICS24 مشکل دارد') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 39,'NotMultiSettle',0,N'درگاه بانک تسهیم نباشد') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 40,'NewCBSIcs24Api_Tokens','',N'توکن های API جدید CBS ICS24') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 41,'NewCBSIcs24Api_UserName','iceusr',N'نام کاربری  API جدید CBS ICS24') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 42,'NewCBSIcs24Api_Password','vZLFoW0r',N'نام کاربری  API جدید CBS ICS24') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 43,'MellatIsActive','1',N'فعالسازی درگاه مشتری بانک ملت') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 44,'FinnotechApi_Tokens','',N'توکن  API Finnotech') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 45,'NewFinnotech_UserName','HooshoDanesh',N'نام کاربری  API Finnotech') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 46,'NewFinnotech_Password','89349d3001ec2cb2e73a',N'پسورد  API Finnotech') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 47,'FinnotechChequeIsActive','1',N'فعالسازی سرویس چک برگشتی فینوتک') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 48,'MyIrancell_UserName','***',N'نام کاربری  API MyIrancell') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 49,'MyIrancell_Password','***',N'پسورد  API MyIrancell') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 50,'FirmApiPaymentAmount','4000000',N'FirmApiPaymentAmount') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 51,'InviteSMSText',N'دوستانت رو معرفی کن ! بعد از اینکه 5تاشون رتبه اعتباری گرفتن شما میتونی دفعه بعدی رتبه اعتباریت رو رایگان دریافت کنی.',N'متن اس ام اس دعوت از دوستان') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 52,'Jam_UserName','10103772598',N'نام کاربری سامانه جام') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 53,'Jam_Password','NHYnhy-123',N'رمز عبور سامانه جام') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 54,'Jam_AuthCode','',N'توکن دسترسی جام') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 55,'ShowSystemIsDisabled','1',N'دسترسی به سایت محدود است') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 56,'DisableEmtaSubmit',0,N'غیرفعال کردن اطلاعات هویتی امتا') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 57,'DisableJaamSubmit',1,N'غیرفعال کردن اطلاعات صورت های مالی') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 58,'EmtaClientId','a9193cd0-962b-415d-bd5a-67ce94029dba',N'کلایدت ایدی امتا') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 59,'EmtaSecretId','LbilAqsBkjFAAajzZsjhjiQNjuaQefcP',N'سیکرت کد امتا') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 60,'DisableEmtaTestSubmit','0',N'غیر فعال کردن تستی امتا') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 61,'DisableEmtaGraphSubmit','0',N'غیر فعال کردن گراف امتا') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 62,'SepMultiSettleShebas','[{"number" : 0,"name" : "ICE","shebaNo" : "IR250560083281001967384001"},{"number" : 1,"name" : "ICS","shebaNo" : "IR210560082681002753291001"},{"number" : 2,"name" : "JAAM","shebaNo" : "IR020550011485005483633001"}]',N'حسابهای آیس و شرکا برای درگاه بانک سامان') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 63,'mtn_Irancell_Gift_Username','Icescoring_78986545',N'نام کاربری سرویس هدیه ایرانسل') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO
MERGE AZ.ApplicationSetting USING (SELECT 64,'mtn_Irancell_Gift_Password','qE9<p).Ur$HY8Y7N',N'رمز عبور سرویس هدیه ایرانسل') AS Source ([VCode],[Key],[Value],[Description])
ON (ApplicationSetting.VCode = Source.VCode)
WHEN MATCHED THEN
	UPDATE SET [Key] = Source.[Key],[Value] = Source.[Value],[Description] = Source.[Description]
WHEN NOT MATCHED THEN
	INSERT ([VCode],[Key],[Value],[Description]) VALUES (Source.[VCode],Source.[Key],Source.[Value],Source.[Description]);
GO