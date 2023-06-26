CREATE PROCEDURE [AZ].[ComplaintIns](
	@VCode BIGINT OUTPUT,
    @Title NVARCHAR(200),
    @FirstName NVARCHAR(100),
	@LastName NVARCHAR(100),
	@NationalCode NVARCHAR(20),
    @Email NVARCHAR(100),
    @CellPhone VARCHAR(12),
    @Description VARCHAR(MAX)
)
AS
BEGIN
	BEGIN TRAN
	BEGIN TRY

		INSERT AZ.Complaint(Title,FirstName,LastName,NationalCode,Email,CellPhone,[Description])
		VALUES(@Title,@FirstName,@LastName,@NationalCode,@Email,@CellPhone,@Description)

		SET @VCode = SCOPE_IDENTITY()

		COMMIT TRAN
	END TRY
		BEGIN CATCH
		DECLARE @ErrNo INT,@ErrMsg NVARCHAR(4000),@ErrSev INT,@ErrStt INT
		SELECT @ErrMsg = '(' + CONVERT(NVARCHAR(4000),ERROR_NUMBER()) + ') ' + ERROR_MESSAGE(),@ErrSev = ERROR_SEVERITY(),@ErrStt = ERROR_STATE()
		IF @@TRANCOUNT > 0 ROLLBACK TRAN
		RAISERROR(@ErrMsg,@ErrSev,@ErrStt)
	END CATCH
END
