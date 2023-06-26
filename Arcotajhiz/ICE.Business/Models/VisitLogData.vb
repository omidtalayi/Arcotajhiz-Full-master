Imports System.Runtime.Serialization

Namespace Models
    <DataContract>
    Public Class VisitLogData
        <DataMember> Public Property VCode As Long
        <DataMember> Public Property Code As Guid
        <DataMember> Public Property Url As String
        <DataMember> Public Property IP As String
        <DataMember> Public Property Browser As String
        <DataMember> Public Property ReferrerUrl As String
        <DataMember> Public Property UserVCode As Integer = 0
        <DataMember> Public Property Guid As Guid
        <DataMember> Public Property RegisterDate As String = Nothing
        <DataMember> Public Property LoadDate As String = Nothing
        <DataMember> Public Property DropDate As String = Nothing
        <DataMember> Public Property DeviceTypeVCode As Integer
        <DataMember> Public Property IsAjaxRequest As Boolean
        <DataMember> Public Property PageTitle As String = Nothing
        <DataMember> Public Property PostData As String
        <DataMember> Public Property IpApiCom As String
        <DataMember> Public Property RequestCookies As String
    End Class
End Namespace