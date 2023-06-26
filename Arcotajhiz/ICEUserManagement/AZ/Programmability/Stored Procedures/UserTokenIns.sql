CREATE PROCEDURE [AZ].[UserTokenIns](
	@UserVCode				BIGINT OUTPUT,
	@Username				NVARCHAR(100),
	@Token					VARCHAR(1000),
	@TokenExpirationDate	DATETIME,
	@IP						VARCHAR(20),
	@DeviceTypeVCode		SMALLINT,
	@SecretCode				VARCHAR(500),
	@DeviceId				NVARCHAR(200) = NULL
)
AS
BEGIN
	BEGIN TRY 
		IF EXISTS(SELECT 1 FROM AZ.[User] WHERE Username = @Username AND UserTypeVCode IN (1,2,3))
		BEGIN
			SELECT @UserVCode = VCode FROM AZ.[User] (NOLOCK) WHERE Username = @Username

			IF @UserVCode IS NOT NULL
			BEGIN
				MERGE AZ.UserToken USING (SELECT @UserVCode,@Token,@TokenExpirationDate,@IP,@DeviceTypeVCode,@SecretCode,@DeviceId) 
					AS Source (UserVCode,Token,TokenExpirationDate,[IP],DeviceTypeVCode,SecretCode,DeviceId)
				ON (UserToken.UserVCode = Source.UserVCode AND ISNULL(UserToken.DeviceId,0) = ISNULL(Source.DeviceId,0))
				WHEN MATCHED THEN
					UPDATE SET
						[Token] = Source.[Token],
						[TokenExpirationDate] = Source.[TokenExpirationDate],
						[IP] = Source.[IP],
						[DeviceTypeVCode] = Source.[DeviceTypeVCode],
						[SecretCode] = Source.[SecretCode],
						[DeviceId] = Source.[DeviceId]
				WHEN NOT MATCHED THEN
					INSERT (UserVCode,Token,TokenExpirationDate,[IP],DeviceTypeVCode,SecretCode,DeviceId) 
					VALUES (Source.[UserVCode],Source.[Token],Source.[TokenExpirationDate],Source.[IP],Source.[DeviceTypeVCode],Source.[SecretCode],Source.[DeviceId]);
			END
		END
		ELSE
		BEGIN
			RAISERROR('Users usertype can not get token',16,126)
		END
	END TRY
	BEGIN CATCH
		DECLARE @ErrNo INT,@ErrMsg NVARCHAR(4000),@ErrSev INT,@ErrStt INT
		SELECT @ErrMsg = '(' + CONVERT(NVARCHAR(4000),ERROR_NUMBER()) + ') ' + ERROR_MESSAGE(),@ErrSev = ERROR_SEVERITY(),@ErrStt = ERROR_STATE()
		IF @@TRANCOUNT > 0 ROLLBACK TRAN
		RAISERROR(@ErrMsg,@ErrSev,@ErrStt)
	END CATCH
END
