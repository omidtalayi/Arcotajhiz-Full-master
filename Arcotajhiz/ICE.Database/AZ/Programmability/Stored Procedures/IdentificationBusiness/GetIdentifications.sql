CREATE PROCEDURE [AZ].[GetIdentifications](
	@Cellphone VARCHAR(20) = NULL,
	@NationalCode VARCHAR(20) = NULL,
	@UserVCode INT = NULL,
	@IdentificationStates XML = NULL,
	@PageSize INT = NULL,
	@PageNo INT = NULL,
	@RowCount INT = NULL,
	@FromJDate VARCHAR(8) = NULL,
	@ToJDate VARCHAR(8) = NULL,
	@WithoutPartners BIT = 0,
	@WithouExpired BIT = 0,
	@HasComplaint BIT = 0,
	@IdentificationTypeVCode INT = NULL,
	@IdentificationComplaintStateVCodes XML = NULL,
	@CompanyNationalId VARCHAR(20) = NULL,
	@TrackingCode BIGINT = NULL
)
AS
BEGIN
	DECLARE @RowOffsetStatement NVARCHAR(MAX) = CONVERT(NVARCHAR(MAX),ISNULL((@PageNo - 1) * @PageSize,0))
	DECLARE @RowPageStatement NVARCHAR(MAX) = CASE WHEN @PageNo IS NOT NULL AND @PageSize IS NOT NULL THEN 'FETCH NEXT ' + CONVERT(NVARCHAR(MAX),@PageSize) + ' ROWS ONLY' ELSE '' END
	
	DECLARE @FromDate DATE = NULL
	DECLARE @ToDate DATE

	IF @FromJDate IS NOT NULL 
	BEGIN
		SET @FromDate = AZ.ConvertFromJalaliDate(@FromJDate)
	END
	IF @ToJDate IS NOT NULL
	BEGIN
		SET @ToDate = AZ.ConvertFromJalaliDate(@ToJDate)
	END

	SELECT Field.value('@ISV','SMALLINT') IdentificationStateVCode
	INTO #IdentificationStates FROM @IdentificationStates.nodes('IS') Details(Field)
	
	DECLARE @Statement NVARCHAR(MAX) = 
		CASE WHEN @HasComplaint = 1 THEN '
		SELECT	Field.value(''@S'',''SMALLINT'') ComplaintStateVCode
		INTO #ComplaintStates FROM @IdentificationComplaintStateVCodes.nodes(''ICSV'') Details(Field) ' 
		ELSE ' ' END + ' 
		SELECT	ROW_NUMBER() OVER(ORDER BY I.VCode DESC) R,
				I.*,
				UPT.[Name] UserPaymentTypeName,
				CRR.TrackingCode,
				OP_OUTER.TraceNo,
				OP_OUTER.PaymentAmount,
				OP_OUTER.PaymentDate,
				(''<IdentificationStates>'' + (SELECT * FROM AZ.IdentificationState (NOLOCK) WHERE VCode = I.IdentificationStateVCode FOR XML PATH(''IdentificationState'')) + ''</IdentificationStates>'') IdentificationState,
				(''<IdentificationTypes>'' + (SELECT * FROM AZ.IdentificationType (NOLOCK) WHERE VCode = I.IdentificationTypeVCode FOR XML PATH(''IdentificationType'')) + ''</IdentificationTypes>'') IdentificationType,
				(''<IdentificationSendToOthers>'' + (SELECT * FROM AZ.IdentificationSendToOthers (NOLOCK) WHERE IdentificationVCode = I.VCode FOR XML PATH(''IdentificationSendToOther'')) + ''</IdentificationSendToOthers>'') IdentificationSendToOthers,
				BatchJSon.ResponseJson
		INTO #T FROM AZ.Identification (NOLOCK) I
		LEFT JOIN AZ.CreditRiskReport (NOLOCK) CRR ON I.VCode = CRR.IdentificationVCode
		LEFT JOIN ICEUserManagement.AZ.UserPaymentType (NOLOCK) UPT ON UPT.VCode = I.UserPaymentTypeVCode 
		OUTER APPLY
		(
			SELECT TOP 1 BR.ResponseJson FROM ICECore.AZ.Batch (NOLOCK) B
			INNER JOIN ICECore.AZ.BatchResult (NOLOCK) BR ON B.VCode = BR.BatchVCode
			WHERE B.IdentificationVCode = I.VCode
		) BatchJSon
		OUTER APPLY 
		(
			SELECT TOP 1 OPR.TraceNo,OPR.EntryDate PaymentDate,OP.Amount PaymentAmount FROM AZ.OnlinePayment (NOLOCK) OP
			INNER JOIN AZ.OnlinePaymentReceived (NOLOCK) OPR ON OP.VCode = OPR.OnlinePaymentVCode
			WHERE OP.IdentificationVCode = I.VCode
		) OP_OUTER
		WHERE ISNULL(Cellphone,0) = ISNULL(@Cellphone,ISNULL(Cellphone,0))
			AND ISNULL(NationalCode,0) = ISNULL(@NationalCode,ISNULL(NationalCode,0))
			AND ISNULL(CompanyNationalId,0) = ISNULL(@CompanyNationalId,ISNULL(CompanyNationalId,0))
			AND ISNULL(UserVCode,0) = ISNULL(@UserVCode,ISNULL(UserVCode,0))
			AND ISNULL(CRR.TrackingCode,0) = ISNULL(@TrackingCode,ISNULL(CRR.TrackingCode,0))
			AND (@IdentificationStates IS NULL OR (@IdentificationStates IS NOT NULL AND EXISTS(SELECT 1 FROM #IdentificationStates WHERE IdentificationStateVCode = I.IdentificationStateVCode)))
			AND CAST(I.EntryDate AS DATE) BETWEEN ISNULL(@FromDate,CAST(I.EntryDate AS DATE)) AND ISNULL(@ToDate,CAST(I.EntryDate AS DATE)) ' + 
		CASE WHEN @WithoutPartners = 1 THEN ' AND NOT EXISTS(SELECT 1 FROM [IceUserManagement].AZ.[User] WHERE VCode = I.UserVCode AND UserTypeVCode = 1) ' ELSE '' END	+
		CASE WHEN @WithouExpired = 1 THEN ' AND I.ReportExpirationDate > GETDATE() ' ELSE '' END	+ 
		CASE WHEN @HasComplaint = 1 THEN ' AND EXISTS(SELECT 1 FROM AZ.IdentificationComplaint (NOLOCK) IC WHERE IC.IdentificationVCode = I.VCode) '  ELSE ' ' END + 
		CASE WHEN @HasComplaint = 1 AND @IdentificationComplaintStateVCodes IS NOT NULL THEN ' AND EXISTS (SELECT 1 FROM #ComplaintStates CS WHERE CS.ComplaintStateVCode = I.IdentificationComplaintStateVCode) ' ELSE ' ' END +
		CASE WHEN @IdentificationTypeVCode IS NOT NULL THEN ' AND I.IdentificationTypeVCode = @IdentificationTypeVCode ' ELSE ' ' END + 
		' ORDER BY I.VCode DESC
		OFFSET ' + @RowOffsetStatement + ' ROWS ' + @RowPageStatement + 
		CASE WHEN @HasComplaint = 1 
			THEN ' 
				UPDATE AZ.Identification SET IdentificationComplaintStateVCode = 3 WHERE VCode IN (SELECT VCode FROM #T WHERE IdentificationComplaintStateVCode = 2) 
				INSERT AZ.IdentificationComplaintHistory(IdentificationVCode,IdnetificationComplaintStateVCode) SELECT VCode,3 FROM #T WHERE IdentificationComplaintStateVCode = 2

				DROP TABLE #ComplaintStates
			' 
			ELSE ' ' 
		END + '
		SELECT * FROM #T

		DROP TABLE #T
		'

	EXEC sp_executesql @stmt = @Statement,@params = N'@NationalCode AS VARCHAR(20),@Cellphone VARCHAR(20),@UserVCode INT,@IdentificationStates XML,@FromDate DATE,@ToDate DATE,@IdentificationTypeVCode INT,@IdentificationComplaintStateVCodes XML,@CompanyNationalId VARCHAR(20),@TrackingCode BIGINT',
		@NationalCode = @NationalCode,@Cellphone = @Cellphone,@UserVCode = @UserVCode,@IdentificationStates = @IdentificationStates,@FromDate = @FromDate,@ToDate = @ToDate,@IdentificationTypeVCode = @IdentificationTypeVCode,@IdentificationComplaintStateVCodes = @IdentificationComplaintStateVCodes,@CompanyNationalId = @CompanyNationalId,@TrackingCode = @TrackingCode

	DROP TABLE #IdentificationStates

END
