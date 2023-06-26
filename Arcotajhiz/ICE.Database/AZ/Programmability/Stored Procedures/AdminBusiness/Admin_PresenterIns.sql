CREATE PROCEDURE AZ.Admin_PresenterIns
(
	@FirstName NVARCHAR(100) ,
	@LastName NVARCHAR(100),
	@CellPhone NVARCHAR(20)
)
AS
BEGIN
	BEGIN TRY
	IF EXISTS(SELECT 1 FROM AZ.Presenter P WHERE P.CellPhone = @CellPhone)
	BEGIN
		SELECT 2 [State],'CellPhone has already existed' [Message]
		RETURN 0
	END
	
	INSERT INTO AZ.Presenter(FirstName,LastName,CellPhone,Code,IsDeleted,EntryDate,LastModifiedDate)
		VALUES(@FirstName,@LastName,@CellPhone,@CellPhone,0,GETDATE(),GETDATE())
	SELECT 1 [State],'User has been successfully submitted' [Message]
	
	END TRY
	BEGIN CATCH
		DECLARE @ErrNo INT,@ErrMsg NVARCHAR(4000),@ErrSev INT,@ErrStt INT
		SELECT @ErrMsg = '(' + CONVERT(NVARCHAR(4000),ERROR_NUMBER()) + ') ' + ERROR_MESSAGE(),@ErrSev = ERROR_SEVERITY(),@ErrStt = ERROR_STATE()
		IF @@TRANCOUNT > 0 ROLLBACK TRAN
		RAISERROR(@ErrMsg,@ErrSev,@ErrStt)
	END CATCH
END



