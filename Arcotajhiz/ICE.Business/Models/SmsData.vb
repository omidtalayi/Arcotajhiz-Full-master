Imports System.ComponentModel
Imports System.Runtime.Serialization
Imports ARCO.Entities.Enums

Namespace Models

    <DataContract>
    Public Class SmsData
        <DataMember> Public Property CenterNumber As String
        <DataMember> Public Property Number As String
        <DataMember> Public Property Message As String
        <DataMember> Public Property LogVCode As Long
        <DataMember> Public Property Type As Integer = 1
        <DataMember> Public Property IdentificationVCode As Long = 0
        <DataMember> Public Property TypeVCode As SMSTypeEnum
        <DataMember> Public Property OperatorType As OperatorTypeEnum

    End Class

End Namespace
