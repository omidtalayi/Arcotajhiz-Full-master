﻿CREATE PROCEDURE [AZ].[GetPartners]
AS
BEGIN
	SELECT [User].VCode UserVCode,
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
			NULL ApiExpirationDate,
			NULL TrackingSendLink,
			[User].IsApiFree,
			[User].SelfOtp,
			[User].AvailableUserPaymentTypeVCode
	FROM AZ.[User] (NOLOCK) WHERE UserTypeVCode = 1 AND VCode NOT IN (1,4) AND IsApproved = 1
END