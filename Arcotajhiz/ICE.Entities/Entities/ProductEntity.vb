Namespace Models
    Public Class ProductEntity
        Public Property id As Guid
        Public Property row As Integer
        Public Property name As String
        Public Property description As String
        Public Property image As String
        Public Property price As Decimal
        Public Property isEnabled As Boolean
        Public Property isDeleted As Boolean
        Public Property isSpecialed As Boolean
        Public Property score As Decimal
        Public Property createDate As DateTime
        Public Property lastModifiedDate As DateTime
        Public Property category As CategoryEntity
        Public Property productProperties As List(Of ProductPropertyEntity)
        Public Property galleries As List(Of GalleryEntity)
        Public Property rowCount As Long

    End Class
End Namespace