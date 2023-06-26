CREATE PROCEDURE [AZ].[IdentificationStateUpdICS24](
	@IdentificationVCode BIGINT,
	@IdentificationStateVCode SMALLINT,
	@ShahkarState BIT,
	@IsLegalPersonState BIT
)
AS
BEGIN
	BEGIN TRAN
	BEGIN TRY
		IF NOT EXISTS(SELECT 1 FROM AZ.IdentificationHistory WHERE IdentificationVCode = @IdentificationVCode AND IdentificationStateVCode = @IdentificationStateVCode)
		BEGIN
			UPDATE AZ.[Identification] SET IdentificationStateVCode = @IdentificationStateVCode,HasCheckedKyc = 1,HasShahkarIdentified = @ShahkarState,IsLegalPerson = @IsLegalPersonState
			WHERE VCode = @IdentificationVCode

			INSERT AZ.IdentificationHistory(IdentificationVCode,IdentificationStateVCode) VALUES(@IdentificationVCode,@IdentificationStateVCode)
		END
		IF EXISTS(SELECT 1 FROM AZ.Identification WHERE VCode = @IdentificationVCode AND SecondCellphone IS NOT NULL AND HasShahkarIdentified = 1)
		BEGIN
			UPDATE AZ.[Identification] SET SecondCellphoneHasCheckedKyc = 1 WHERE VCode = @IdentificationVCode
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