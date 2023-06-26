CREATE PROCEDURE [AZ].[VerifyShahkar](
	@IdentificationVCode BIGINT,
    @Cellphone VARCHAR(12),
	@NationalCode VARCHAR(20),
	@HasShahkarIdentified BIT,
    @ShahkarState SMALLINT OUTPUT,
    @ShahkarStateDescription VARCHAR(500) OUTPUT
)
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION

		DECLARE @HasCheckedKyc BIT,
				@SecondCellphone NVARCHAR(20)

		SELECT @SecondCellphone = SecondCellphone FROM AZ.Identification WHERE VCode = @IdentificationVCode

		IF @SecondCellphone IS NULL
		BEGIN
			IF EXISTS(SELECT 1 FROM AZ.Identification WHERE VCode = @IdentificationVCode AND Cellphone = @Cellphone AND NationalCode = @NationalCode)
			BEGIN
				IF NOT EXISTS(SELECT 1 FROM AZ.IdentificationHistory WHERE IdentificationVCode = @IdentificationVCode AND IdentificationStateVCode = 2)
				BEGIN
					UPDATE [AZ].[Identification] SET IdentificationStateVCode = 2,LastModifiedDate = GETDATE() WHERE Cellphone = @Cellphone AND VCode = @IdentificationVCode
					INSERT AZ.IdentificationHistory(IdentificationVCode,IdentificationStateVCode) VALUES(@IdentificationVCode,2)
				END

				SELECT @HasCheckedKyc = HasCheckedKyc,@HasShahkarIdentified = HasShahkarIdentified FROM AZ.Identification WHERE VCode = @IdentificationVCode

			
				IF @HasCheckedKyc = 1
				BEGIN
					IF @HasShahkarIdentified = 1
					BEGIN
						SELECT @ShahkarState = 1,@ShahkarStateDescription = 'توسط شاهکار تایید شد'
					END
					ELSE
					BEGIN
						SELECT @ShahkarState = 2,@ShahkarStateDescription = 'شماره موبایل با کد ملی مطابقت ندارد'
					END
				END
				ELSE
				BEGIN
					IF @HasShahkarIdentified = 1
					BEGIN
						UPDATE [AZ].[Identification] SET IdentificationStateVCode = 12,LastModifiedDate = GETDATE() WHERE Cellphone = @Cellphone AND VCode = @IdentificationVCode
						IF NOT EXISTS(SELECT 1 FROM AZ.IdentificationHistory WHERE IdentificationVCode = @IdentificationVCode AND IdentificationStateVCode = 12)
						BEGIN
							INSERT AZ.IdentificationHistory(IdentificationVCode,IdentificationStateVCode) VALUES(@IdentificationVCode,12)
						END
						SELECT @ShahkarState = 1,@ShahkarStateDescription = 'توسط شاهکار تایید شد'
					END
					ELSE
					BEGIN
						UPDATE [AZ].[Identification] SET IdentificationStateVCode = 6,LastModifiedDate = GETDATE() WHERE Cellphone = @Cellphone AND VCode = @IdentificationVCode
						IF NOT EXISTS(SELECT 1 FROM AZ.IdentificationHistory WHERE IdentificationVCode = @IdentificationVCode AND IdentificationStateVCode = 6)
						BEGIN
							INSERT AZ.IdentificationHistory(IdentificationVCode,IdentificationStateVCode) VALUES(@IdentificationVCode,6)
						END
						SELECT @ShahkarState = 2,@ShahkarStateDescription = 'شماره موبایل با کد ملی مطابقت ندارد'
					END
					UPDATE AZ.[Identification] SET HasCheckedKyc = 1 WHERE VCode = @IdentificationVCode
				END
			END
			ELSE
			BEGIN
				RAISERROR('(Suspicious) Cellphone and nationalCode does not belong to this identification',16,126)
			END
		END
		ELSE
		BEGIN
			IF EXISTS(SELECT 1 FROM AZ.Identification WHERE VCode = @IdentificationVCode AND Cellphone = @Cellphone AND NationalCode = @NationalCode)
			BEGIN
				IF @HasShahkarIdentified = 1
				BEGIN
					UPDATE [AZ].[Identification] SET LastModifiedDate = GETDATE() WHERE SecondCellphone = @SecondCellphone AND VCode = @IdentificationVCode
					SELECT @ShahkarState = 1,@ShahkarStateDescription = 'توسط شاهکار تایید شد'
				END
				ELSE
				BEGIN
					UPDATE [AZ].[Identification] SET LastModifiedDate = GETDATE() WHERE SecondCellphone = @SecondCellphone AND VCode = @IdentificationVCode
					SELECT @ShahkarState = 2,@ShahkarStateDescription = 'شماره موبایل دوم با کد ملی مطابقت ندارد'
				END
				UPDATE AZ.[Identification] SET SecondCellphoneHasCheckedKyc = 1 WHERE VCode = @IdentificationVCode
			END
			ELSE
			BEGIN
				RAISERROR('(Suspicious) Cellphone and nationalCode does not belong to this identification',16,126)
			END
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
GO

