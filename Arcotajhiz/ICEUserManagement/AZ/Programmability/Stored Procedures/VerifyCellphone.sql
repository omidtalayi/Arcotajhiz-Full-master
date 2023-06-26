CREATE PROCEDURE [AZ].[VerifyCellphone](
	@UserCellphoneVerificationCode VARCHAR(MAX),
    @Cellphone VARCHAR(12),
    @VerificationState SMALLINT OUTPUT,
    @VerificationStateDescription NVARCHAR(500) OUTPUT
)
AS
BEGIN
	BEGIN TRY
		IF NOT EXISTS(SELECT 1 FROM [AZ].[User] WHERE Cellphone = @Cellphone)	
		BEGIN
			SELECT @VerificationState = 4,@VerificationStateDescription = N'موبایل موردنظر یافت نشد'
			RETURN
		END
		IF EXISTS(SELECT 1 FROM [AZ].[User] WHERE  Cellphone = @Cellphone AND VerificationExpireDate < GETDATE()) 
		BEGIN 
			SELECT @VerificationState = 3,@VerificationStateDescription = N'کد تایید منقضی شده است'
			RETURN
		END
		IF EXISTS(SELECT 1 FROM [AZ].[User] WHERE Cellphone = @Cellphone AND VerificationCode <> @UserCellphoneVerificationCode) BEGIN 
			SELECT @VerificationState = 2,@VerificationStateDescription = N'کد تایید وارد شده صحیح نمی باشد'
			RETURN 
		END
		SELECT @VerificationState = 1,@VerificationStateDescription = N'موبایل موردنظر تایید شد'
	END TRY
	BEGIN CATCH
		DECLARE @ErrNo INT,@ErrMsg NVARCHAR(4000),@ErrSev INT,@ErrStt INT
		SELECT @ErrMsg = '(' + CONVERT(NVARCHAR(4000),ERROR_NUMBER()) + ') ' + ERROR_MESSAGE(),@ErrSev = ERROR_SEVERITY(),@ErrStt = ERROR_STATE()
		IF @@TRANCOUNT > 0 ROLLBACK TRAN
		RAISERROR(@ErrMsg,@ErrSev,@ErrStt)
	END CATCH
END