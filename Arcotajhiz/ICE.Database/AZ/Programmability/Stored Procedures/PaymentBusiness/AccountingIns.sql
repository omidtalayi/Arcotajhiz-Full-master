CREATE PROCEDURE [AZ].[AccountingIns](
	@UserVCode					INT,
	@AccountingTypeVCode		INT,
	@Bed						DECIMAL,
	@Bes						DECIMAL,
	@OnlinePaymentVCode			BIGINT = NULL,
	@IdentificationVCode		BIGINT = NULL,
	@RequestRegistrationVCode	BIGINT = NULL,
	@GroupOfPersonsVCode		BIGINT = NULL,
	@JaamVCode					BIGINT = NULL,
	@CompanyPersonVCode			BIGINT = NULL
)
AS
BEGIN
	BEGIN TRAN
	BEGIN TRY
		DECLARE @CS BIGINT,
		@VCode BIGINT
		IF NOT EXISTS(SELECT 1 From AZ.Accounting WHERE OnlinePaymentVCode = @OnlinePaymentVCode) 
		BEGIN
			INSERT AZ.Accounting(UserVCode,AccountingTypeVCode,Bed,Bes,OnlinePaymentVCode,IdentificationVCode,RequestRegistrationVCode,GroupOfPersonsVCode,JaamVCode,CompanyPersonVCode)
			VALUES(@UserVCode,@AccountingTypeVCode,@Bed,@Bes,@OnlinePaymentVCode,@IdentificationVCode,@RequestRegistrationVCode,@GroupOfPersonsVCode,@JaamVCode,@CompanyPersonVCode)

			SET @VCode = SCOPE_IDENTITY()

			UPDATE AZ.Accounting 
			SET CS = BINARY_CHECKSUM(VCode,UserVCode,IdentificationVCode,AccountingTypeVCode,Bed,Bes,OnlinePaymentVCode,[Description],ExpirationDate,EntryDate) 
			WHERE VCode = @VCode

			COMMIT TRAN
		END
	END TRY
	BEGIN CATCH
		DECLARE @ErrNo INT,@ErrMsg NVARCHAR(4000),@ErrSev INT,@ErrStt INT
		SELECT @ErrMsg = '(' + CONVERT(NVARCHAR(4000),ERROR_NUMBER()) + ') ' + ERROR_MESSAGE(),@ErrSev = ERROR_SEVERITY(),@ErrStt = ERROR_STATE()
		IF @@TRANCOUNT > 0 ROLLBACK TRAN
		RAISERROR(@ErrMsg,@ErrSev,@ErrStt)
	END CATCH
END
GO