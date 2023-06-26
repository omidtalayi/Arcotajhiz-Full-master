CREATE PROCEDURE [AZ].[GetCategory](
		@CategoryId UNIQUEIDENTIFIER = null
	)
	AS
	Select * from az.Category WHERE id = @CategoryId AND isDeleted = 0