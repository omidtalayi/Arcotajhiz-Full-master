CREATE PROCEDURE [AZ].[GetIdentificationLinkAndExpirationDateFirmPanel](
	@NationalCode VARCHAR(20),
	@Cellphone VARCHAR(20),
	@UserVCode BIGINT = NULL,
	@CompanyNationalID VARCHAR(20) = NULL
)
AS
BEGIN
	DECLARE @Now DATETIME = GETDATE()

	SELECT TOP 1 ReportExpirationDate,ReportLink,I.VCode IdentificationVCode,I.IdentificationStateVCode FROM AZ.Identification (NOLOCK) I
	WHERE I.Cellphone = @Cellphone AND 
		I.NationalCode = @NationalCode AND 
		I.ReportExpirationDate > GETDATE() AND
		I.UserVCode = @UserVCode AND
		ISNULL(I.CompanyNationalID,0) = ISNULL(@CompanyNationalID,ISNULL(@CompanyNationalID,0))
	ORDER BY I.VCode DESC
END