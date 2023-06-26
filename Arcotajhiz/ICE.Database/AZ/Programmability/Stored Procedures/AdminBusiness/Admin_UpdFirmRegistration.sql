CREATE PROCEDURE [AZ].[Admin_UpdFirmRegistration]
	@FirmRegistrationVCode int = 0,
	@Description NVARCHAR(MAX) = NULL
AS
BEGIN
BEGIN TRAN
	BEGIN TRY
		
		UPDATE AZ.FirmRegistration 
		SET [Description] = ISNULL(@Description,[Description])
		WHERE VCode = @FirmRegistrationVCode

	COMMIT TRAN
	END TRY
		BEGIN CATCH
		DECLARE @ErrNo INT,@ErrMsg NVARCHAR(4000),@ErrSev INT,@ErrStt INT
		SELECT @ErrMsg = '(' + CONVERT(NVARCHAR(4000),ERROR_NUMBER()) + ') ' + ERROR_MESSAGE(),@ErrSev = ERROR_SEVERITY(),@ErrStt = ERROR_STATE()
		IF @@TRANCOUNT > 0 ROLLBACK TRAN
		RAISERROR(@ErrMsg,@ErrSev,@ErrStt)
	END CATCH
END
