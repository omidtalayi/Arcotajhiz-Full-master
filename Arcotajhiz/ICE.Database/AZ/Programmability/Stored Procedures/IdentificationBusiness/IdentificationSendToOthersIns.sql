CREATE PROCEDURE [AZ].[IdentificationSendToOthersIns](
	@IdentificationVCode BIGINT,
	@IdentificationSendToOthers XML
)
AS
BEGIN
	BEGIN TRAN
	BEGIN TRY

		SELECT	Field.value('@ISTOT','SMALLINT') IdentificationSendToOthersTypeVCode,
				Field.value('@R','VARCHAR(500)') Receiver
		INTO #Data FROM @IdentificationSendToOthers.nodes('I') Details(Field)
	
		INSERT AZ.IdentificationSendToOthers(IdentificationVCode,IdentificationSendToOthersTypeVCode,Receiver)
		SELECT @IdentificationVCode,IdentificationSendToOthersTypeVCode,Receiver FROM #Data

		DROP TABLE #Data

		COMMIT TRAN
	END TRY
		BEGIN CATCH
		DECLARE @ErrNo INT,@ErrMsg NVARCHAR(4000),@ErrSev INT,@ErrStt INT
		SELECT @ErrMsg = '(' + CONVERT(NVARCHAR(4000),ERROR_NUMBER()) + ') ' + ERROR_MESSAGE(),@ErrSev = ERROR_SEVERITY(),@ErrStt = ERROR_STATE()
		IF @@TRANCOUNT > 0 ROLLBACK TRAN
		RAISERROR(@ErrMsg,@ErrSev,@ErrStt)
	END CATCH
END