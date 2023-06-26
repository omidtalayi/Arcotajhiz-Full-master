Imports System.Runtime.Serialization
Imports ARCO.Entities.Enums

Namespace Models

    <DataContract>
    Public Class BankBailData
        <DataMember> Public Property VCode As Integer
        <DataMember> Public Property ContractVCode As Long
        <DataMember> Public Property BankBailType As BankBailTypeData
        <DataMember> Public Property Amount As Decimal
        <DataMember> Public Property EntryDate As DateTime
        <DataMember> Public Property LastModifiedDate As DateTime
    End Class

End Namespace

