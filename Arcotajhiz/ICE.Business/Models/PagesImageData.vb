Imports System.Globalization
Imports System.Runtime.Serialization
Imports ARCO.Entities.Enums

Namespace Models
    Public Class PagesImageData
        <DataMember> Public Property ID As String
        <DataMember> Public Property PagesVCode As Integer
        <DataMember> Public Property Name As String
        <DataMember> Public Property EntryDate As DateTime
        <DataMember> Public Property LastModifiedDate As DateTime
        <DataMember> Public Property EntryUserVCode As Integer
        <DataMember> Public Property LastModifiedUserVCode As Integer
        <DataMember> Public ReadOnly Property ImagePath As String
            Get
                Return "/Uploads/" & Name
            End Get
        End Property

    End Class
End Namespace