﻿CREATE PROCEDURE [AZ].[UserWebHookCallIns](
	@UserVCode	BIGINT,
	@UserWebHookVCode BIGINT,
	@IdentificationVCode BIGINT
)
AS
BEGIN
	BEGIN TRY 
		INSERT AZ.UserWebHookCall(UserVCode,UserWebHookVCode,IdentificationVCode)
		VALUES(@UserVCode,@UserWebHookVCode,@IdentificationVCode)
	END TRY
	BEGIN CATCH
		DECLARE @ErrNo INT,@ErrMsg NVARCHAR(4000),@ErrSev INT,@ErrStt INT
		SELECT @ErrMsg = '(' + CONVERT(NVARCHAR(4000),ERROR_NUMBER()) + ') ' + ERROR_MESSAGE(),@ErrSev = ERROR_SEVERITY(),@ErrStt = ERROR_STATE()
		IF @@TRANCOUNT > 0 ROLLBACK TRAN
		RAISERROR(@ErrMsg,@ErrSev,@ErrStt)
	END CATCH
END