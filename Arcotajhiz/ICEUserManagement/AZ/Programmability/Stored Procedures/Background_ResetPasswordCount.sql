CREATE PROCEDURE AZ.Background_ResetPasswordCount
AS
BEGIN
	UPDATE AZ.[User] SET ResetPasswordCount = 0
END