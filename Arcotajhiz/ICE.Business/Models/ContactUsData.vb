Imports System.Runtime.Serialization
Imports ARCO.Entities.Enums

Namespace Models
    <DataContract>
    Public Class ContactUsData
        <DataMember> Public Property VCode As Integer
        <DataMember> Public Property Title As String
        <DataMember> Public Property FullName As String
        <DataMember> Public Property NationalCode As String
        <DataMember> Public Property Email As String
        <DataMember> Public Property CellPhone As String
        <DataMember> Public Property Description As String
        <DataMember> Public Property EntryDate As DateTime
        <DataMember> Public Property LastModifiedDate As DateTime
        <DataMember> Public Property AdminDescription As String
        <DataMember> Public Property LastModifiedUserVCode As Integer
    End Class
End Namespace