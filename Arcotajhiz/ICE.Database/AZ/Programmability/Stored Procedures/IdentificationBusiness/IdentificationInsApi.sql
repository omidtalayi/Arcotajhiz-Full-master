CREATE PROCEDURE [AZ].[IdentificationInsApi](
	@VCode				BIGINT OUTPUT,
	@State				SMALLINT = 1 OUTPUT,
	@Cellphone			VARCHAR(20),
	@NationalCode		VARCHAR(12),
	@VerificationCode	VARCHAR(6) OUTPUT,
	@UserVCode			INT,
	@IP					VARCHAR(50),
    @SaleAmount			DECIMAL(18,0),
    @SaleRefID			VARCHAR(100)
)
AS
BEGIN
	BEGIN TRAN
	BEGIN TRY
		SET @State = 1
		DECLARE @SMSCodeDuration INT = 60 * 24,
				@OnlinePaymentVCode BIGINT,
				@BankVCode INT,
				@BankPortalVCode INT,
				@ReportExpirationDate INT = 60 * 24,
				@ReportIsCorrupted BIT = 0

		--IF LEN(@NationalCode) <> 10
		--BEGIN
		--	RAISERROR('(suspicious) National Code is not 10 digits',16,126) 
		--END

		--IF LEN(@Cellphone) <> 11
		--BEGIN
		--	RAISERROR('(suspicious) Cellphone is not 11 digits',16,126) 
		--END
		IF EXISTS (SELECT 1 FROM AZ.IdentificationPayment WHERE @SaleRefID = SaleRefID)
		BEGIN
			SET @State = 0
		END
		ELSE
		BEGIN
			IF (SELECT [Value] FROM AZ.ApplicationSetting WHERE [Key] = 'ShowSystemIsDisabled') = 1 
			BEGIN
				SET @ReportIsCorrupted = 1
			END

			SELECT @BankVCode = VCode FROM AZ.Bank WHERE UserVCode = @UserVCode
			SELECT @BankPortalVCode = VCode FROM AZ.BankPortal WHERE BankVCode = @BankVCode

			SET @VerificationCode = LEFT(CAST(ROUND(RAND() * 10000,0) AS VARCHAR(4)) + '0000',4)

			IF EXISTS(SELECT 1 FROM AZ.Identification WHERE NationalCode = @NationalCode AND Cellphone = @Cellphone AND UserVCode = @UserVCode AND ReportExpirationDate >= GETDATE())
			BEGIN
				SELECT @VCode = VCode,@VerificationCode = VerificationCode FROM AZ.Identification WHERE NationalCode = @NationalCode AND Cellphone = @Cellphone AND UserVCode = @UserVCode AND ReportExpirationDate >= GETDATE()
			END
			ELSE
			BEGIN
				DECLARE @GUID NVARCHAR(50) = (SELECT NEWID())
				SET @GUID = REPLACE(@GUID,'-','')

				INSERT AZ.Identification([Cellphone],[NationalCode],[VerificationCode],[ExpirationDate],[HasShahkarIdentified],[IsVerified],[IdentificationStateVCode],[UserVCode],[ReportExpirationDate],[GUID],ReportIsCorrupted) 
				VALUES(@Cellphone,@NationalCode,@VerificationCode,DATEADD(MINUTE,@SMSCodeDuration,GETDATE()),0,1,3,@UserVCode,DATEADD(MINUTE,@ReportExpirationDate,GETDATE()),@GUID,@ReportIsCorrupted)

				SET @VCode = SCOPE_IDENTITY()
				INSERT AZ.IdentificationHistory(IdentificationVCode,IdentificationStateVCode) VALUES(@VCode,1)
				--INSERT AZ.IdentificationHistory(IdentificationVCode,IdentificationStateVCode) VALUES(@VCode,2)
				INSERT AZ.IdentificationHistory(IdentificationVCode,IdentificationStateVCode) VALUES(@VCode,3)

				IF @SaleAmount IS NOT NULL AND @SaleRefID IS NOT NULL
				BEGIN
					IF NOT EXISTS(SELECT 1 FROM AZ.IdentificationPayment WHERE IdentificationVCode = @VCode)
					BEGIN
						INSERT AZ.IdentificationPayment(IdentificationVCode,SaleAmount,SaleRefID)
						VALUES(@VCode,@SaleAmount,@SaleRefID)

						UPDATE AZ.[Identification] SET IdentificationStateVCode = 11 WHERE VCode = @VCode
						INSERT AZ.IdentificationHistory(IdentificationVCode,IdentificationStateVCode) VALUES(@VCode,11)
					END
				END
			END

			--IF @UserVCode IS NOT NULL
			--BEGIN
			--	UPDATE AZ.[Identification] SET IdentificationTypeVCode = 2 WHERE VCode = @VCode
			--END

			IF @VCode IS NULL
			BEGIN
				SELECT @VCode = VCode,@VerificationCode = VerificationCode FROM AZ.Identification WHERE Cellphone = @Cellphone AND NationalCode = @NationalCode
			END
		END

		COMMIT TRAN
	END TRY
		BEGIN CATCH
		DECLARE @ErrNo INT,@ErrMsg NVARCHAR(4000),@ErrSev INT,@ErrStt INT
		SELECT @ErrMsg = '(' + CONVERT(NVARCHAR(4000),ERROR_NUMBER()) + ') ' + ERROR_MESSAGE(),@ErrSev = ERROR_SEVERITY(),@ErrStt = ERROR_STATE()
		IF @@TRANCOUNT > 0 ROLLBACK TRAN
		RAISERROR(@ErrMsg,@ErrSev,@ErrStt)
	END CATCH
END
GO

