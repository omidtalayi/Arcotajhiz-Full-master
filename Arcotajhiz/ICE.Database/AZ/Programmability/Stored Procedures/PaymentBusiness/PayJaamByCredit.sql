CREATE PROCEDURE [AZ].[PayJaamByCredit](
	@UserVCode INT,
	@JaamVCode BIGINT,
	@PayJaamByCreditState SMALLINT OUTPUT
)
AS
BEGIN
	BEGIN TRAN
    BEGIN TRY
		SET @PayJaamByCreditState = 0
		DECLARE @ReportPrice DECIMAL(18,0),
				@TaxPrice DECIMAL(18,0),
				@TotalPrice	DECIMAL(18,0),
				@PaymentAmount DECIMAL(28,0),
				@SourceSharePercentage DECIMAL(18,10),
				@SourceShare DECIMAL(18,0),
				@JaamTypeVCode INT

		CREATE TABLE #UserCredit( 
			Credit DECIMAL(18,0)
		)

		INSERT #UserCredit EXEC [AZ].[GetUserCredit] @UserVCode = @UserVCode

		SELECT @JaamTypeVCode = JaamTypeVCode FROM AZ.Jaam WHERE VCode = @JaamVCode
		
		IF @JaamTypeVCode = 1
		BEGIN
			SELECT @ReportPrice = 3000000
			SELECT @TaxPrice = 0
		END
		IF @JaamTypeVCode = 2
		BEGIN
			SELECT @ReportPrice = 3000000
			SELECT @TaxPrice = 0
		END

		SELECT @PaymentAmount = @ReportPrice + @TaxPrice

		IF EXISTS(SELECT 1 FROM AZ.Jaam WHERE VCode = @JaamVCode)
		BEGIN
			IF (SELECT ISNULL(Credit,0) FROM #UserCredit) < @PaymentAmount
			BEGIN
				SET @PayJaamByCreditState = 2	
			END
			ELSE
			BEGIN
				SET @PayJaamByCreditState = 1
				EXEC AZ.AccountingIns @UserVCode = @UserVCode,@AccountingTypeVCode = 2,@Bed = @PaymentAmount,@Bes = 0,@JaamVCode = @JaamVCode

				IF NOT EXISTS(SELECT 1 FROM AZ.JaamHistory WHERE JaamVCode = @JaamVCode AND JaamStateVCode = 2)
				BEGIN
					UPDATE AZ.[Jaam] SET JaamStateVCode	= 2 WHERE VCode = @JaamVCode
					INSERT AZ.JaamHistory(JaamVCode,JaamStateVCode) VALUES(@JaamVCode,2)
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