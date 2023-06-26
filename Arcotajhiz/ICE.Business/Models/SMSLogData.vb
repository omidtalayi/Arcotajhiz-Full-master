Imports System.Runtime.Serialization
Imports ARCO.Entities.Enums

Namespace Models

    <DataContract>
    Public Class SMSLogData
        <DataMember> Public Property VCode As Long
        <DataMember> Public Property Send_ID As String
        <DataMember> Public Property FromNumber As String
        <DataMember> Public Property ToNumber As String
        <DataMember> Public Property Message As String
        <DataMember> Public Property State As Integer
        <DataMember> Public Property EntryDate As DateTime
        <DataMember> Public Property SendStateMessage As String
        <DataMember> Public Property IdentificationVCode As Long
        <DataMember> Public Property SMSLogTypeVCode As SMSTypeEnum
    End Class

End Namespace
