CREATE PROCEDURE AZ.Admin_GetIndividualContactUs
(
	@contactUsVCode INT
)
AS
BEGIN
	SELECT * FROM AZ.ContactUs C
	WHERE C.VCode = @contactUsVCode
END