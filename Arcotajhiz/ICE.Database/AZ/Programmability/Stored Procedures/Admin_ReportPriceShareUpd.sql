CREATE PROCEDURE AZ.Admin_ReportPriceShareUpd
(
	@UserVCode INT,
	@SourcePrice INT,
	@IceShare INT,
	@PartnerShare INT
)
AS
BEGIN
	UPDATE AZ.ReportPriceShare
	SET SourceShare = @SourcePrice,
		IceShare = @IceShare,
		PartnerShare = @PartnerShare		
	WHERE UserVCode = @UserVCode
END