CREATE PROCEDURE [AZ].[ExpireIdentification]
AS
BEGIN
	BEGIN TRY
		SELECT VCode INTO #ExpireIdentification FROM AZ.Identification WHERE ExpirationDate < GETDATE()

		UPDATE AZ.Identification SET IdentificationStateVCode = 10,LastModifiedDate = GETDATE() 
		WHERE VCode IN (SELECT VCode FROM #ExpireIdentification)	

		INSERT AZ.IdentificationHistory(IdentificationVCode,IdentificationStateVCode) SELECT VCode,10 FROM #ExpireIdentification

	END TRY
	BEGIN CATCH
		DECLARE @ErrNo INT,@ErrMsg NVARCHAR(4000),@ErrSev INT,@ErrStt INT
		SELECT @ErrMsg = '(' + CONVERT(NVARCHAR(4000),ERROR_NUMBER()) + ') ' + ERROR_MESSAGE(),@ErrSev = ERROR_SEVERITY(),@ErrStt = ERROR_STATE()
		IF @@TRANCOUNT > 0 ROLLBACK TRAN
		RAISERROR(@ErrMsg,@ErrSev,@ErrStt)
	END CATCH
END