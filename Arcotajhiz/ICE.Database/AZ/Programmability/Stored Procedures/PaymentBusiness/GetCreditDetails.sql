CREATE PROCEDURE [AZ].[GetCreditDetails](
	@AccountingVCode BIGINT
)
AS
BEGIN
	IF EXISTS(
		SELECT 1 FROM AZ.Accounting (NOLOCK) WHERE VCode = @AccountingVCode
		AND CS <> BINARY_CHECKSUM(VCode,UserVCode,IdentificationVCode,AccountingTypeVCode,Bed,Bes,OnlinePaymentVCode,[Description],ExpirationDate,EntryDate)
	)
	BEGIN
		SELECT	NULL VCode,
				NULL UserVCode,
				NULL AccountingTypeVCode,
				NULL Bed,
				NULL Bes,
				NULL EntryDate,
				NULL TrackingCode,
				NULL BankPortalVCode,
				NULL Remain,
				NULL IdentificationVCode,
				NULL [Description],
				NULL CreditDescription
	END
	ELSE
	BEGIN
		SELECT	A.VCode,
				A.UserVCode,
				A.AccountingTypeVCode,
				A.Bed,
				A.Bes,
				A.EntryDate,
				OPR.TraceNo TrackingCode,
				OP.BankPortalVCode,
				A.[Description],
				A.IdentificationVCode,
				CASE WHEN A.AccountingTypeVCode = 1 THEN 'افزایش اعتبار از درگاه اینترنتی بانک ' +
						CASE WHEN OP.BankPortalVCode = 1 THEN ' سامان ' WHEN OP.BankPortalVCode = 8 THEN ' ملت ' ELSE ' ' END
					 WHEN A.AccountingTypeVCode = 2 THEN 'تسویه با آیس بابت گزارش با کد رهگیری ' + ISNULL(CAST(CRR.TrackingCode AS NVARCHAR(100)),'[عدم تایید شخص با شماره : '+ I.Cellphone +']')
					 WHEN A.AccountingTypeVCode = 3 THEN 'هزینه ثبت و تشکیل پرونده'
					 WHEN A.AccountingTypeVCode = 4 THEN 'برگشت اعتبار بابت گزارش شخص با شماره: ' + I.Cellphone
					 WHEN A.AccountingTypeVCode = 5 THEN 'هزینه دریافتی بابت از تاریخ  API پنل حقوقی' + 
						SUBSTRING(AZ.ConvertToJalaliDate(A.EntryDate),1,4) + '/' + SUBSTRING(AZ.ConvertToJalaliDate(A.EntryDate),5,2) + '/' + SUBSTRING(AZ.ConvertToJalaliDate(A.EntryDate),7,2) +
						' تا تاریخ ' + 
						SUBSTRING((SELECT [AZ].[JAddMonth](AZ.ConvertToJalaliDate(A.EntryDate),1)),1,4) + '/' + SUBSTRING((SELECT [AZ].[JAddMonth](AZ.ConvertToJalaliDate(A.EntryDate),1)),5,2) + '/' + SUBSTRING((SELECT [AZ].[JAddMonth](AZ.ConvertToJalaliDate(A.EntryDate),1)),7,2)
 				END CreditDescription,
				ROW_NUMBER() OVER(ORDER BY A.VCode ASC) R
		INTO #T FROM AZ.Accounting (NOLOCK) A 
		INNER JOIN AZ.AccountingType (NOLOCK) ATE ON A.AccountingTypeVCode = ATE.VCode
		LEFT JOIN AZ.OnlinePayment (NOLOCK) OP ON A.OnlinePaymentVCode = OP.VCode
		LEFT JOIN AZ.OnlinePaymentReceived (NOLOCK) OPR ON OP.VCode = OPR.OnlinePaymentVCode
		LEFT JOIN AZ.CreditRiskReport (NOLOCK) CRR ON CRR.IdentificationVCode = A.IdentificationVCode
		LEFT JOIN AZ.Identification (NOLOCK) I ON I.VCode = A.IdentificationVCode
		WHERE A.VCode = @AccountingVCode 
			AND A.CS = BINARY_CHECKSUM(A.VCode,A.UserVCode,A.IdentificationVCode,A.AccountingTypeVCode,A.Bed,A.Bes,A.OnlinePaymentVCode,A.[Description],A.ExpirationDate,A.EntryDate)
		ORDER BY A.EntryDate DESC

		SELECT
				T1.VCode,
				T1.UserVCode,
				T1.AccountingTypeVCode,
				T1.Bed,
				T1.Bes,
				T1.EntryDate,
				T1.TrackingCode,
				T1.BankPortalVCode,
				ISNULL(SUM(T2.Bes),0) - ISNULL(SUM(T2.Bed),0) Remain,
				T1.IdentificationVCode,
				T1.CreditDescription,
				T1.[Description]
		FROM #T T1
		INNER JOIN #T T2 ON T1.R >= T2.R
		GROUP BY
				T1.VCode, 
				T1.UserVCode, 
				T1.AccountingTypeVCode,
				T1.Bed,
				T1.Bes,
				T1.EntryDate,
				T1.TrackingCode,
				T1.BankPortalVCode,
				T1.R,
				T1.IdentificationVCode,
				T1.CreditDescription,
				T1.[Description]
		ORDER BY T1.R DESC

		DROP TABLE #T
	END
END
GO

