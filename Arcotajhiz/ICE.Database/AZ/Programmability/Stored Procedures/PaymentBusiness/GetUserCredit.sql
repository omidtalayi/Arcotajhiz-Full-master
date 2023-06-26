CREATE PROCEDURE [AZ].[GetUserCredit](
	@UserVCode INT
)
AS
BEGIN
	IF EXISTS(
		SELECT 1 FROM AZ.Accounting (NOLOCK) WHERE UserVCode = @UserVCode 
		AND CS <> BINARY_CHECKSUM(VCode,UserVCode,IdentificationVCode,AccountingTypeVCode,Bed,Bes,OnlinePaymentVCode,[Description],ExpirationDate,EntryDate)
		)
	BEGIN
		SELECT 0
	END
	ELSE
	BEGIN
		SELECT ISNULL(SUM(Bes),0) - ISNULL(SUM(Bed),0) FROM AZ.Accounting (NOLOCK)
		WHERE UserVCode = @UserVCode 
			AND CS = BINARY_CHECKSUM(VCode,UserVCode,IdentificationVCode,AccountingTypeVCode,Bed,Bes,OnlinePaymentVCode,[Description],ExpirationDate,EntryDate)
	END
END
