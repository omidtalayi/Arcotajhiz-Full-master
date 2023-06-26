CREATE PROCEDURE [AZ].[VerifySecondCellphone](
	@IdentificationVCode BIGINT,
	@SecondCellphoneVerificationCode VARCHAR(MAX),
    @SecondCellphone VARCHAR(12),
    @VerificationState SMALLINT OUTPUT,
    @VerificationStateDescription VARCHAR(500) OUTPUT
)
AS
BEGIN
	BEGIN TRY
		IF NOT EXISTS(SELECT 1 FROM [AZ].[Identification] WHERE VCode = @IdentificationVCode AND SecondCellphone = @SecondCellphone)	
		BEGIN
			SELECT @VerificationState = 4,@VerificationStateDescription = 'موبایل موردنظر یافت نشد'
			RETURN
		END
		IF EXISTS(SELECT 1 FROM [AZ].[Identification] WHERE VCode = @IdentificationVCode AND SecondCellphone = @SecondCellphone AND SecondCellphoneExpirationDate < GETDATE()) 
		BEGIN 
			SELECT @VerificationState = 3,@VerificationStateDescription = 'کد تایید منقضی شده است'
			RETURN
		END
		IF EXISTS(SELECT 1 FROM [AZ].[Identification] WHERE VCode = @IdentificationVCode AND SecondCellphone = @SecondCellphone AND SecondCellphoneVerificationCode <> @SecondCellphoneVerificationCode) BEGIN 
			SELECT @VerificationState = 2,@VerificationStateDescription = 'کد تایید وارد شده صحیح نمی باشد'
			RETURN 
		END

		UPDATE [AZ].[Identification] SET SecondCellphoneIsVerified = 1,LastModifiedDate = GETDATE() WHERE VCode = @IdentificationVCode AND SecondCellphone = @SecondCellphone
		SELECT @VerificationState = 1,@VerificationStateDescription = 'موبایل موردنظر تایید شد'
	END TRY
	BEGIN CATCH
		DECLARE @ErrNo INT,@ErrMsg NVARCHAR(4000),@ErrSev INT,@ErrStt INT
		SELECT @ErrMsg = '(' + CONVERT(NVARCHAR(4000),ERROR_NUMBER()) + ') ' + ERROR_MESSAGE(),@ErrSev = ERROR_SEVERITY(),@ErrStt = ERROR_STATE()
		IF @@TRANCOUNT > 0 ROLLBACK TRAN
		RAISERROR(@ErrMsg,@ErrSev,@ErrStt)
	END CATCH
END