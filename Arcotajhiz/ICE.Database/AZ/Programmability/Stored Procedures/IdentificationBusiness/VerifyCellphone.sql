CREATE PROCEDURE [AZ].[VerifyCellphone](
	@IdentificationVCode BIGINT,
	@VerificationCode VARCHAR(MAX),
    @Cellphone VARCHAR(12),
    @VerificationState SMALLINT OUTPUT,
    @VerificationStateDescription VARCHAR(500) OUTPUT
)
AS
BEGIN
	BEGIN TRY
		IF NOT EXISTS(SELECT 1 FROM [AZ].[Identification] WHERE VCode = @IdentificationVCode AND Cellphone = @Cellphone)	
		BEGIN
			SELECT @VerificationState = 4,@VerificationStateDescription = 'موبایل موردنظر یافت نشد'
			UPDATE AZ.[Identification] SET IdentificationStateVCode = 8,LastModifiedDate = GETDATE() WHERE VCode = @IdentificationVCode 
			INSERT AZ.IdentificationHistory(IdentificationVCode,IdentificationStateVCode) VALUES(@IdentificationVCode,8)
			RETURN
		END
		IF EXISTS(SELECT 1 FROM [AZ].[Identification] WHERE VCode = @IdentificationVCode AND Cellphone = @Cellphone AND ExpirationDate < GETDATE()) 
		BEGIN 
			SELECT @VerificationState = 3,@VerificationStateDescription = 'کد تایید منقضی شده است'
			--UPDATE AZ.[Identification] SET IdentificationStateVCode = 5,LastModifiedDate = GETDATE() WHERE VCode = @IdentificationVCode 
			--INSERT AZ.IdentificationHistory(IdentificationVCode,IdentificationStateVCode) VALUES(@IdentificationVCode,5)
			RETURN
		END
		IF EXISTS(SELECT 1 FROM [AZ].[Identification] WHERE VCode = @IdentificationVCode AND Cellphone = @Cellphone AND VerificationCode <> @VerificationCode) BEGIN 
			SELECT @VerificationState = 2,@VerificationStateDescription = 'کد تایید وارد شده صحیح نمی باشد'
			RETURN 
		END
		
		BEGIN TRANSACTION

		IF NOT EXISTS(SELECT 1 FROM AZ.IdentificationHistory WHERE IdentificationVCode = @IdentificationVCode AND IdentificationStateVCode = 3)
		BEGIN
			UPDATE [AZ].[Identification] SET IsVerified = 1,IdentificationStateVCode = 3,LastModifiedDate = GETDATE() WHERE VCode = @IdentificationVCode AND Cellphone = @Cellphone
			INSERT AZ.IdentificationHistory(IdentificationVCode,IdentificationStateVCode) VALUES(@IdentificationVCode,3)
		END
			
		COMMIT TRANSACTION

		SELECT @VerificationState = 1,@VerificationStateDescription = 'موبایل موردنظر تایید شد'
	END TRY
	BEGIN CATCH
		DECLARE @ErrNo INT,@ErrMsg NVARCHAR(4000),@ErrSev INT,@ErrStt INT
		SELECT @ErrMsg = '(' + CONVERT(NVARCHAR(4000),ERROR_NUMBER()) + ') ' + ERROR_MESSAGE(),@ErrSev = ERROR_SEVERITY(),@ErrStt = ERROR_STATE()
		IF @@TRANCOUNT > 0 ROLLBACK TRAN
		RAISERROR(@ErrMsg,@ErrSev,@ErrStt)
	END CATCH
END