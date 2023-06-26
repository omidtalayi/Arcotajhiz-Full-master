CREATE PROCEDURE AZ.Admin_GetReportPriceShare
(
	@UserVCode INT = NULL
)
AS
BEGIN
	SELECT * FROM AZ.ReportPriceShare R 
	WHERE R.UserVCode = ISNULL(@UserVCode,R.UserVCode)
END

