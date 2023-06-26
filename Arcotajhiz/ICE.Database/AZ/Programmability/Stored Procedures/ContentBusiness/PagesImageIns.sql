CREATE PROCEDURE [AZ].[PagesImageIns](
	@Name AS NVARCHAR(500),
	@PagesVCode INT
)
AS
BEGIN
	INSERT AZ.PagesImage([Name],PagesVCode) VALUES(@Name,@PagesVCode)
END