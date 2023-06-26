CREATE PROCEDURE [AZ].[PayGroupOfPersonsByCredit](
	@UserVCode INT,
	@GroupOfPersonsVCode BIGINT,
	@PayGroupOfPersonsByCreditState SMALLINT OUTPUT
)
AS
BEGIN
	BEGIN TRAN
    BEGIN TRY
		SET @PayGroupOfPersonsByCreditState = 0
		DECLARE @ReportPrice DECIMAL(18,0),
				@TaxPrice DECIMAL(18,0),
				@TotalPrice	DECIMAL(18,0),
				@PaymentAmount DECIMAL(28,0),
				@SourceSharePercentage DECIMAL(18,10),
				@SourceShare DECIMAL(18,0),
				@GroupOfPersonsTypeVCode INT

		CREATE TABLE #UserCredit( 
			Credit DECIMAL(18,0)
		)

		INSERT #UserCredit EXEC [AZ].[GetUserCredit] @UserVCode = @UserVCode

		SELECT @GroupOfPersonsTypeVCode = GroupOfPersonsTypeVCode FROM AZ.GroupOfPersons WHERE VCode = @GroupOfPersonsVCode
		
		IF @GroupOfPersonsTypeVCode = 1
		BEGIN
			SELECT @ReportPrice = 300000
			SELECT @TaxPrice = 0
		END
		IF @GroupOfPersonsTypeVCode = 2
		BEGIN
			SELECT @ReportPrice = 300000
			SELECT @TaxPrice = 0
		END

		SELECT @PaymentAmount = @ReportPrice + @TaxPrice

		IF EXISTS(SELECT 1 FROM AZ.GroupOfPersons WHERE VCode = @GroupOfPersonsVCode)
		BEGIN
			IF (SELECT ISNULL(Credit,0) FROM #UserCredit) < @PaymentAmount
			BEGIN
				SET @PayGroupOfPersonsByCreditState = 2	
			END
			ELSE
			BEGIN
				SET @PayGroupOfPersonsByCreditState = 1
				EXEC AZ.AccountingIns @UserVCode = @UserVCode,@AccountingTypeVCode = 2,@Bed = @PaymentAmount,@Bes = 0,@GroupOfPersonsVCode = @GroupOfPersonsVCode

				IF NOT EXISTS(SELECT 1 FROM AZ.GroupOfPersonsHistory WHERE GroupOfPersonsVCode = @GroupOfPersonsVCode AND GroupOfPersonsStateVCode = 2)
				BEGIN
					UPDATE AZ.[GroupOfPersons] SET GroupOfPersonsStateVCode	= 2 WHERE VCode = @GroupOfPersonsVCode
					INSERT AZ.GroupOfPersonsHistory(GroupOfPersonsVCode,GroupOfPersonsStateVCode) VALUES(@GroupOfPersonsVCode,2)
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

