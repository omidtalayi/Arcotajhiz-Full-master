CREATE PROCEDURE [AZ].[CancelIdentifiction](
	@IdentificationVCode BIGINT,
	@UserVCode BIGINT
)
AS
BEGIN
	BEGIN TRAN
	BEGIN TRY
		IF NOT EXISTS(SELECT 1 FROM AZ.Identification (NOLOCK) WHERE VCode = @IdentificationVCode AND UserVCode = @UserVCode)
		BEGIN
			RAISERROR('Identification does not belong to this user',16,126) 
		END

		IF EXISTS(SELECT 1 FROM AZ.Identification WHERE VCode = @IdentificationVCode AND IdentificationStateVCode = 1)
		BEGIN
			UPDATE AZ.[Identification] SET IdentificationStateVCode = 14,LastModifiedDate = GETDATE() WHERE VCode = @IdentificationVCode
			INSERT AZ.IdentificationHistory(IdentificationVCode,IdentificationStateVCode) VALUES(@IdentificationVCode,14)
		END

		IF EXISTS(SELECT 1 FROM AZ.Identification WHERE VCode = @IdentificationVCode AND IdentificationStateVCode = 3)
		BEGIN
			UPDATE AZ.[Identification] SET IdentificationStateVCode = 15,LastModifiedDate = GETDATE() WHERE VCode = @IdentificationVCode
			INSERT AZ.IdentificationHistory(IdentificationVCode,IdentificationStateVCode) VALUES(@IdentificationVCode,15)
		END

		COMMIT TRAN
	END TRY
		BEGIN CATCH
		DECLARE @ErrNo INT,@ErrMsg NVARCHAR(4000),@ErrSev INT,@ErrStt INT
		SELECT @ErrMsg = '(' + CONVERT(NVARCHAR(4000),ERROR_NUMBER()) + ') ' + ERROR_MESSAGE(),@ErrSev = ERROR_SEVERITY(),@ErrStt = ERROR_STATE()
		IF @@TRANCOUNT > 0 ROLLBACK TRAN
		RAISERROR(@ErrMsg,@ErrSev,@ErrStt)
	END CATCH
END