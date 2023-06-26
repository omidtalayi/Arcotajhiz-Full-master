CREATE PROCEDURE AZ.Admin_ComplaintUpd
(
	@userVCode INT,
	@complaintVCode INT,
	@Description NVARCHAR(MAX) = NULL
)
AS
BEGIN
	UPDATE AZ.Complaint 
	SET AdminDescription = ISNULL(@Description,AdminDescription),
		LastModifiedUserVCode = ISNULL(@userVCode,LastModifiedUserVCode),
		LastModifiedDate = GETDATE()
	WHERE VCode = @complaintVCode
END
