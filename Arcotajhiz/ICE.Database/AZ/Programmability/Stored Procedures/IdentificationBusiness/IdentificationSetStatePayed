CREATE PROCEDURE [AZ].[ExpirationDateUpd](
	@VCode BIGINT,
	@ExpirationDate DATETIME
)
AS
BEGIN
	UPDATE AZ.Identification SET ExpirationDate = @ExpirationDate WHERE VCode = @VCode
END