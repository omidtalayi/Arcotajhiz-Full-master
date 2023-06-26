CREATE PROCEDURE [AZ].[GetProduct](
		@id UNIQUEIDENTIFIER = null
	)
	AS
	Select P.*,
	('<ProductCategories>' + (SELECT * FROM AZ.Category NOLOCK WHERE Id = P.categoryId FOR XML PATH('ProductCategory')) + '</ProductCategories>') Category,
	('<ProductProperties>' + (SELECT *,(SELECT * FROM AZ.Property (NOLOCK) PR WHERE PR.id = propertyID FOR XML PATH('Property'),type) FROM AZ.ProductProperty NOLOCK WHERE productID = P.id FOR XML PATH('ProductProperty')) + '</ProductProperties>') ProductProperty,
	('<Galleries>' + (SELECT * FROM AZ.Gallery (NOLOCK) WHERE productID = P.id FOR XML PATH('Gallery')) + '</Galleries>') ProductGallery,
	1 R
	From az.Product P WHERE id = @id AND isDeleted = 0