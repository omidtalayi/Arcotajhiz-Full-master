CREATE PROCEDURE [AZ].[PagesCommentIns](
	@PagesId	 UNIQUEIDENTIFIER = NULL,
	@ProductId	 UNIQUEIDENTIFIER = NULL,
	@Name		 NVARCHAR(MAX),
	@Message	 NVARCHAR(MAX),
	@Email		 NVARCHAR(MAX) = NULL,
	@Website	 NVARCHAR(MAX) = NULL,
	@ParentId	 UNIQUEIDENTIFIER = NULL,
	@Cellphone   NVARCHAR(15) = NULL
)
AS
BEGIN
	BEGIN TRAN
	BEGIN TRY
		DECLARE @uniqId NVARCHAR(50)
		DECLARE @MyTableOutput TABLE
		(
			NewID UNIQUEIDENTIFIER NOT NULL
		);	
		
		INSERT AZ.PagesComment(PagesId,ProductId,[Name],[Message],Email,Website,Cellphone,ParentId)
		OUTPUT inserted.ID AS NewID
		INTO @MyTableOutput(NewID)
		VALUES(@PagesId,@ProductId,@Name,@Message,@Email,@Website,@Cellphone,@ParentId)

		COMMIT TRAN
	END TRY
	BEGIN CATCH
		DECLARE @ErrNo INT,@ErrMsg NVARCHAR(4000),@ErrSev INT,@ErrStt INT
		SELECT @ErrMsg = '(' + CONVERT(NVARCHAR(4000),ERROR_NUMBER()) + ') ' + ERROR_MESSAGE(),@ErrSev = ERROR_SEVERITY(),@ErrStt = ERROR_STATE()
		IF @@TRANCOUNT > 0 ROLLBACK TRAN
		RAISERROR(@ErrMsg,@ErrSev,@ErrStt)
	END CATCH
END
GO