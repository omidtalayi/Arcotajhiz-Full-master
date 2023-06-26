CREATE PROCEDURE [AZ].[CheckIndividualReportExistenceIns](
	@VCode			BIGINT OUTPUT,
	@UserVCode		BIGINT,
	@NationalCode	NVARCHAR(20),
	@Cellphone		NVARCHAR(20),
	@ReportSources	XML,
	@IP				NVARCHAR(100)
)
AS
BEGIN
	BEGIN TRY
		SELECT Field.value('@V','SMALLINT') ReportSourceVCode
		INTO #ReportSources FROM @ReportSources.nodes('RS') Details(Field)

		BEGIN TRAN

		INSERT AZ.CheckIndividualReportExistence(UserVCode,NationalCode,Cellphone,[IP])
		VALUES(@UserVCode,@NationalCode,@Cellphone,@IP)

		SET @VCode = SCOPE_IDENTITY()

		INSERT AZ.CheckIndividualReportExistenceReportSource(CheckIndividualReportExistenceVCode,ReportSourceVCode)
		SELECT @VCode,ReportSourceVCode FROM #ReportSources
		
		COMMIT TRAN

		DROP TABLE #ReportSources
	END TRY
		BEGIN CATCH
		DECLARE @ErrNo INT,@ErrMsg NVARCHAR(4000),@ErrSev INT,@ErrStt INT
		SELECT @ErrMsg = '(' + CONVERT(NVARCHAR(4000),ERROR_NUMBER()) + ') ' + ERROR_MESSAGE(),@ErrSev = ERROR_SEVERITY(),@ErrStt = ERROR_STATE()
		IF @@TRANCOUNT > 0 ROLLBACK TRAN
		RAISERROR(@ErrMsg,@ErrSev,@ErrStt)
	END CATCH
END