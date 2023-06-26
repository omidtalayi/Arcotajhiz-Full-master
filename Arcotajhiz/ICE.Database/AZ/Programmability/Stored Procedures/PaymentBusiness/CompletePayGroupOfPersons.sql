CREATE PROCEDURE [AZ].[CompletePayGroupOfPersons](
	@GroupOfPersonsVCode BIGINT,
	@OnlinePaymentID BIGINT = NULL,
	@PortalPaymentTypeVCode Smallint,
	@State SMALLINT OUTPUT,
	@PaymentAmount DECIMAL OUTPUT
)
AS BEGIN
    BEGIN TRAN
    BEGIN TRY
		DECLARE @Amount DECIMAL,
				@IsComplete BIT,
				@IsFromApp BIT,
				@UserVCode BIGINT,
				@ResCode INT
		
		SELECT	@IsComplete = 1,
				@State = 0,
				@PaymentAmount = 0

		SELECT @ResCode = ResCode FROM AZ.OnlinePaymentReceived WHERE OnlinePaymentVCode = @OnlinePaymentID
		
		IF @ResCode <> 0
		BEGIN
			SET @IsComplete = 0
		END
				
		IF @PortalPaymentTypeVCode = 1 
		BEGIN
			SELECT @PaymentAmount = Amount FROM AZ.OnlinePayment WHERE VCode = @OnlinePaymentID
			
			IF @IsComplete = 1
			BEGIN
				IF NOT EXISTS(SELECT * FROM AZ.GroupOfPersonsHistory WHERE GroupOfPersonsVCode = @GroupOfPersonsVCode AND GroupOfPersonsStateVCode = 2)
				BEGIN
					UPDATE AZ.[GroupOfPersons] SET GroupOfPersonsStateVCode = 2 WHERE VCode = @GroupOfPersonsVCode
					INSERT AZ.GroupOfPersonsHistory(GroupOfPersonsVCode,GroupOfPersonsStateVCode) VALUES(@GroupOfPersonsVCode,2)
				END
			END 
		END

		COMMIT TRAN
		RETURN 1
	END TRY
	BEGIN CATCH
		DECLARE @ErrNo INT,@ErrMsg NVARCHAR(4000),@ErrSev INT,@ErrStt INT
		SELECT @ErrMsg = '(' + CONVERT(NVARCHAR(4000),ERROR_NUMBER()) + ') ' + ERROR_MESSAGE(),@ErrSev = ERROR_SEVERITY(),@ErrStt = ERROR_STATE()
		IF @@TRANCOUNT > 0 ROLLBACK TRAN
		RAISERROR(@ErrMsg,@ErrSev,@ErrStt)
		RETURN 0
	END CATCH
END
GO


