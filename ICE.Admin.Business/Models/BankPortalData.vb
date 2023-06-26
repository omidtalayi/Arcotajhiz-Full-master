Imports System.Globalization
Imports System.Runtime.Serialization
Imports ARCO.Entities.Enums

Namespace Models

    <DataContract>
    Public Class BankPortalData
        <DataMember> Public Property VCode As Integer
        <DataMember> Public Property Code As Integer
        <DataMember> Public Property BankVCode As Integer
        <DataMember> Public Property Name As String
        <DataMember> Public Property EnumName As BankPortalEnum
        <DataMember> Public Property AccTafzil As String
        <DataMember> Public Property AccountNumber As String
    End Class

End Namespace


