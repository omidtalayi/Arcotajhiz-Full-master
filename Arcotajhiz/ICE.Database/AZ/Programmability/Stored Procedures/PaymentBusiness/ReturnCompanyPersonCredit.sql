CREATE PROCEDURE [AZ].[ReturnCompanyPersonCredit](
	@UserVCode INT,
	@CompanyPersonVCode BIGINT
)
AS
BEGIN
	BEGIN TRAN
    BEGIN TRY
		DECLARE @ReturnAmount DECIMAL(18,0)
		IF EXISTS(SELECT 1 FROM [$(ICEUserManagement)].AZ.[User] WHERE VCode = @UserVCode AND UserTypeVCode = 2)
		BEGIN
			IF EXISTS(SELECT 1 FROM AZ.CompanyPerson WHERE VCode = @CompanyPersonVCode AND CompanyPersonStateVCode = 2)
			BEGIN
				SELECT @ReturnAmount = Bed FROM AZ.Accounting WHERE CompanyPersonVCode = @CompanyPersonVCode AND AccountingTypeVCode = 2
				IF @ReturnAmount > 0 AND NOT EXISTS(SELECT 1 FROM AZ.Accounting WHERE CompanyPersonVCode = @CompanyPersonVCode AND AccountingTypeVCode = 4)
				BEGIN
					EXEC AZ.AccountingIns @UserVCode = @UserVCode,@AccountingTypeVCode = 4,@Bed = 0,@Bes = @ReturnAmount,@CompanyPersonVCode = @CompanyPersonVCode

					IF NOT EXISTS(SELECT 1 FROM AZ.CompanyPersonHistory WHERE CompanyPersonVCode = @CompanyPersonVCode AND CompanyPersonStateVCode = 7)
					BEGIN
						UPDATE AZ.CompanyPerson SET CompanyPersonStateVCode = 7,LastModifiedDate = GETDATE() WHERE VCode = @CompanyPersonVCode
						INSERT AZ.CompanyPersonHistory(CompanyPersonVCode,CompanyPersonStateVCode) VALUES(@CompanyPersonVCode,7)
					END 

					IF EXISTS(SELECT 1 FROM AZ.CompanyPersonHistory WHERE CompanyPersonVCode = @CompanyPersonVCode AND CompanyPersonStateVCode = 2)
					BEGIN
						DELETE AZ.CompanyPersonHistory WHERE CompanyPersonVCode = @CompanyPersonVCode AND CompanyPersonStateVCode = 2
					END
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
