CREATE PROCEDURE [AZ].[CancelPayGroupOfPersons](
	@OnlinePaymentID BIGINT,
	@RefID VARCHAR(100)= NULL,
	@SaleRefID VARCHAR(100),
	@CardholderInfo VARCHAR(100) = NULL,
	@CardPAN VARCHAR(25) = NULL,
	@ResCode INT  = NULL,
	@GroupOfPersonsVCode BIGINT OUTPUT,
	@State BIT OUTPUT,
	@PortalPaymentTypeVCode Smallint
)
AS BEGIN
    BEGIN TRAN
    BEGIN TRY
		DECLARE @OnlinePaymentReceivedVCode BIGINT

		If @OnlinePaymentID IS NULL 
		BEGIN
			RAISERROR('"OnlinePaymentID" is missing',16,126)
		END

		IF @PortalPaymentTypeVCode IS NULL
		BEGIN
			RAISERROR('@PortalPaymentTypeVCode is null',16,126)
		END

		IF @PortalPaymentTypeVCode = 1 
		BEGIN
			SELECT @GroupOfPersonsVCode = GroupOfPersonsVCode FROM AZ.OnlinePayment WHERE VCode = @OnlinePaymentID
			If @GroupOfPersonsVCode IS NULL 
			BEGIN
				RAISERROR('"OnlinePaymentID" is invalid',16,126)
			END

			IF EXISTS(SELECT 1 FROM AZ.GroupOfPersons WHERE VCode = @GroupOfPersonsVCode AND GroupOfPersonsStateVCode = 1)
			BEGIN
				UPDATE AZ.OnlinePayment SET SaleRefID = @SaleRefID WHERE VCode = @OnlinePaymentID

				SET @State = 1
				COMMIT TRAN
			END
			ELSE
			BEGIN
				SET @State = 0
			END
		END
		
	END TRY
	BEGIN CATCH
		DECLARE @ErrNo INT,@ErrMsg NVARCHAR(4000),@ErrSev INT,@ErrStt INT
		SELECT @ErrMsg = '(' + CONVERT(NVARCHAR(4000),ERROR_NUMBER()) + ') ' + ERROR_MESSAGE(),@ErrSev = ERROR_SEVERITY(),@ErrStt = ERROR_STATE()
		IF @@TRANCOUNT > 0 ROLLBACK TRAN
		RAISERROR(@ErrMsg,@ErrSev,@ErrStt)
		SET @State = 0
	END CATCH
END
