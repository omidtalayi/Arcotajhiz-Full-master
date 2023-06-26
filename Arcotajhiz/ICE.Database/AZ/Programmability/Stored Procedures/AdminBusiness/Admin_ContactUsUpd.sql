CREATE PROCEDURE AZ.Admin_ContactUsUpd
(
	@userVCode INT,
	@contactUsVCode INT ,
	@Description NVARCHAR(MAX) = NULL
)
AS
BEGIN
	UPDATE AZ.ContactUs
	SET	AdminDescription = ISNULL(@Description,AdminDescription),
		LastModifiedUserVCode = ISNULL(@userVCode,LastModifiedUserVCode),
		LastModifiedDate = GETDATE()
	WHERE VCode = @contactUsVCode
END
