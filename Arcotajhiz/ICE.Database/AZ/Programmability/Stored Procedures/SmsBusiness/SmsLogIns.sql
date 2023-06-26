CREATE PROCEDURE [AZ].[SmsLogIns](
	@VCode					BIGINT OUTPUT,
	@Send_ID				BIGINT,
	@Send_FromNumber		NVARCHAR(20),
	@Send_ToNumber			NVARCHAR(20),
	@Send_Message			NVARCHAR(Max),
	@Send_State				SMALLINT,
	@Send_Delivery			NVARCHAR(50),
	@IdentificationVCode	BIGINT,
	@SMSLogTypeVCode         SMALLINT

)
AS
BEGIN
	BEGIN TRAN
	BEGIN TRY
		INSERT AZ.SMSLog(SendID,SendFromNumber,SendToNumber,SendMessage,SendState,SendDelivery,IdentificationVCode,SMSLogTypeVCode)
		VALUES(@Send_ID,@Send_FromNumber,@Send_ToNumber,@Send_Message,@Send_State,@Send_Delivery,@IdentificationVCode,@SMSLogTypeVCode)

		SET @VCode = SCOPE_IDENTITY()
		COMMIT TRAN
	END TRY
	BEGIN CATCH
		DECLARE @ErrNo INT,@ErrMsg NVARCHAR(4000),@ErrSev INT,@ErrStt INT
		SELECT @ErrMsg = '(' + CONVERT(NVARCHAR(4000),ERROR_NUMBER()) + ') ' + ERROR_MESSAGE(),@ErrSev = ERROR_SEVERITY(),@ErrStt = ERROR_STATE()
		IF @@TRANCOUNT > 0 ROLLBACK TRAN
		RAISERROR(@ErrMsg,@ErrSev,@ErrStt)
	END CATCH
END