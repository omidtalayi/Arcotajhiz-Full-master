CREATE PROCEDURE [AZ].[IdentificationReportTypeIns](
	@IdentificationVCode BIGINT,
	@ReportTypes XML
)
AS
BEGIN
	BEGIN TRAN
	BEGIN TRY
		SELECT	Field.value('@RT','SMALLINT') ReportTypeVCode
		INTO #ReportTypes FROM @ReportTypes.nodes('IRT') Details(Field)

		INSERT AZ.IdentificationReportType(IdentificationVCode,ReportTypeVCode)
		SELECT @IdentificationVCode,ReportTypeVCode FROM #ReportTypes

		DROP TABLE #ReportTypes

		COMMIT TRAN
	END TRY
		BEGIN CATCH
		DECLARE @ErrNo INT,@ErrMsg NVARCHAR(4000),@ErrSev INT,@ErrStt INT
		SELECT @ErrMsg = '(' + CONVERT(NVARCHAR(4000),ERROR_NUMBER()) + ') ' + ERROR_MESSAGE(),@ErrSev = ERROR_SEVERITY(),@ErrStt = ERROR_STATE()
		IF @@TRANCOUNT > 0 ROLLBACK TRAN
		RAISERROR(@ErrMsg,@ErrSev,@ErrStt)
	END CATCH
END