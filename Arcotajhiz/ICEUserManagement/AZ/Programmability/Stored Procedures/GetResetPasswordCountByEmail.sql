CREATE PROCEDURE [AZ].[GetResetPasswordCountByEmail](
	@Email NVARCHAR(180)
)
AS
BEGIN
	IF NOT EXISTS(SELECT 1 FROM AZ.[User] WHERE Email = @Email)
	BEGIN
		SELECT -1 ResetPasswordCount
	END
	ELSE
	BEGIN
		DECLARE @cnt INT
		SET @cnt = (SELECT U.ResetPasswordCount FROM AZ.[User] U WHERE U.Email = @Email AND U.IsApproved = 1)
		SET @cnt = @cnt+1;

		UPDATE AZ.[User]
		SET ResetPasswordCount = @cnt
		WHERE Email = @Email

		SELECT ResetPasswordCount FROM AZ.[User] WHERE Email = @Email
	END
END