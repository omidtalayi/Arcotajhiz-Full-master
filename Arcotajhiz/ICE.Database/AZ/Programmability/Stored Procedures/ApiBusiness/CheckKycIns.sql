﻿CREATE PROCEDURE [AZ].[CheckKycIns](
	@VCode			BIGINT OUTPUT,
	@UserVCode		BIGINT,
	@NationalCode	NVARCHAR(20),
	@Cellphone		NVARCHAR(20),
	@IP				NVARCHAR(100)
)
AS
BEGIN
	BEGIN TRY
		INSERT AZ.Kyc(UserVCode,NationalCode,Cellphone,IsValid,[IP])
		VALUES(@UserVCode,@NationalCode,@Cellphone,0,@IP)

		SET @VCode = SCOPE_IDENTITY()
	END TRY
		BEGIN CATCH
		DECLARE @ErrNo INT,@ErrMsg NVARCHAR(4000),@ErrSev INT,@ErrStt INT
		SELECT @ErrMsg = '(' + CONVERT(NVARCHAR(4000),ERROR_NUMBER()) + ') ' + ERROR_MESSAGE(),@ErrSev = ERROR_SEVERITY(),@ErrStt = ERROR_STATE()
		IF @@TRANCOUNT > 0 ROLLBACK TRAN
		RAISERROR(@ErrMsg,@ErrSev,@ErrStt)
	END CATCH
END