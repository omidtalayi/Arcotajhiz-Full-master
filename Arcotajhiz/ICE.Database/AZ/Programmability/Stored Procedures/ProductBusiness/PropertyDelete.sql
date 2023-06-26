﻿CREATE PROCEDURE [AZ].[PropertyDelete](
	@id UNIQUEIDENTIFIER = null
 )
AS
BEGIN
	BEGIN TRAN
	BEGIN TRY
		DELETE AZ.ProductProperty WHERE propertyID = @Id
		DELETE AZ.Property WHERE id = @Id
		COMMIT TRAN
	END TRY
		BEGIN CATCH
		DECLARE @ErrNo INT,@ErrMsg NVARCHAR(4000),@ErrSev INT,@ErrStt INT
		SELECT @ErrMsg = '(' + CONVERT(NVARCHAR(4000),ERROR_NUMBER()) + ') ' + ERROR_MESSAGE(),@ErrSev = ERROR_SEVERITY(),@ErrStt = ERROR_STATE()
		IF @@TRANCOUNT > 0 ROLLBACK TRAN
		RAISERROR(@ErrMsg,@ErrSev,@ErrStt)
	END CATCH
END
