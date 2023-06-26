CREATE PROCEDURE [AZ].[IsFirmApiExpired](
	@UserVCode BIGINT
)
AS
BEGIN
	DECLARE @AccountingVCode BIGINT,
			@IsFirmApiExpired BIT = 1,
			@AccountingEntryJDate CHAR(8),
			@FirmApiExpireJDate CHAR(8),
			@Today CHAR(8)

	SET @AccountingVCode = (SELECT TOP 1 VCode FROM AZ.Accounting (NOLOCK) WHERE UserVCode = @UserVCode AND AccountingTypeVCode = 5 ORDER BY VCode DESC)
	SET @Today = AZ.ConvertToJalaliDate(GETDATE())
	IF @AccountingVCode IS NOT NULL
	BEGIN
		SELECT @AccountingEntryJDate = AZ.ConvertToJalaliDate(EntryDate) FROM AZ.Accounting (NOLOCK) WHERE VCode = @AccountingVCode
		SET @FirmApiExpireJDate = (SELECT [AZ].[JAddMonth](@AccountingEntryJDate,1)) 
		IF @Today <= @FirmApiExpireJDate
		BEGIN
			SET @IsFirmApiExpired = 0
		END
	END

	SELECT @IsFirmApiExpired IsFirmApiExpired,@AccountingEntryJDate FirmApiStartJDate,@FirmApiExpireJDate FirmApiExpireJDate
END	