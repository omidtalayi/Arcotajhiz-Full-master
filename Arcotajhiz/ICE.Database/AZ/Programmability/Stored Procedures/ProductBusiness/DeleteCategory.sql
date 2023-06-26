CREATE PROCEDURE [AZ].[DeleteCategory](
		@id UNIQUEIDENTIFIER = null
	)
	AS
		Update Az.Category Set isDeleted = 1
		WHERE id = @id