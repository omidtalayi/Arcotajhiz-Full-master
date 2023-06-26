CREATE PROCEDURE [AZ].[SetIdentificationICS24](
	@IdentificationVCode BIGINT,
	@RedirectUrlICS24 NVARCHAR(MAX),
	@IdICS24 NVARCHAR(200)
)
AS
BEGIN
	BEGIN TRAN
	BEGIN TRY
		UPDATE AZ.Identification SET RedirectUrlICS24 = @RedirectUrlICS24,IdICS24 = @IdICS24,IsPendingICS24Service = 0,ReportExpirationDate = DATEADD(DAY,1,GETDATE()) 
		WHERE VCode = @IdentificationVCode

		IF NOT EXISTS(SELECT 1 FROM AZ.IdentificationHistory WHERE IdentificationVCode = @IdentificationVCode AND IdentificationStateVCode = 17)
		BEGIN
			UPDATE [AZ].[Identification] SET IdentificationStateVCode = 17,LastModifiedDate = GETDATE() WHERE VCode = @IdentificationVCode
			INSERT AZ.IdentificationHistory(IdentificationVCode,IdentificationStateVCode) VALUES(@IdentificationVCode,17)
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
