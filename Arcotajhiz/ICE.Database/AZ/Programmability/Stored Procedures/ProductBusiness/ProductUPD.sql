CREATE PROCEDURE [AZ].[ProductUPD](
	@id UNIQUEIDENTIFIER = null,
	@name NVARCHAR(40) = NULL, 
    @description NVARCHAR(MAX) = NULL, 
    @image NVARCHAR(MAX) = NULL, 
    @price DECIMAL = NULL, 
    @categoryID UNIQUEIDENTIFIER = NULL, 
    @isEnabled BIT = 1, 
    @isDeleted BIT = 0, 
    @isSpecialed BIT = 0, 
    @score DECIMAL = NULL,
	@productPropertiesXML XML = NULL
)
AS
BEGIN
	UPDATE AZ.Product SET [name] = ISNULL(@name,[name]),
                            [description] = ISNULL(@description,[description]),
                            [image] = ISNULL(@image,[image]),
                            [price] = ISNULL(@price,[price]),
                            [categoryID] = ISNULL(@categoryID,[categoryID]),
                            [isEnabled] = @isEnabled,
                            [isDeleted] = @isDeleted,
                            [isSpecialed] = @isSpecialed,
                            score = ISNULL(@score,[score])

                            WHERE id = @id

    IF @productPropertiesXML IS NOT NULL
	BEGIN
            DELETE AZ.ProductProperty Where productID = @id
			SELECT	
					Field.value('@value','NVARCHAR(50)') [value],
					@id productID,
					Field.value('@pID','uniqueidentifier') propertyID
			INTO #Data FROM @productPropertiesXML.nodes('PP') Details(Field)
			INSERT AZ.ProductProperty([value],productID,propertyID)
			SELECT * FROM #Data
    END
END