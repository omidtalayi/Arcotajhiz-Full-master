CREATE PROCEDURE [AZ].[PagesIns](
	@Name		 NVARCHAR(MAX) = NULL,
	@PagesTypeId NVARCHAR(MAX) = NULL,
	@Title		 NVARCHAR(MAX) = NULL,
	@Image		 NVARCHAR(MAX) = NULL,
	@Description NVARCHAR(MAX) = NULL,
	@Body	     NVARCHAR(MAX) = NULL,
	@Keywords	 NVARCHAR(MAX) = NULL
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
		
		INSERT AZ.Pages([Name],PagesTypeId,Title,[Image],[Description],[Body],Keywords)
		OUTPUT inserted.ID AS NewID
		INTO @MyTableOutput(NewID)
		VALUES(@Name,@PagesTypeId,@Title,@Image,@Description,@Body,@Keywords)

		Select P.*,
		('<PagesComments>' + (SELECT * FROM AZ.PagesComment (NOLOCK) WHERE Id = P.Id And ApprovalStateVCode = 2 ORDER BY Id DESC FOR XML PATH('PagesComment')) + '</PagesComments>') PagesComments,
		1 R
		From az.Pages P
		WHERE Id = CONVERT(uniqueidentifier,(select NewID From @MyTableOutput))
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