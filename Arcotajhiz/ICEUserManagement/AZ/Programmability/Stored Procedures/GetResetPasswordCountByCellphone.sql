CREATE PROCEDURE [AZ].[GetResetPasswordCountByCellphone](
	@Cellphone NVARCHAR(20)
)
AS
BEGIN
	IF NOT EXISTS(SELECT 1 FROM AZ.[User] WHERE CellPhone = @Cellphone)
	BEGIN
		SELECT -1 ResetPasswordCount
	END
	ELSE
	BEGIN
		DECLARE @cnt INT
		SET @cnt = (SELECT U.ResetPasswordCount FROM AZ.[User] U WHERE U.CellPhone = @Cellphone)
		SET @cnt = @cnt+1;

		UPDATE AZ.[User]
		SET ResetPasswordCount = @cnt
		WHERE CellPhone = @Cellphone

		SELECT ResetPasswordCount FROM AZ.[User] WHERE CellPhone = @Cellphone
	END
END