CREATE PROCEDURE AZ.Admin_GetPresenters
(
	@VCode INT = NULL,
	@LastName NVARCHAR(MAX) = NULL,
	@CellPhone NVARCHAR(20) = NULL
)
AS
BEGIN
	SELECT 
		P.VCode,
		P.FirstName,
		P.LastName,
		P.CellPhone,
		P.Code,
		P.EntryDate 
	FROM AZ.Presenter P
	WHERE P.VCode=ISNULL(@VCode,P.VCode) AND P.LastName = ISNULL(@LastName,P.LastName) AND P.CellPhone = ISNULL(@CellPhone,P.CellPhone) AND P.IsDeleted <> 1
END


