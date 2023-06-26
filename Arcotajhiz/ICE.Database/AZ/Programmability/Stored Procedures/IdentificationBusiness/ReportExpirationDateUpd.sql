CREATE PROCEDURE AZ.ReportExpirationDateUpd
(
	@VCode INT,
	@Hour INT = 24
)
AS
BEGIN
	
	UPDATE AZ.Identification
	SET ReportExpirationDate = DATEADD(HOUR, @Hour, GETDATE())
	WHERE VCode = @VCode

END
GO

