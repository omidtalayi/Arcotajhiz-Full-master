CREATE PROCEDURE [AZ].[SetIdentificationAppICS24HashCode](
	@IdentificationVCode BIGINT,
	@AppIcs24HashCode NVARCHAR(MAX)
)
AS
BEGIN
	BEGIN TRAN
	BEGIN TRY
		UPDATE AZ.Identification SET AppIcs24HashCode = @AppIcs24HashCode,ReportExpirationDate = DATEADD(DAY,1,GETDATE()) ,IsPendingICS24Service = 0
		WHERE VCode = @IdentificationVCode
		UPDATE [AZ].[Identification] SET IdentificationStateVCode = 17,LastModifiedDate = GETDATE() WHERE VCode = @IdentificationVCode
		IF NOT EXISTS(SELECT 1 FROM AZ.IdentificationHistory WHERE IdentificationVCode = @IdentificationVCode AND IdentificationStateVCode = 17)
		BEGIN
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