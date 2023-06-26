CREATE PROCEDURE [AZ].[DeletePage](
		@id UNIQUEIDENTIFIER = null
	)
	AS
		Update Az.Pages Set isDeleted = 1
		WHERE id = @id