CREATE PROCEDURE AZ.Admin_GetApi
(
	@VCode INT = NULL
)
AS
BEGIN
	SELECT * FROM [$(ICEUserManagement)].AZ.Api A
	WHERE A.VCode = ISNULL(@VCode,A.VCode)
END
