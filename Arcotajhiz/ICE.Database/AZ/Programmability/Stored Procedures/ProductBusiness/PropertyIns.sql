CREATE PROCEDURE [AZ].[PropertyIns](
	@Id NVARCHAR(20) NULL,
	@name NVARCHAR(60) NULL,
    @defaultValue NVARCHAR(MAX) NULL, 
    @isEnabled BIT = 1,
    @isDeleted BIT = 0
 )
AS
BEGIN
	BEGIN TRAN
	BEGIN TRY
		DECLARE @productId NVARCHAR(50)
		DECLARE @MyTableOutput TABLE
		(
			NewID UNIQUEIDENTIFIER NOT NULL
		);
		IF @Id IS NULL
		BEGIN
			INSERT AZ.Property([name],defaultValue,isEnabled,isDeleted)
			OUTPUT inserted.ID AS NewID
			INTO @MyTableOutput(NewID)
			VALUES(@name,@defaultValue,@isEnabled,@isDeleted)
		END
		IF @Id IS NOT NULL
		BEGIN
			UPDATE AZ.Property 
			SET [name] = ISNULL(@name,[name]),
				defaultValue = ISNULL(@defaultValue,defaultValue),
				isEnabled = ISNULL(@isEnabled,isEnabled),
				isDeleted = ISNULL(@isDeleted,isDeleted),
				LastModifiedDate = GETDATE()
			WHERE id = @Id
		END
		SELECT * from Az.Property WHERE id = CONVERT(uniqueidentifier,(select NewID From @MyTableOutput))
		COMMIT TRAN
	END TRY
		BEGIN CATCH
		DECLARE @ErrNo INT,@ErrMsg NVARCHAR(4000),@ErrSev INT,@ErrStt INT
		SELECT @ErrMsg = '(' + CONVERT(NVARCHAR(4000),ERROR_NUMBER()) + ') ' + ERROR_MESSAGE(),@ErrSev = ERROR_SEVERITY(),@ErrStt = ERROR_STATE()
		IF @@TRANCOUNT > 0 ROLLBACK TRAN
		RAISERROR(@ErrMsg,@ErrSev,@ErrStt)
	END CATCH
END
