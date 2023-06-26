CREATE PROCEDURE [AZ].[GetUserVCodeByCellphone](
	@Cellphone NVARCHAR(20)
)
AS
BEGIN
	SELECT ISNULL(VCode,0) FROM AZ.[User] WHERE Cellphone = @Cellphone
END