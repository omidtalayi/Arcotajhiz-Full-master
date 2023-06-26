CREATE PROCEDURE AZ.Admin_GetPartnerApi
(
	@UserVCode INT = NULL
)
AS
BEGIN
	SELECT 
		UA.VCode,
		UA.UserVCode,
		UA.ApiVCode,
		A.[Name] ,
		A.[EnumName],
		U.Username
	FROM [$(ICEUserManagement)].AZ.[UserApi] UA 
		INNER JOIN [$(ICEUserManagement)].AZ.[Api] A ON UA.ApiVCode = A.VCode
		INNER JOIN [$(ICEUserManagement)].AZ.[User] U ON UA.UserVCode = U.VCode
	WHERE UA.UserVCode = ISNULL(@UserVCode,UA.UserVCode)
END

