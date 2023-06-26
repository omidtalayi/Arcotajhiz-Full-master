CREATE PROCEDURE [AZ].[Rpt_GetFirmRegistration](
	@FromFirmRegistrationJDate NVARCHAR(8) = NULL,
	@ToFirmRegistrationJDate NVARCHAR(8) = NULL,
	@FirmRegistrationStateVCode INT = NULL,
	@PageSize INT = NULL,
	@PageNo INT = NULL,
	@RowCount INT = NULL
)
AS
BEGIN
	DECLARE @RowOffsetStatement NVARCHAR(MAX) = CONVERT(NVARCHAR(MAX),ISNULL((@PageNo - 1) * @PageSize,0))
	DECLARE @RowPageStatement NVARCHAR(MAX) = CASE WHEN @PageNo IS NOT NULL AND @PageSize IS NOT NULL THEN 'FETCH NEXT ' + CONVERT(NVARCHAR(MAX),@PageSize) + ' ROWS ONLY' ELSE '' END

	DECLARE @FromDate DATE = NULL
	DECLARE @ToDate DATE = NULL

	IF @FromFirmRegistrationJDate IS NOT NULL 
	BEGIN
		SET @FromDate = AZ.ConvertFromJalaliDate(@FromFirmRegistrationJDate)
	END
	IF @ToFirmRegistrationJDate IS NOT NULL
	BEGIN
		SET @ToDate = AZ.ConvertFromJalaliDate(@ToFirmRegistrationJDate)
	END

	DECLARE @Statement NVARCHAR(MAX) = '
		SELECT	[RowCount] = COUNT(1) OVER(),
				ROW_NUMBER() OVER(ORDER BY FR.VCode) [Row],
				FR.VCode [VCode],
				FR.FirmName [FirmName],
				CAST(FR.ContactPointName + '' '' +FR.ContactPointFamilyName AS NVARCHAR(MAX)) [ContactPointFullName],
				FR.RegisteredIdentificationNo [RegisteredIdentificationNo],
				FR.RegistrationNo [RegistrationNo],
				FRS.[Name] [FirmRegisrationStateName],
				FR.[Email] [Email],
				FR.[Telephone] [Cellphone],
				FR.ContactPointCellphone ContactPointCellphone,
				SUBSTRING(AZ.ConvertToJalaliDate(FR.EntryDate),1,4) + ''/'' +SUBSTRING(AZ.ConvertToJalaliDate(FR.EntryDate),5,2) + ''/'' +SUBSTRING(AZ.ConvertToJalaliDate(FR.EntryDate),7,2)  ConfirmedJDate,
				CAST(DATEPART(HOUR,FR.EntryDate) AS NVARCHAR(MAX)) +'':''+ CAST(DATEPART(MINUTE,FR.EntryDate) AS NVARCHAR(MAX)) ConfirmedJDateTime
		FROM AZ.FirmRegistration (NOLOCK) FR 
			INNER JOIN	AZ.FirmRegistrationState (NOLOCK) FRS ON FRS.VCode = FR.FirmRegistrationStateVCode
		WHERE CAST(FR.EntryDate AS DATE) BETWEEN ISNULL(@FromDate,CAST(FR.EntryDate AS DATE)) AND ISNULL(@ToDate,CAST(FR.EntryDate AS DATE))
		AND FRS.VCode = ISNULL(@FirmRegistrationStateVCode,FRS.VCode)
		ORDER BY FR.EntryDate DESC
		OFFSET ' + @RowOffsetStatement + ' ROWS ' + @RowPageStatement 

	EXEC sp_executesql @stmt = @Statement,@params = N'@FromDate DATE,@ToDate DATE,@FirmRegistrationStateVCode INT',@FromDate = @FromDate,@ToDate = @ToDate,@FirmRegistrationStateVCode = @FirmRegistrationStateVCode

END
