CREATE PROCEDURE [AZ].[PayCompanyPersonByCredit](
	@UserVCode INT,
	@CompanyPersonVCode BIGINT,
	@PayCompanyPersonByCreditState SMALLINT OUTPUT
)
AS
BEGIN
	BEGIN TRAN
    BEGIN TRY
		SET @PayCompanyPersonByCreditState = 0
		DECLARE @ReportPrice DECIMAL(18,0),
				@TaxPrice DECIMAL(18,0),
				@TotalPrice	DECIMAL(18,0),
				@PaymentAmount DECIMAL(28,0),
				@SourceSharePercentage DECIMAL(18,10),
				@SourceShare DECIMAL(18,0),
				@CompanyPersonTypeVCode INT

		CREATE TABLE #UserCredit( 
			Credit DECIMAL(18,0)
		)

		INSERT #UserCredit EXEC [AZ].[GetUserCredit] @UserVCode = @UserVCode

		SELECT @CompanyPersonTypeVCode = CompanyPersonTypeVCode FROM AZ.CompanyPerson WHERE VCode = @CompanyPersonVCode
		
		IF @CompanyPersonTypeVCode = 1
		BEGIN
			SELECT @ReportPrice = 300000
			SELECT @TaxPrice = 0
		END
		IF @CompanyPersonTypeVCode = 2
		BEGIN
			SELECT @ReportPrice = 300000
			SELECT @TaxPrice = 0
		END

		SELECT @PaymentAmount = @ReportPrice + @TaxPrice

		IF EXISTS(SELECT 1 FROM AZ.CompanyPerson WHERE VCode = @CompanyPersonVCode)
		BEGIN
			IF (SELECT ISNULL(Credit,0) FROM #UserCredit) < @PaymentAmount
			BEGIN
				SET @PayCompanyPersonByCreditState = 2	
			END
			ELSE
			BEGIN
				SET @PayCompanyPersonByCreditState = 1
				EXEC AZ.AccountingIns @UserVCode = @UserVCode,@AccountingTypeVCode = 2,@Bed = @PaymentAmount,@Bes = 0,@CompanyPersonVCode = @CompanyPersonVCode

				IF NOT EXISTS(SELECT 1 FROM AZ.CompanyPersonHistory WHERE CompanyPersonVCode = @CompanyPersonVCode AND CompanyPersonStateVCode = 2)
				BEGIN
					UPDATE AZ.[CompanyPerson] SET CompanyPersonStateVCode	= 2 WHERE VCode = @CompanyPersonVCode
					INSERT AZ.CompanyPersonHistory(CompanyPersonVCode,CompanyPersonStateVCode) VALUES(@CompanyPersonVCode,2)
				END
			END
		END

		DROP TABLE #UserCredit

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


