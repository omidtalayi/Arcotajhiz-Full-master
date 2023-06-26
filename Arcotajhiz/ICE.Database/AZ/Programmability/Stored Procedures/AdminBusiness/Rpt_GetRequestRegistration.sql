CREATE PROCEDURE [AZ].[Rpt_GetRequestRegistration](
	@FromRequestRegistrationJDate NVARCHAR(8) = NULL,
	@ToRequestRegistrationJDate NVARCHAR(8) = NULL,
	@RequestRegistrationStateVCode INT = NULL,
	@PageSize INT = NULL,
	@PageNo INT = NULL,
	@RowCount INT = NULL,
	@RequestRegistrationType INT = NULL
)
AS
BEGIN
	DECLARE @RowOffsetStatement NVARCHAR(MAX) = CONVERT(NVARCHAR(MAX),ISNULL((@PageNo - 1) * @PageSize,0))
	DECLARE @RowPageStatement NVARCHAR(MAX) = CASE WHEN @PageNo IS NOT NULL AND @PageSize IS NOT NULL THEN 'FETCH NEXT ' + CONVERT(NVARCHAR(MAX),@PageSize) + ' ROWS ONLY' ELSE '' END

	DECLARE @FromDate DATE = NULL
	DECLARE @ToDate DATE = NULL

	IF @FromRequestRegistrationJDate IS NOT NULL 
	BEGIN
		SET @FromDate = AZ.ConvertFromJalaliDate(@FromRequestRegistrationJDate)
	END
	IF @ToRequestRegistrationJDate IS NOT NULL
	BEGIN
		SET @ToDate = AZ.ConvertFromJalaliDate(@ToRequestRegistrationJDate)
	END
	DECLARE @Statement NVARCHAR(MAX) = '
	SELECT 
			[RowCount] = COUNT(1) OVER(),
			ROW_NUMBER() OVER(ORDER BY RR.VCode) [Row],
			RR.VCode [VCode],
			RR.FirmName [FirmName],
			RR.RegisteredIdentificationNo [RegisteredIdentificationNo],
			RR.RegistrationNo [RegistrationNo],
			RRS.[Name] [RequestRegistrationState],
			RR.[Email] [Email],
			RR.[Telephone] [Cellphone],
			CAST(RR.ContactPointName + '' '' +RR.ContactPointFamilyName AS NVARCHAR(MAX)) [ContactPointFullName],
			RR.ContactPointCellphone ContactPointCellphone,
			RR.RequestRegistrationTypeVCode RequestRegistrationTypeVCode
	FROM AZ.RequestRegistration (NOLOCK) RR
		INNER JOIN	AZ.RequestRegistrationState (NOLOCK) RRS ON RRS.VCode = RR.RequestRegistrationStateVCode
	WHERE CAST(RR.EntryDate AS DATE) BETWEEN ISNULL(@FromDate,CAST(RR.EntryDate AS DATE)) AND ISNULL(@ToDate,CAST(RR.EntryDate AS DATE))
	AND RRS.VCode = ISNULL(@RequestRegistrationStateVCode,RRS.VCode)
	AND RR.RequestRegistrationTypeVCode = ISNULL(@RequestRegistrationType,RR.RequestRegistrationTypeVCode)
	ORDER BY RR.EntryDate DESC
	OFFSET ' + @RowOffsetStatement + ' ROWS ' + @RowPageStatement 

	EXEC sp_executesql @stmt = @Statement,@params = N'@FromDate DATE,@ToDate DATE,@RequestRegistrationStateVCode INT,@RequestRegistrationType INT',@FromDate = @FromDate,@ToDate = @ToDate,@RequestRegistrationStateVCode = @RequestRegistrationStateVCode,@RequestRegistrationType = @RequestRegistrationType
END
GO