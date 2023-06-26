CREATE PROCEDURE [AZ].[VerifyIndividual](
	@IdentificationVCode BIGINT,
	@NationalCode VARCHAR(20),
	@HasRejected BIT = 0,
    @VerificationState SMALLINT OUTPUT,
    @VerificationStateDescription VARCHAR(500) OUTPUT
)
AS
BEGIN
	BEGIN TRY
		IF @HasRejected = 1 AND EXISTS(SELECT 1 FROM AZ.Identification WHERE VCode = @IdentificationVCode)
		BEGIN
			SELECT @VerificationState = 7,@VerificationStateDescription = 'شخص درخواست مشاهده رتبه اعتباری را رد کرد'
			UPDATE AZ.[Identification] SET IdentificationStateVCode = 7,LastModifiedDate = GETDATE() WHERE VCode = @IdentificationVCode 
			INSERT AZ.IdentificationHistory(IdentificationVCode,IdentificationStateVCode) VALUES(@IdentificationVCode,7)
			RETURN
		END
		IF EXISTS(SELECT 1 FROM [AZ].[Identification] WHERE VCode = @IdentificationVCode AND NationalCode = @NationalCode AND IdentificationStateVCode = 16)
		BEGIN
			SELECT @VerificationState = 8,@VerificationStateDescription = 'تعداد دفعات مجاز شما برای وارد کردن کد ملی تمام شده است'
			RETURN
		END
		IF NOT EXISTS(SELECT 1 FROM [AZ].[Identification] WHERE VCode = @IdentificationVCode AND NationalCode = @NationalCode)	
		BEGIN
			SELECT @VerificationState = 6,@VerificationStateDescription = 'کد ملی موردنظر یافت نشد'

			IF (SELECT IncorrectNationalCodeCount FROM [AZ].[Identification] WHERE VCode = @IdentificationVCode) = 3
			BEGIN
				IF NOT EXISTS(SELECT 1 FROM AZ.IdentificationHistory WHERE IdentificationVCode = @IdentificationVCode AND IdentificationStateVCode = 16)
				BEGIN
					UPDATE AZ.[Identification] SET IdentificationStateVCode = 16,LastModifiedDate = GETDATE() WHERE VCode = @IdentificationVCode 
					INSERT AZ.IdentificationHistory(IdentificationVCode,IdentificationStateVCode) VALUES(@IdentificationVCode,16)
				END
			END
			ELSE
			BEGIN
				UPDATE AZ.Identification SET IncorrectNationalCodeCount = IncorrectNationalCodeCount + 1 WHERE VCode = @IdentificationVCode
			END
			RETURN
		END
		IF EXISTS(SELECT 1 FROM [AZ].[Identification] WHERE VCode = @IdentificationVCode AND NationalCode = @NationalCode AND ReportExpirationDate < GETDATE()) 
		BEGIN 
			SELECT @VerificationState = 5,@VerificationStateDescription = 'لینک تایید منقضی شده است'
			UPDATE AZ.[Identification] SET IdentificationStateVCode = 5,LastModifiedDate = GETDATE() WHERE VCode = @IdentificationVCode 
			INSERT AZ.IdentificationHistory(IdentificationVCode,IdentificationStateVCode) VALUES(@IdentificationVCode,5)
			RETURN
		END
		
		BEGIN TRANSACTION
		
		IF NOT EXISTS(SELECT 1 FROM AZ.IdentificationHistory WHERE IdentificationVCode = @IdentificationVCode AND IdentificationStateVCode = 3)
		BEGIN
			UPDATE [AZ].[Identification] SET IsVerified = 1,IdentificationStateVCode = 3,LastModifiedDate = GETDATE() WHERE VCode = @IdentificationVCode 
			INSERT AZ.IdentificationHistory(IdentificationVCode,IdentificationStateVCode) VALUES(@IdentificationVCode,3)
		END

		COMMIT TRANSACTION

		SELECT @VerificationState = 1,@VerificationStateDescription = 'لینک موردنظر تایید شد'
	END TRY
	BEGIN CATCH
		DECLARE @ErrNo INT,@ErrMsg NVARCHAR(4000),@ErrSev INT,@ErrStt INT
		SELECT @ErrMsg = '(' + CONVERT(NVARCHAR(4000),ERROR_NUMBER()) + ') ' + ERROR_MESSAGE(),@ErrSev = ERROR_SEVERITY(),@ErrStt = ERROR_STATE()
		IF @@TRANCOUNT > 0 ROLLBACK TRAN
		RAISERROR(@ErrMsg,@ErrSev,@ErrStt)
	END CATCH
END