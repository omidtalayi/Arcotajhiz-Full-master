CREATE PROCEDURE [AZ].[UpdateUserApis](
	@UserVCode BIGINT,
	@APis XML
)
AS
BEGIN
	SELECT Field.value('@AV','INT') ApiVCode
	INTO #Apis FROM @Apis.nodes('A') Details(Field)

	INSERT AZ.UserApi(UserVCode,ApiVCode)
	SELECT @UserVCode,ApiVCode FROM #Apis A
	WHERE NOT EXISTS (SELECT 1 FROM AZ.UserApi UA WHERE UA.APiVCode = A.ApiVCode AND UA.UserVCode = @UserVCode)

	DROP TABLE #Apis
END