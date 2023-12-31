﻿CREATE PROCEDURE [AZ].[GetTrackingUsers]
AS
BEGIN
	SELECT	[User].VCode UserVCode,
			[User].Username,
			[User].CellPhone,
			[User].Email,
			[User].Password,
			[User].PasswordSalt,
			[User].IsApproved,
			[User].IsLock,
			[User].LastLoginDate,
			[User].LastFailedAttemptDate,
			[User].FailedAttemptCount,
			[User].EntryDate,
			[User].LastModifiedDate,
			[User].SendLinkUrlSms,
			[User].IsSubscribed,
			[User].TokenExpirationTime,
			[User].UserTypeVCode,
			[User].UserPaymentTypeVCode,
			[User].[Name],
			[User].[FirmRegistrationVCode],
			[User].[DocApproved],
			[User].[TrackingCode],
			[User].[PartnerLogoUrl],
			[User].VerificationCode,
			[User].VerificationCodeTryCount,
			[User].VerificationExpireDate,
			NULL HasConfirmedRequest,
			NULL PresenterPaymentHasPaid,
			NULL Token,
			NULL TokenExpirationDate,
			NULL SecretCode,
			NULL Credit,
			NULL API,
			NULL WebHook,
			NULL [Role],
			NULL TrackingSendLink,
			[User].IsApiFree,
			[User].SelfOtp,
			[User].AvailableUserPaymentTypeVCode,
			[User].ApiExpirationDate
	FROM AZ.[User] (NOLOCK) WHERE UserTypeVCode = 3 AND VCode NOT IN (1,4)
END
GO

