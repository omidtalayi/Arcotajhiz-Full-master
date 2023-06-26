Imports System.Data.SqlClient
Imports System.Xml
Imports ARCO.Entities
Imports ARCO.Entities.Models

Public Class ProductBusiness
    Private Shared Function GetProductData(reader As IDataRecord) As ProductEntity
        Try
            Dim product = New ProductEntity With
                  {
                      .id = reader("id"),
                      .Row = reader("R"),
                      .name = If(IsDBNull(reader("name")), Nothing, reader("name")),
                      .category = If(IsDBNull(reader("Category")), Nothing, ConvertFromXMLToProductCategory(reader("Category"))),
                      .description = If(IsDBNull(reader("description")), Nothing, reader("description")),
                      .image = If(IsDBNull(reader("image")), Nothing, reader("image")),
                      .price = If(IsDBNull(reader("price")), Nothing, reader("price")),
                      .productProperties = If(IsDBNull(reader("ProductProperty")), Nothing, ConvertFromXMLToProductProperties(reader("ProductProperty"))),
                      .isDeleted = If(IsDBNull(reader("isDeleted")), False, reader("isDeleted")),
                      .isEnabled = If(IsDBNull(reader("isEnabled")), True, reader("isEnabled")),
                      .isSpecialed = If(IsDBNull(reader("isSpecialed")), False, reader("isSpecialed")),
                      .rowCount = If(IsDBNull(reader("isSpecialed")), False, reader("isSpecialed"))
                  }
            Return product
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Private Shared Function GetSingleProductData(reader As IDataRecord) As ProductEntity
        Try
            Dim product = New ProductEntity With
                  {
                      .id = reader("id"),
                      .name = If(IsDBNull(reader("name")), Nothing, reader("name")),
                      .category = If(IsDBNull(reader("Category")), Nothing, ConvertFromXMLToProductCategory(reader("Category"))),
                      .description = If(IsDBNull(reader("description")), Nothing, reader("description")),
                      .image = If(IsDBNull(reader("image")), Nothing, reader("image")),
                      .price = If(IsDBNull(reader("price")), Nothing, reader("price")),
                      .isDeleted = If(IsDBNull(reader("isDeleted")), False, reader("isDeleted")),
                      .isEnabled = If(IsDBNull(reader("isEnabled")), True, reader("isEnabled")),
                      .isSpecialed = If(IsDBNull(reader("isSpecialed")), False, reader("isSpecialed"))
                  }
            product.productProperties = If(IsDBNull(reader("ProductProperty")), Nothing, ConvertFromXMLToProductProperties(reader("ProductProperty")))
            Return product
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Private Shared Function GetCategoryData(reader As IDataRecord) As CategoryEntity
        Try
            Dim category = New CategoryEntity With
                  {
                      .id = reader("id"),
                      .name = If(IsDBNull(reader("name")), Nothing, reader("name")),
                      .image = If(IsDBNull(reader("image")), Nothing, reader("image")),
                      .createDate = If(IsDBNull(reader("createDate")), Date.MinValue, reader("createDate")),
                      .lastModifiedDate = If(IsDBNull(reader("lastModifiedDate")), Date.MinValue, reader("lastModifiedDate")),
                      .isDeleted = If(IsDBNull(reader("isDeleted")), False, reader("isDeleted")),
                      .isEnabled = If(IsDBNull(reader("isEnabled")), True, reader("isEnabled"))
                  }
            category.parentID = If(IsDBNull(reader("parentID")), Guid.Empty, reader("parentID"))
            Return category
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Private Shared Function GetPropertyData(reader As IDataRecord) As PropertyEntity
        Try
            Dim prop = New PropertyEntity With
                  {
                      .id = reader("id"),
                      .name = If(IsDBNull(reader("name")), Nothing, reader("name")),
                      .defaultValue = If(IsDBNull(reader("defaultValue")), Nothing, reader("defaultValue")),
                      .createDate = If(IsDBNull(reader("createDate")), Date.MinValue, reader("createDate")),
                      .lastModifiedDate = If(IsDBNull(reader("lastModifiedDate")), Date.MinValue, reader("lastModifiedDate")),
                      .isDeleted = If(IsDBNull(reader("isDeleted")), False, reader("isDeleted")),
                      .isEnabled = If(IsDBNull(reader("isEnabled")), True, reader("isEnabled"))
                  }
            Return prop
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Shared Function ConvertFromXMLToProductProperties(xmlString As String) As List(Of ProductPropertyEntity)
        Try
            Dim productProperties = New List(Of ProductPropertyEntity)
            Dim xml As New XmlDocument()
            If Not xmlString Is Nothing Then
                xml.LoadXml(xmlString)
                Dim xnList As XmlNodeList = xml.SelectNodes("/ProductProperties/ProductProperty")
                For Each xn As XmlNode In xnList
                    Dim productProperty = New ProductPropertyEntity
                    If xn("id") IsNot Nothing Then
                        productProperty.id = Guid.Parse(xn("id").InnerText.ToString())
                    End If
                    If xn("value") IsNot Nothing Then
                        productProperty.value = xn("value").InnerText
                    End If
                    If xn("productID") IsNot Nothing Then
                        productProperty.productID = xn("productID").InnerText
                    End If
                    productProperty.createDate = xn("createDate").InnerText
                    productProperty.lastModifiedDate = xn("lastModifiedDate").InnerText
                    If xn("Property") IsNot Nothing Then
                        productProperty.Property = New PropertyEntity With {
                            .id = Guid.Parse(xn("Property").ChildNodes().Item(0).InnerText),
                            .defaultValue = xn("Property").ChildNodes().Item(2).InnerText,
                            .name = xn("Property").ChildNodes().Item(1).InnerText
                            }
                        'xn("Property").InnerXml("212")
                        'productProperty.EnumName = xn("EnumName").InnerText
                    End If
                    productProperties.Add(productProperty)
                Next
            End If
            Return productProperties
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Shared Function ConvertFromXMLToProductCategory(xmlString As String) As CategoryEntity
        Try
            Dim category = New CategoryEntity
            Dim xml As New XmlDocument()
            If Not xmlString Is Nothing Then
                xml.LoadXml(xmlString)
                Dim xnList As XmlNodeList = xml.SelectNodes("/ProductCategories/ProductCategory")
                For Each xn As XmlNode In xnList
                    If xn("id") IsNot Nothing Then
                        category.id = Guid.Parse(xn("id").InnerText.ToString())
                    End If
                    If xn("name") IsNot Nothing Then
                        category.name = xn("name").InnerText
                    End If
                    If xn("parentID") IsNot Nothing Then
                        category.parentID = Guid.Parse(xn("parentID").InnerText.ToString())
                    End If
                    category.createDate = xn("createDate").InnerText
                    category.lastModifiedDate = xn("lastModifiedDate").InnerText
                Next
            End If
            Return category
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetAllProducts(Optional pageNo As Integer = 0, Optional pageSize As Integer = 0, Optional categoryId As Guid? = Nothing, Optional search As String = Nothing, Optional props As List(Of Guid?) = Nothing, Optional ByRef rowC As Integer = Nothing) As List(Of ProductEntity)
        Dim products As New List(Of ProductEntity)
        Dim rowCount As Integer = 0
        Dim command As SqlCommand = Nothing
        Dim reader As SqlDataReader = Nothing
        Try
            command = ARCOConnection.Command("AZ.GetProducts")
            command.Parameters("@PageNo").Value = If(pageNo = 0, DBNull.Value, pageNo)
            command.Parameters("@PageSize").Value = If(pageSize = 0, DBNull.Value, pageSize)
            command.Parameters("@CategoryId").Value = If(categoryId = Nothing, DBNull.Value, categoryId)
            command.Parameters("@Name").Value = If(search = Nothing, DBNull.Value, search)
            command.Parameters("@ProductProperties").Value = If(props Is Nothing OrElse props.Count = 0, DBNull.Value, ConvertProductPropertiesDataToXmlPP(props))
            reader = command.ExecuteReader()
            While reader.Read()
                products.Add(GetProductData(reader))
                rowC = reader("RowCount")
            End While
            Return products
        Catch ex As Exception
            Throw ex
        Finally
            If reader IsNot Nothing Then
                reader.Close()
                reader.Dispose()
            End If
            If command IsNot Nothing Then
                command.Connection.Close()
                command.Connection.Dispose()
                command.Dispose()
            End If
        End Try
    End Function
    Public Function GetCategories(Optional pageNo As Integer = 0, Optional pageSize As Integer = 0) As List(Of CategoryEntity)
        Dim items As New List(Of CategoryEntity)
        Dim rowCount As Integer = 0
        Dim command As SqlCommand = Nothing
        Dim reader As SqlDataReader = Nothing
        Try
            command = ARCOConnection.Command("AZ.GetCategories")
            command.Parameters("@PageNo").Value = If(pageNo = 0, DBNull.Value, pageNo)
            command.Parameters("@PageSize").Value = If(pageSize = 0, DBNull.Value, pageSize)
            reader = command.ExecuteReader()
            While reader.Read()
                items.Add(GetCategoryData(reader))
                'rowCount = reader("RowCount")
            End While
            Return items
        Catch ex As Exception
            Throw ex
        Finally
            If reader IsNot Nothing Then
                reader.Close()
                reader.Dispose()
            End If
            If command IsNot Nothing Then
                command.Connection.Close()
                command.Connection.Dispose()
                command.Dispose()
            End If
        End Try
    End Function
    Public Function GetCategory(categoryId As Guid?) As CategoryEntity
        Dim item As New CategoryEntity
        Dim command As SqlCommand = Nothing
        Dim reader As SqlDataReader = Nothing
        Try
            command = ARCOConnection.Command("AZ.GetCategory")
            command.Parameters("@CategoryId").Value = If(categoryId = Nothing, DBNull.Value, categoryId)
            reader = command.ExecuteReader()
            If reader.HasRows Then
                reader.Read()
                item = GetCategoryData(reader)
            End If
            Return item
        Catch ex As Exception
            Throw ex
        Finally
            If reader IsNot Nothing Then
                reader.Close()
                reader.Dispose()
            End If
            If command IsNot Nothing Then
                command.Connection.Close()
                command.Connection.Dispose()
                command.Dispose()
            End If
        End Try
    End Function
    Public Function GetProduct(productId As Guid?) As ProductEntity
        Dim item As New ProductEntity
        Dim command As SqlCommand = Nothing
        Dim reader As SqlDataReader = Nothing
        Try
            command = ARCOConnection.Command("AZ.GetProduct")
            command.Parameters("@id").Value = If(productId = Nothing, DBNull.Value, productId)
            reader = command.ExecuteReader()
            If reader.HasRows Then
                reader.Read()
                item = GetProductData(reader)
            End If
            Return item
        Catch ex As Exception
            Throw ex
        Finally
            If reader IsNot Nothing Then
                reader.Close()
                reader.Dispose()
            End If
            If command IsNot Nothing Then
                command.Connection.Close()
                command.Connection.Dispose()
                command.Dispose()
            End If
        End Try
    End Function
    Public Function DeleteProduct(productId As Guid?) As ProductEntity
        Dim item As New ProductEntity
        Dim command As SqlCommand = Nothing
        Dim reader As SqlDataReader = Nothing
        Try
            command = ARCOConnection.Command("AZ.DeleteProduct")
            command.Parameters("@id").Value = If(productId = Nothing, DBNull.Value, productId)
            reader = command.ExecuteReader()
            If reader.HasRows Then
                reader.Read()
                item = GetProductData(reader)
            End If
            Return item
        Catch ex As Exception
            Throw ex
        Finally
            If reader IsNot Nothing Then
                reader.Close()
                reader.Dispose()
            End If
            If command IsNot Nothing Then
                command.Connection.Close()
                command.Connection.Dispose()
                command.Dispose()
            End If
        End Try
    End Function
    Public Function DeleteCategory(id As Guid?) As CategoryEntity
        Dim item As New CategoryEntity
        Dim command As SqlCommand = Nothing
        Dim reader As SqlDataReader = Nothing
        Try
            command = ARCOConnection.Command("AZ.DeleteCategory")
            command.Parameters("@id").Value = If(id = Nothing, DBNull.Value, id)
            reader = command.ExecuteReader()
            If reader.HasRows Then
                reader.Read()
                item = GetCategoryData(reader)
            End If
            Return item
        Catch ex As Exception
            Throw ex
        Finally
            If reader IsNot Nothing Then
                reader.Close()
                reader.Dispose()
            End If
            If command IsNot Nothing Then
                command.Connection.Close()
                command.Connection.Dispose()
                command.Dispose()
            End If
        End Try
    End Function
    Public Function GetProperties(Optional pageNo As Integer = 0, Optional pageSize As Integer = 0) As List(Of PropertyEntity)
        Dim items As New List(Of PropertyEntity)
        Dim rowCount As Integer = 0
        Dim command As SqlCommand = Nothing
        Dim reader As SqlDataReader = Nothing
        Try
            command = ARCOConnection.Command("AZ.GetProperties")
            command.Parameters("@PageNo").Value = If(pageNo = 0, DBNull.Value, pageNo)
            command.Parameters("@PageSize").Value = If(pageSize = 0, DBNull.Value, pageSize)
            reader = command.ExecuteReader()
            While reader.Read()
                items.Add(GetPropertyData(reader))
                'rowCount = reader("RowCount")
            End While
            Return items
        Catch ex As Exception
            Throw ex
        Finally
            If reader IsNot Nothing Then
                reader.Close()
                reader.Dispose()
            End If
            If command IsNot Nothing Then
                command.Connection.Close()
                command.Connection.Dispose()
                command.Dispose()
            End If
        End Try
    End Function
    Public Function AddProduct(product As ProductEntity) As ProductEntity
        Dim productobj = New ProductEntity()
        Dim command As SqlCommand = Nothing
        Dim reader As SqlDataReader = Nothing
        Try
            command = ARCOConnection.Command("AZ.ProductIns")
            command.Parameters("@description").Value = If(String.IsNullOrEmpty(product.description), DBNull.Value, product.description)
            command.Parameters("@price").Value = If(String.IsNullOrEmpty(product.price), DBNull.Value, product.price)
            command.Parameters("@name").Value = If(String.IsNullOrEmpty(product.name), DBNull.Value, product.name)
            command.Parameters("@image").Value = If(String.IsNullOrEmpty(product.image), DBNull.Value, product.image)
            command.Parameters("@isEnabled").Value = product.isEnabled
            command.Parameters("@isDeleted").Value = product.isDeleted
            command.Parameters("@isSpecialed").Value = product.isSpecialed
            command.Parameters("@score").Value = If(product.score = 0, DBNull.Value, product.score)
            command.Parameters("@categoryID").Value = If(product.category Is Nothing, DBNull.Value, product.category.id)
            command.Parameters("@productPropertiesXML").Value = If(product.productProperties Is Nothing, DBNull.Value, ConvertProductPropertiesDataToXml(product.productProperties))
            reader = command.ExecuteReader()
            If reader.HasRows Then
                reader.Read()
                productobj = GetSingleProductData(reader)
            End If

            Return productobj
        Catch ex As Exception
            Throw ex
        Finally
            If reader IsNot Nothing Then
                reader.Close()
                reader.Dispose()
            End If
            If command IsNot Nothing Then
                command.Connection.Close()
                command.Connection.Dispose()
                command.Dispose()
            End If
        End Try
    End Function
    Public Function UpdateProduct(product As ProductEntity) As ProductEntity
        Dim productobj = New ProductEntity()
        Dim command As SqlCommand = Nothing
        Dim reader As SqlDataReader = Nothing
        Try
            command = ARCOConnection.Command("AZ.ProductUPD")
            command.Parameters("@id").Value = product.id
            command.Parameters("@description").Value = If(String.IsNullOrEmpty(product.description), DBNull.Value, product.description)
            command.Parameters("@price").Value = If(String.IsNullOrEmpty(product.price), DBNull.Value, product.price)
            command.Parameters("@name").Value = If(String.IsNullOrEmpty(product.name), DBNull.Value, product.name)
            command.Parameters("@image").Value = If(String.IsNullOrEmpty(product.image), DBNull.Value, product.image)
            command.Parameters("@isEnabled").Value = product.isEnabled
            command.Parameters("@isDeleted").Value = product.isDeleted
            command.Parameters("@isSpecialed").Value = product.isSpecialed
            command.Parameters("@score").Value = If(product.score = 0, DBNull.Value, product.score)
            command.Parameters("@categoryID").Value = If(product.category Is Nothing, DBNull.Value, product.category.id)
            command.Parameters("@productPropertiesXML").Value = If(product.productProperties Is Nothing, DBNull.Value, ConvertProductPropertiesDataToXml(product.productProperties))
            reader = command.ExecuteReader()
            If reader.HasRows Then
                reader.Read()
                productobj = GetSingleProductData(reader)
            End If

            Return productobj
        Catch ex As Exception
            Throw ex
        Finally
            If reader IsNot Nothing Then
                reader.Close()
                reader.Dispose()
            End If
            If command IsNot Nothing Then
                command.Connection.Close()
                command.Connection.Dispose()
                command.Dispose()
            End If
        End Try
    End Function
    Public Function AddCategory(category As CategoryEntity) As CategoryEntity
        Dim categoryObj = New CategoryEntity()
        Dim command As SqlCommand = Nothing
        Dim reader As SqlDataReader = Nothing
        Try
            command = ARCOConnection.Command("AZ.CategoryIns")
            command.Parameters("@Id").Value = If(category.id = Nothing, DBNull.Value, category.id)
            command.Parameters("@parentID").Value = If(category.parentID = Nothing, DBNull.Value, category.parentID)
            command.Parameters("@name").Value = If(String.IsNullOrEmpty(category.name), DBNull.Value, category.name)
            command.Parameters("@image").Value = If(String.IsNullOrEmpty(category.image), DBNull.Value, category.image)
            command.Parameters("@isEnabled").Value = If(String.IsNullOrEmpty(category.isEnabled), DBNull.Value, category.isEnabled)
            command.Parameters("@isDeleted").Value = If(String.IsNullOrEmpty(category.isDeleted), DBNull.Value, category.isDeleted)
            reader = command.ExecuteReader()
            If reader.HasRows Then
                reader.Read()
                categoryObj = GetCategoryData(reader)
            End If

            Return categoryObj
        Catch ex As Exception
            Throw ex
        Finally
            If reader IsNot Nothing Then
                reader.Close()
                reader.Dispose()
            End If
            If command IsNot Nothing Then
                command.Connection.Close()
                command.Connection.Dispose()
                command.Dispose()
            End If
        End Try
    End Function
    Public Function AddProperty(prop As PropertyEntity) As PropertyEntity
        Dim propertyObj = New PropertyEntity()
        Dim command As SqlCommand = Nothing
        Dim reader As SqlDataReader = Nothing
        Try
            command = ARCOConnection.Command("AZ.PropertyIns")
            command.Parameters("@Id").Value = If(prop.id = Nothing, DBNull.Value, prop.id)
            command.Parameters("@name").Value = If(String.IsNullOrEmpty(prop.name), DBNull.Value, prop.name)
            command.Parameters("@defaultValue").Value = If(String.IsNullOrEmpty(prop.defaultValue), DBNull.Value, prop.defaultValue)
            command.Parameters("@isEnabled").Value = If(String.IsNullOrEmpty(prop.isEnabled), DBNull.Value, prop.isEnabled)
            command.Parameters("@isDeleted").Value = If(String.IsNullOrEmpty(prop.isDeleted), DBNull.Value, prop.isDeleted)
            reader = command.ExecuteReader()
            If reader.HasRows Then
                reader.Read()
                propertyObj = GetPropertyData(reader)
            End If

            Return propertyObj
        Catch ex As Exception
            Throw ex
        Finally
            If reader IsNot Nothing Then
                reader.Close()
                reader.Dispose()
            End If
            If command IsNot Nothing Then
                command.Connection.Close()
                command.Connection.Dispose()
                command.Dispose()
            End If
        End Try
    End Function

    Public Function DeleteProperty(id As Guid?) As Boolean
        Dim command As SqlCommand = Nothing
        Dim reader As SqlDataReader = Nothing
        Try
            command = ARCOConnection.Command("AZ.PropertyDelete")
            command.Parameters("@id").Value = If(id = Nothing, DBNull.Value, id)
            reader = command.ExecuteReader()
            Return True
        Catch ex As Exception
            Throw ex
            Return False
        Finally
            If reader IsNot Nothing Then
                reader.Close()
                reader.Dispose()
            End If
            If command IsNot Nothing Then
                command.Connection.Close()
                command.Connection.Dispose()
                command.Dispose()
            End If
        End Try
    End Function
    Private Shared Function ConvertProductPropertiesDataToXml(productProperties As List(Of ProductPropertyEntity)) As String
        Try
            Return productProperties.Aggregate(String.Empty, Function(current, productProperty) current & String.Format("<PP value=""{0}"" pID=""{1}""/>", productProperty.value, productProperty.Property.id))
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Private Shared Function ConvertProductPropertiesDataToXmlPP(productProperties As List(Of Guid?)) As String
        Try
            Return productProperties.Aggregate(String.Empty, Function(current, productProperty) current & String.Format("<PP value=""{0}"" pID=""{1}""/>", "test", productProperty))
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
