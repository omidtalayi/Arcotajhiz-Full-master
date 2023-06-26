CREATE PROCEDURE AZ.Admin_PagesDel(
	@VCode INT = NULL
)
AS
BEGIN
	UPDATE AZ.Pages
	SET	IsDeleted = 1 
	WHERE VCode = @VCode
END