﻿CREATE PROCEDURE [AZ].[SetIdentificationOtpLock](
	@identificationVCode BIGINT
)
AS
BEGIN
	BEGIN TRAN
	BEGIN TRY
		IF NOT EXISTS(SELECT 1 FROM AZ.IdentificationHistory WHERE IdentificationVCode = @IdentificationVCode AND IdentificationStateVCode = 19)
		BEGIN
			UPDATE AZ.Identification SET IdentificationStateVCode = 19,LastModifiedDate = GETDATE() WHERE VCode = @IdentificationVCode
			INSERT AZ.IdentificationHistory(IdentificationVCode,IdentificationStateVCode) VALUES(@IdentificationVCode,19)
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