CREATE PROCEDURE [AZ].[DeleteProduct](
		@id UNIQUEIDENTIFIER = null
	)
	AS
		Update Az.Product Set isDeleted = 1
		WHERE id = @id