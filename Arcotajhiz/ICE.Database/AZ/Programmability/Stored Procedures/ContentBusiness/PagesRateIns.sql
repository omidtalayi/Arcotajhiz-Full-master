CREATE PROCEDURE [AZ].[PagesRateIns](
	@VCode			BIGINT OUTPUT,
	@PagesVCode		INT,
	@Rate			DECIMAL(5,2),
	@IP				NVARCHAR(20) = NULL,
	@SessionId		NVARCHAR(100),
	@AverageRate	DECIMAL(5,2) OUTPUT,
	@Message		NVARCHAR(MAX) OUTPUT
)
AS
BEGIN
	BEGIN TRAN
	BEGIN TRY
		
		IF EXISTS(SELECT 1 FROM AZ.PagesRate WHERE PagesVCode = @PagesVCode AND [IP] = @IP AND SessionId = @SessionId)
		BEGIN
			--SET @Message = 'User has already rated this page'
			UPDATE AZ.PagesRate SET Rate = @Rate WHERE PagesVCode = @PagesVCode AND [IP] = @IP AND SessionId = @SessionId 
		END
		ELSE
		BEGIN
			INSERT AZ.PagesRate(PagesVCode,Rate,[IP],SessionId)
			VALUES(@PagesVCode,@Rate,@IP,@SessionId)

			SET @VCode = SCOPE_IDENTITY()
		END

		SELECT @AverageRate = ROUND(AVG(Rate),1) FROM AZ.PagesRate (NOLOCK) WHERE PagesVCode = @PagesVCode

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

