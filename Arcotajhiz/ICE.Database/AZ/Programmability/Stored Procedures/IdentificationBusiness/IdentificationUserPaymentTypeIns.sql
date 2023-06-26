CREATE PROCEDURE [AZ].[IdentificationUserPaymentTypeIns](
	@IdentificationVCode BIGINT,
	@UserPaymentTypeVCode SMALLINT,
    @IdentificationUserPaymentTypeInsState SMALLINT OUTPUT,
    @IdentificationUserPaymentTypeInsStateDescription VARCHAR(500) OUTPUT
)
AS
BEGIN
	BEGIN TRY
		IF NOT EXISTS(SELECT 1 FROM [AZ].[Identification] WHERE VCode = @IdentificationVCode) 
		BEGIN 
			SELECT @IdentificationUserPaymentTypeInsState = 2,@IdentificationUserPaymentTypeInsStateDescription = 'گزارش مورد نظر یافت نشد'
			RETURN
		END		
		IF EXISTS(SELECT 1 FROM [AZ].[Identification] WHERE VCode = @IdentificationVCode AND ReportExpirationDate < GETDATE()) 
		BEGIN 
			SELECT @IdentificationUserPaymentTypeInsState = 3,@IdentificationUserPaymentTypeInsStateDescription = 'گزارش مورد نظر منقضی شده است'
			RETURN
		END

		UPDATE [AZ].[Identification] SET UserPaymentTypeVCode = @UserPaymentTypeVCode,LastModifiedDate = GETDATE() WHERE VCode = @IdentificationVCode 
		SELECT @IdentificationUserPaymentTypeInsState = 1,@IdentificationUserPaymentTypeInsStateDescription = CASE WHEN @UserPaymentTypeVCode = 1 THEN 'عملیات با موفقیت انجام شد منتظر تایید و پرداخت شخص بمانید' ELSE 'عملیات با موفقیت انجام شد منتظر تایید شخص بمانید'  END
	END TRY
	BEGIN CATCH
		DECLARE @ErrNo INT,@ErrMsg NVARCHAR(4000),@ErrSev INT,@ErrStt INT
		SELECT @ErrMsg = '(' + CONVERT(NVARCHAR(4000),ERROR_NUMBER()) + ') ' + ERROR_MESSAGE(),@ErrSev = ERROR_SEVERITY(),@ErrStt = ERROR_STATE()
		IF @@TRANCOUNT > 0 ROLLBACK TRAN
		RAISERROR(@ErrMsg,@ErrSev,@ErrStt)
	END CATCH
END