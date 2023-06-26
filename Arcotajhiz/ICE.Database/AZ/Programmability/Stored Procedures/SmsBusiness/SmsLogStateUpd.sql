CREATE PROCEDURE [AZ].[SmsLogStateUpd](
	@Smslogs XML
)
AS
BEGIN
	BEGIN TRAN
	BEGIN TRY

	SELECT	Field.value('@V','BIGINT') VCode,
			Field.value('@S','INT') StateVCode
	INTO #SmsLog FROM @Smslogs.nodes('SL') Details(Field)

	UPDATE AZ.SmsLog 
	SET SendState = SLS.VCode,
		SendDelivery = SLS.[Name]
	FROM #SmsLog SL 
	INNER JOIN AZ.SMSLogState SLS ON SLS.Code = SL.StateVCode
	WHERE SL.VCode = AZ.SmsLog.VCode

	UPDATE AZ.Identification
	SET ReportSuccessfullySent = 1
	FROM #SmsLog SLT 
	INNER JOIN AZ.SMSLog SL ON SL.VCode = SLT.VCode
	WHERE SLT.StateVCode = 1 AND SL.IdentificationVCode = AZ.Identification.VCode AND SL.SMSLogTypeVCode = 4

	DROP TABLE #SmsLog
	COMMIT TRAN
	END TRY
		BEGIN CATCH
		DECLARE @ErrNo INT,@ErrMsg NVARCHAR(4000),@ErrSev INT,@ErrStt INT
		SELECT @ErrMsg = '(' + CONVERT(NVARCHAR(4000),ERROR_NUMBER()) + ') ' + ERROR_MESSAGE(),@ErrSev = ERROR_SEVERITY(),@ErrStt = ERROR_STATE()
		IF @@TRANCOUNT > 0 ROLLBACK TRAN
		RAISERROR(@ErrMsg,@ErrSev,@ErrStt)
	END CATCH
END