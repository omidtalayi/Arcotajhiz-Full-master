CREATE PROCEDURE AZ.Admin_ApplicationSettingUpd
(
	@VCode INT,
	@Value NVARCHAR(MAX) 
)
AS
BEGIN
	UPDATE az.ApplicationSetting 
	SET [Value] = ISNULL(@Value,[Value])
	WHERE VCode = @VCode
END