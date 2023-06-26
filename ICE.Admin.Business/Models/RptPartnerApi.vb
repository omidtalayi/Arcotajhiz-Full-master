Imports System.Runtime.Serialization

Namespace Models
    <DataContract>
    Public Class RptPartnerApi
        <DataMember> Public Property VCode As Integer
        <DataMember> Public Property UserVCode As Long
        <DataMember> Public Property UserName As String
        <DataMember> Public Property ApiVCode As Long
        <DataMember> Public Property Name As String
        <DataMember> Public Property EnumName As String
    End Class
End Namespace


