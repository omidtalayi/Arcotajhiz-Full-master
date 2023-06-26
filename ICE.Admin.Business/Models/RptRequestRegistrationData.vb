Imports System.Globalization
Imports System.Runtime.Serialization
Imports ARCO.Entities.Enums

Namespace Models
    <DataContract>
    Public Class RptRequestRegistrationData
        <DataMember> Public Property Row As Integer
        <DataMember> Public Property VCode As Long
        <DataMember> Public Property RequestRegistrationState As String
        <DataMember> Public Property FirmName As String
        <DataMember> Public Property Cellphone As String
        <DataMember> Public Property Email As String
        <DataMember> Public Property ContactPointFullName As String
        <DataMember> Public Property ContactPointCellphone As String
        <DataMember> Public Property RegisteredIdentificationNo As String
        <DataMember> Public Property RegistrationNo As String
        <DataMember> Public Property RequestRegistrationTypeVCode As RequestRegistrationTypeEnum
    End Class
End Namespace

