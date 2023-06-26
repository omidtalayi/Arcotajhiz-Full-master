CREATE PROCEDURE AZ.Admin_GetIndividualComplaint
(
	@ComplaintVCode AS INT
)
AS
BEGIN
	SELECT * FROM AZ.Complaint C
	WHERE C.VCode = @ComplaintVCode
END