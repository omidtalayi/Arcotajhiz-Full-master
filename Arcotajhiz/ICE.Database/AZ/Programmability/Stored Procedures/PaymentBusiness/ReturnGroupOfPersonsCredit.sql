CREATE PROCEDURE [AZ].[ReturnGroupOfPersonsCredit](
	@UserVCode INT,
	@GroupOfPersonsVCode BIGINT
)
AS
BEGIN
	BEGIN TRAN
    BEGIN TRY
		DECLARE @ReturnAmount DECIMAL(18,0)
		IF EXISTS(SELECT 1 FROM [$(ICEUserManagement)].AZ.[User] WHERE VCode = @UserVCode AND UserTypeVCode = 2)
		BEGIN
			IF EXISTS(SELECT 1 FROM AZ.GroupOfPersons WHERE VCode = @GroupOfPersonsVCode AND GroupOfPersonsStateVCode = 2)
			BEGIN
				SELECT @ReturnAmount = Bed FROM AZ.Accounting WHERE GroupOfPersonsVCode = @GroupOfPersonsVCode AND AccountingTypeVCode = 2
				IF @ReturnAmount > 0 AND NOT EXISTS(SELECT 1 FROM AZ.Accounting WHERE GroupOfPersonsVCode = @GroupOfPersonsVCode AND AccountingTypeVCode = 4)
				BEGIN
					EXEC AZ.AccountingIns @UserVCode = @UserVCode,@AccountingTypeVCode = 4,@Bed = 0,@Bes = @ReturnAmount,@GroupOfPersonsVCode = @GroupOfPersonsVCode

					IF NOT EXISTS(SELECT 1 FROM AZ.GroupOfPersonsHistory WHERE GroupOfPersonsVCode = @GroupOfPersonsVCode AND GroupOfPersonsStateVCode = 7)
					BEGIN
						UPDATE AZ.GroupOfPersons SET GroupOfPersonsStateVCode = 7,LastModifiedDate = GETDATE() WHERE VCode = @GroupOfPersonsVCode
						INSERT AZ.GroupOfPersonsHistory(GroupOfPersonsVCode,GroupOfPersonsStateVCode) VALUES(@GroupOfPersonsVCode,7)
					END 

					IF EXISTS(SELECT 1 FROM AZ.GroupOfPersonsHistory WHERE GroupOfPersonsVCode = @GroupOfPersonsVCode AND GroupOfPersonsStateVCode = 2)
					BEGIN
						DELETE AZ.GroupOfPersonsHistory WHERE GroupOfPersonsVCode = @GroupOfPersonsVCode AND GroupOfPersonsStateVCode = 2
					END
				END
			END
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
