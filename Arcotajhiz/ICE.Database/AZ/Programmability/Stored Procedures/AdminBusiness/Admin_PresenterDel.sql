CREATE PROCEDURE AZ.Admin_PresenterDel
(
	@VCode INT 
)
AS
BEGIN
	UPDATE AZ.Presenter
	SET IsDeleted = 1 , LastModifiedDate = GETDATE()
	WHERE VCode = @VCode
END