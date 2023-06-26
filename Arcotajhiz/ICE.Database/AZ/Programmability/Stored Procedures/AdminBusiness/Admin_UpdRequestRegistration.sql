CREATE PROCEDURE [AZ].[Admin_UpdRequestRegistration]
	@RequestRegistrationVCode int = 0,
	@FirmRegistrationVCode int = 0,
	@Description NVARCHAR(MAX) = NULL
AS
BEGIN
BEGIN TRAN
	BEGIN TRY
		
		IF NOT EXISTS(SELECT 1 FROM AZ.RequestRegistration WHERE VCode = @RequestRegistrationVCode AND FirmRegistrationVCode = @FirmRegistrationVCode)
		BEGIN
			RAISERROR('(Suspicious) FirmRegistrationVCode does not belong to this requestRegistration',16,126)
		END

		UPDATE AZ.RequestRegistration 
		SET [Description] = ISNULL(@Description,[Description])
		WHERE VCode = @RequestRegistrationVCode

	COMMIT TRAN
	END TRY
		BEGIN CATCH
		DECLARE @ErrNo INT,@ErrMsg NVARCHAR(4000),@ErrSev INT,@ErrStt INT
		SELECT @ErrMsg = '(' + CONVERT(NVARCHAR(4000),ERROR_NUMBER()) + ') ' + ERROR_MESSAGE(),@ErrSev = ERROR_SEVERITY(),@ErrStt = ERROR_STATE()
		IF @@TRANCOUNT > 0 ROLLBACK TRAN
		RAISERROR(@ErrMsg,@ErrSev,@ErrStt)
	END CATCH
END
GO

