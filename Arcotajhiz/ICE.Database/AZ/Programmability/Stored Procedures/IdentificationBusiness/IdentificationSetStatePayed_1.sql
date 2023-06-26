CREATE PROCEDURE [AZ].[IdentificationSetStatePayed](
	@IdentificationVCode BIGINT
)
AS
BEGIN
	BEGIN TRAN
	BEGIN TRY

		UPDATE az.Identification SET 
			IdentificationStateVCode = 11,
			AppIcs24HashCode = NULL,
			ReportExpirationDate = DATEADD(Day,1,GETDATE()), 
			ReportIsCorrupted = 0,
			HasShahkarIdentified = 0,
			ReportLink = NULL,
			HasCheckedKyc = 0,
			IsRepairMessageSent = 1
		WHERE VCode = @IdentificationVCode

		DELETE AZ.IdentificationHistory Where IdentificationStateVCode in(17,18,19) AND IdentificationVCode = @IdentificationVCode

		COMMIT TRAN
	END TRY
		BEGIN CATCH
		DECLARE @ErrNo INT,@ErrMsg NVARCHAR(4000),@ErrSev INT,@ErrStt INT
		SELECT @ErrMsg = '(' + CONVERT(NVARCHAR(4000),ERROR_NUMBER()) + ') ' + ERROR_MESSAGE(),@ErrSev = ERROR_SEVERITY(),@ErrStt = ERROR_STATE()
		IF @@TRANCOUNT > 0 ROLLBACK TRAN
		RAISERROR(@ErrMsg,@ErrSev,@ErrStt)
	END CATCH
END