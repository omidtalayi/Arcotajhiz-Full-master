CREATE PROCEDURE [AZ].[IdentificationIns](
	@VCode BIGINT OUTPUT,
	@Cellphone VARCHAR(20),
	@NationalCode VARCHAR(12),
	@VerificationCode VARCHAR(6) OUTPUT,
	@UserVCode INT = NULL,
	@UserPaymentTypeVCode INT = 1,
	@FromFirmPanel BIT = 0,
	@IdentificationInsStateVCode INT = 1 OUTPUT,
	@ReceiverCellphone VARCHAR(20) = NULL,
	@ReceiverCellphoneVerificationCode VARCHAR(6) OUTPUT,
	@CompanyNationalID NVARCHAR(12) = NULL,
	@IdentificationTypeVCode INT = 1,
	@IsFromSendSmsPeygiri BIT = 0,
	@HasChequeReport BIT = 0,
	@TrackingId NVARCHAR(MAX) = NULL,
	@InvitationCode NVARCHAR(10) = NULL,
	@FromApp BIT = 0
)
AS
BEGIN
	BEGIN TRAN
	BEGIN TRY
		DECLARE @SMSCodeDuration INT = 2,
				@ReportExpirationDate INT = 60 * 24,
				@ReportPrice DECIMAL(18,0),
				@TaxPrice DECIMAL(18,0),
				@PaymentAmount DECIMAL(28,0),
				@InvitationVCode BIGINT,
				@ReportIsCorrupted BIT = 0

	BEGIN
		IF ISNULL(@CompanyNationalID,'') <> '' AND LEN(@CompanyNationalID) = 11
		BEGIN
			SET @IdentificationTypeVCode = 2
		END

			SET @IdentificationInsStateVCode = 1

			IF LEN(@NationalCode) <> 10
			BEGIN
				RAISERROR('(suspicious) National Code is not 10 digits',16,126) 
			END

			IF LEN(@Cellphone) <> 11
			BEGIN
				RAISERROR('(suspicious) Cellphone is not 11 digits',16,126) 
			END

			IF @UserVCode = 2887 /*داتین*/ AND @UserPaymentTypeVCode = 1
			BEGIN
				RAISERROR('(suspicious) This userpaymenttype is not allowed by this user',16,126) 
			END

			IF (SELECT [Value] FROM AZ.ApplicationSetting WHERE [Key] = 'ShowSystemIsDisabled') = 1 
			BEGIN
				SET @ReportIsCorrupted = 1
			END

			IF @UserPaymentTypeVCode = 2 AND @UserVCode IS NOT NULL
			BEGIN
				CREATE TABLE #UserCredit(
					Credit DECIMAL(18,0)
				)

				SELECT @ReportPrice = ISNULL(SUM(ISNULL(Price,0)),0) FROM AZ.PriceList (NOLOCK) WHERE VCode = 1
				SELECT @TaxPrice = ISNULL(SUM(ISNULL(Price,0)),0) FROM AZ.PriceList (NOLOCK) WHERE VCode = 2
				SELECT @PaymentAmount = @ReportPrice + @TaxPrice

				INSERT #UserCredit EXEC [AZ].[GetUserCredit] @UserVCode = @UserVCode

				IF (SELECT ISNULL(Credit,0) FROM #UserCredit) < @PaymentAmount
				BEGIN
					SET @IdentificationInsStateVCode = 2
				END
			END

			IF @FromFirmPanel = 1
			BEGIN
				SET @VerificationCode = LEFT(CAST(ROUND(RAND() * 1000000,0) AS VARCHAR(6)) + '000000',6)
				SET @SMSCodeDuration = @ReportExpirationDate
			END
			ELSE
			BEGIN
				SET @VerificationCode = LEFT(CAST(ROUND(RAND() * 10000,0) AS VARCHAR(4)) + '0000',4)
			END

			SET @ReceiverCellphoneVerificationCode = LEFT(CAST(ROUND(RAND() * 10000,0) AS VARCHAR(4)) + '0000',4)

			IF @IdentificationInsStateVCode = 1
			BEGIN
				IF @FromFirmPanel = 0
				BEGIN
					IF @IdentificationTypeVCode = 1
					BEGIN
						SELECT @VCode = VCode FROM AZ.Identification WHERE VCode IN
						(
							SELECT TOP 1 I.VCode FROM AZ.Identification (NOLOCK) I
							--INNER JOIN AZ.CreditRiskReport (NOLOCK) CRR ON CRR.IdentificationVCode = I.VCode 
							WHERE	I.Cellphone = @Cellphone AND 
									I.NationalCode = @NationalCode AND 
									ISNULL(I.ReceiverCellphone,0) = ISNULL(@ReceiverCellphone,0) AND
									I.ReportExpirationDate > GETDATE() AND
									ISNULL(I.CompanyNationalID,0) = ISNULL(@CompanyNationalID,0) AND
									(EXISTS(SELECT 1 FROM [ICEUserManagement].AZ.[User] WHERE VCode = I.UserVCode AND UserTypeVCode = 3) OR I.UserVCode = @UserVCode)
									AND IdentificationStateVCode IN (11,12,6,13,4,17)
							ORDER BY I.VCode DESC
						)
						IF @VCode IS NULL
						BEGIN
							SELECT @VCode = VCode FROM AZ.Identification WHERE VCode IN
							(
								SELECT TOP 1 I.VCode FROM AZ.Identification (NOLOCK) I
								WHERE	I.Cellphone = @Cellphone AND 
										I.NationalCode = @NationalCode AND 
										ISNULL(I.ReceiverCellphone,0) = ISNULL(@ReceiverCellphone,0) AND
										I.ReportExpirationDate > GETDATE() AND
										(EXISTS(SELECT 1 FROM [ICEUserManagement].AZ.[User] WHERE VCode = I.UserVCode AND UserTypeVCode = 3) OR I.UserVCode = @UserVCode) AND
										I.IdentificationStateVCode = 11
								ORDER BY I.VCode DESC
							)
						END
					END
					ELSE
					BEGIN
						SELECT @VCode = VCode FROM AZ.Identification WHERE VCode IN
						(
							SELECT TOP 1 I.VCode FROM AZ.Identification (NOLOCK) I
							--INNER JOIN AZ.CreditRiskReport (NOLOCK) CRR ON CRR.IdentificationVCode = I.VCode 
							WHERE	I.Cellphone = @Cellphone AND 
									I.NationalCode = @NationalCode AND 
									I.CompanyNationalID = @CompanyNationalID AND 
									ISNULL(I.ReceiverCellphone,0) = ISNULL(@ReceiverCellphone,0) AND
									I.ReportExpirationDate > GETDATE() AND
									(EXISTS(SELECT 1 FROM [ICEUserManagement].AZ.[User] WHERE VCode = I.UserVCode AND UserTypeVCode = 3) OR I.UserVCode = @UserVCode) 
									AND IdentificationStateVCode IN (11,12,6,13,4,17)
							ORDER BY I.VCode DESC
						)
						IF @VCode IS NULL
						BEGIN
							SELECT @VCode = VCode FROM AZ.Identification WHERE VCode IN
							(
								SELECT TOP 1 I.VCode FROM AZ.Identification (NOLOCK) I
								WHERE	I.Cellphone = @Cellphone AND 
										I.NationalCode = @NationalCode AND 
										I.CompanyNationalID = @CompanyNationalID AND 
										ISNULL(I.ReceiverCellphone,0) = ISNULL(@ReceiverCellphone,0) AND
										I.ReportExpirationDate > GETDATE() AND
										(EXISTS(SELECT 1 FROM [ICEUserManagement].AZ.[User] WHERE VCode = I.UserVCode AND UserTypeVCode = 3) OR I.UserVCode = @UserVCode) AND
										I.IdentificationStateVCode = 11
								ORDER BY I.VCode DESC
							)
						END
					END
				END
				ELSE
				BEGIN
					IF @IdentificationTypeVCode = 1
					BEGIN
						SELECT @VCode = VCode FROM AZ.Identification WHERE VCode IN
						(
							SELECT TOP 1 I.VCode FROM AZ.Identification (NOLOCK) I
							WHERE	I.Cellphone = @Cellphone AND 
									I.NationalCode = @NationalCode AND 
									I.ReportExpirationDate > GETDATE() AND
									ISNULL(I.CompanyNationalID,0) = ISNULL(@CompanyNationalID,0) AND
									I.IdentificationStateVCode IN (1,2,3,4,6,11,12,13,17) AND
									I.UserVCode = @UserVCode
							ORDER BY I.VCode DESC
						)
					END
					ELSE
					BEGIN
						SELECT @VCode = VCode FROM AZ.Identification WHERE VCode IN
						(
							SELECT TOP 1 I.VCode FROM AZ.Identification (NOLOCK) I
							WHERE	I.Cellphone = @Cellphone AND 
									I.NationalCode = @NationalCode AND 
									I.CompanyNationalID = @CompanyNationalID AND 
									I.ReportExpirationDate > GETDATE() AND
									I.IdentificationStateVCode IN (1,2,3,4,6,11,12,13,17) AND
									I.UserVCode = @UserVCode
							ORDER BY I.VCode DESC
						)
					END
				END

				IF @VCode IS NULL
				BEGIN
					SELECT @InvitationVCode = VCode FROM AZ.Invitation WHERE Code = @InvitationCode

					DECLARE @GUID NVARCHAR(50) = (SELECT NEWID())
					SET @GUID = REPLACE(@GUID,'-','')

					INSERT AZ.Identification([Cellphone],[NationalCode],[VerificationCode],[ExpirationDate],[IdentificationStateVCode],[UserVCode],[ReportExpirationDate],[UserPaymentTypeVCode],[FromFirmPanel],[ReceiverCellphone],[ReceiverCellphoneVerificationCode],[ReceiverCellphoneExpirationDate],CompanyNationalID,IdentificationTypeVCode,IsFromSendSmsPeygiri,TrackingId,InvitationVCode,FromApp,[GUID],ReportIsCorrupted)
					VALUES(@Cellphone,@NationalCode,@VerificationCode,DATEADD(MINUTE,@SMSCodeDuration,GETDATE()),1,@UserVCode,DATEADD(MINUTE,@ReportExpirationDate,GETDATE()),@UserPaymentTypeVCode,@FromFirmPanel,@ReceiverCellphone,@ReceiverCellphoneVerificationCode,DATEADD(MINUTE,@SMSCodeDuration,GETDATE()),@CompanyNationalID,@IdentificationTypeVCode,@IsFromSendSmsPeygiri,@TrackingId,@InvitationVCode,@FromApp,@GUID,@ReportIsCorrupted)

					SET @VCode = SCOPE_IDENTITY()
					INSERT AZ.IdentificationHistory(IdentificationVCode,IdentificationStateVCode) VALUES(@VCode,1)

					IF @HasChequeReport = 1
					BEGIN
						INSERT AZ.IdentificationCheque(IdentificationVCode,IdentificationTypeVCode,NationalCode,CompanyNationalID,UserVCode)
						VALUES(@VCode,@IdentificationTypeVCode,@NationalCode,@CompanyNationalID,@UserVCode)
					END

					IF @FromFirmPanel = 0
					BEGIN 
						IF NOT EXISTS(SELECT 1 FROM AZ.IdentificationHistory WHERE IdentificationVCode = @VCode AND IdentificationStateVCode = 3)
						BEGIN
							UPDATE [AZ].[Identification] SET IsVerified = 1,IdentificationStateVCode = 3,LastModifiedDate = GETDATE() WHERE VCode = @VCode AND Cellphone = @Cellphone
							INSERT AZ.IdentificationHistory(IdentificationVCode,IdentificationStateVCode) VALUES(@VCode,3)
						END
					END
				END
				ELSE
				BEGIN
					UPDATE AZ.Identification 
					SET VerificationCode = @VerificationCode,
						ReceiverCellphoneVerificationCode = @ReceiverCellphoneVerificationCode,
						ReceiverCellphoneExpirationDate = DATEADD(MINUTE,@SMSCodeDuration,GETDATE()) 
					WHERE VCode = @VCode
				END
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

