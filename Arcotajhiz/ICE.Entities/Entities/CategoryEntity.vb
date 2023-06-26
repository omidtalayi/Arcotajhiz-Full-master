Public Class CategoryEntity
    Public Property id As Guid
    Public Property name As String
    Public Property image As String
    Public Property parentID As Guid
    Public Property isDeleted As Boolean
    Public Property isEnabled As Boolean
    Public Property createDate As DateTime
    Public Property lastModifiedDate As DateTime

    Public ReadOnly Property Link() As String
        Get
            Return "/Product/Category?id=" + id.ToString() + "&title=" + name
        End Get
    End Property
End Class
