CREATE PROCEDURE AZ.GetSetting(@Key VARCHAR(100)) AS
BEGIN
	DECLARE @VCode INT
	SELECT @VCode = VCode FROM AZ.ApplicationSetting (NOLOCK) WHERE [Key] = @Key
	SELECT [Value] FROM AZ.ApplicationSetting (NOLOCK) WHERE VCode = @VCode
END