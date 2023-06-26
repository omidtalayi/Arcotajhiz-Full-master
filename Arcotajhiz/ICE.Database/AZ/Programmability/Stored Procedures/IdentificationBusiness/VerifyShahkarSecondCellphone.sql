CREATE PROCEDURE [AZ].[VerifyShahkarSecondCellphone](
	@IdentificationVCode BIGINT,
    @SecondCellphone VARCHAR(12),
	@NationalCode VARCHAR(20),
	@HasShahkarIdentified BIT,
    @ShahkarState SMALLINT OUTPUT,
    @ShahkarStateDescription VARCHAR(500) OUTPUT
)
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION

		IF EXISTS(SELECT 1 FROM AZ.Identification WHERE VCode = @IdentificationVCode AND SecondCellphone = @SecondCellphone AND NationalCode = @NationalCode)
		BEGIN
			--IF NOT EXISTS(SELECT 1 FROM AZ.IdentificationHistory WHERE IdentificationVCode = @IdentificationVCode AND IdentificationStateVCode = 2)
			--BEGIN
			--	UPDATE [AZ].[Identification] SET IdentificationStateVCode = 2,LastModifiedDate = GETDATE() WHERE Cellphone = @Cellphone AND VCode = @IdentificationVCode
			--	INSERT AZ.IdentificationHistory(IdentificationVCode,IdentificationStateVCode) VALUES(@IdentificationVCode,2)
			--END

			IF @HasShahkarIdentified = 1
			BEGIN
				UPDATE [AZ].[Identification] SET HasShahkarIdentified = 1,LastModifiedDate = GETDATE() WHERE SecondCellphone = @SecondCellphone AND VCode = @IdentificationVCode
				--IF NOT EXISTS(SELECT 1 FROM AZ.IdentificationHistory WHERE IdentificationVCode = @IdentificationVCode AND IdentificationStateVCode = 12)
				--BEGIN
				--	INSERT AZ.IdentificationHistory(IdentificationVCode,IdentificationStateVCode) VALUES(@IdentificationVCode,12)
				--END
				SELECT @ShahkarState = 1,@ShahkarStateDescription = 'توسط شاهکار تایید شد'
			END
			ELSE
			BEGIN
				UPDATE [AZ].[Identification] SET HasShahkarIdentified = @HasShahkarIdentified,LastModifiedDate = GETDATE() WHERE SecondCellphone = @SecondCellphone AND VCode = @IdentificationVCode
				--IF NOT EXISTS(SELECT 1 FROM AZ.IdentificationHistory WHERE IdentificationVCode = @IdentificationVCode AND IdentificationStateVCode = 6)
				--BEGIN
				--	INSERT AZ.IdentificationHistory(IdentificationVCode,IdentificationStateVCode) VALUES(@IdentificationVCode,6)
				--END
				SELECT @ShahkarState = 2,@ShahkarStateDescription = 'شماره موبایل با کد ملی مطابقت ندارد'
			END
			UPDATE AZ.[Identification] SET SecondCellphoneHasCheckedKyc = 1 WHERE VCode = @IdentificationVCode
		END
		ELSE
		BEGIN
			RAISERROR('(Suspicious) Cellphone and nationalCode does not belong to this identification',16,126)
		END

		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		DECLARE @ErrNo INT,@ErrMsg NVARCHAR(4000),@ErrSev INT,@ErrStt INT
		SELECT @ErrMsg = '(' + CONVERT(NVARCHAR(4000),ERROR_NUMBER()) + ') ' + ERROR_MESSAGE(),@ErrSev = ERROR_SEVERITY(),@ErrStt = ERROR_STATE()
		IF @@TRANCOUNT > 0 ROLLBACK TRAN
		RAISERROR(@ErrMsg,@ErrSev,@ErrStt)
	END CATCH
END
