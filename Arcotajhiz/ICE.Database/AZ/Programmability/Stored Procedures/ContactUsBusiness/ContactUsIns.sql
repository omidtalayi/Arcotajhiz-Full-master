CREATE PROCEDURE [AZ].[ContactUsIns](
	@VCode BIGINT OUTPUT,
    @Title NVARCHAR(100),
    @FullName NVARCHAR(50),
    @Email NVARCHAR(100),
    @CellPhone VARCHAR(12),
    @Description VARCHAR(MAX)
)
AS
BEGIN
	BEGIN TRAN
	BEGIN TRY

		INSERT AZ.ContactUs(Title,FullName,Email,CellPhone,[Description])
		VALUES(@Title,@FullName,@Email,@CellPhone,@Description)

		COMMIT TRAN
	END TRY
		BEGIN CATCH
		DECLARE @ErrNo INT,@ErrMsg NVARCHAR(4000),@ErrSev INT,@ErrStt INT
		SELECT @ErrMsg = '(' + CONVERT(NVARCHAR(4000),ERROR_NUMBER()) + ') ' + ERROR_MESSAGE(),@ErrSev = ERROR_SEVERITY(),@ErrStt = ERROR_STATE()
		IF @@TRANCOUNT > 0 ROLLBACK TRAN
		RAISERROR(@ErrMsg,@ErrSev,@ErrStt)
	END CATCH
END
