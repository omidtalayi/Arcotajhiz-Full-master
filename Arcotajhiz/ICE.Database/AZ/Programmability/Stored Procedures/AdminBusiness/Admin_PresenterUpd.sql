CREATE PROCEDURE AZ.Admin_PresenterUpd
(
	@VCode INT,
	@FirstName NVARCHAR(MAX) = NULL,
	@LastName NVARCHAR(MAX) = NULL,
	@Cellphone NVARCHAR(20) = NULL,
	@Code NVARCHAR(MAX) = NULL
)
AS
BEGIN
	UPDATE AZ.Presenter 
		SET FirstName = ISNULL(@FirstName,FirstName),
			LastName = ISNULL(@LastName,LastName),
			CellPhone = ISNULL(@Cellphone,CellPhone),
			Code = ISNULL(@Code,Code),
			LastModifiedDate = GETDATE()
	WHERE VCode = @VCode
END
