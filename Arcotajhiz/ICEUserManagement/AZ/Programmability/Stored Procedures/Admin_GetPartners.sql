CREATE PROCEDURE [AZ].[Admin_GetPartners]
(
	@VCode INT = NULL
)
AS
BEGIN
	SELECT 
		U.VCode,
		U.Username ,
		U.IsApproved ,
		U.IsLock ,
		U.Email ,
		U.CellPhone ,
		U.IsSubscribed ,
		U.UserTypeVCode,
		U.TokenExpirationTime TokenExpirationDate,
		U.UserPaymentTypeVCode,
		U.PartnerLogoUrl
	FROM AZ.[User] U 
	WHERE U.UserTypeVCode = 1 AND U.VCode = ISNULL(@VCode,U.VCode)
END
GO

