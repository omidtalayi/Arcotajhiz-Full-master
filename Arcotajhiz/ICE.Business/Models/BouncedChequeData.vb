Imports System.ComponentModel
Imports System.Runtime.Serialization

Namespace Models
    <DataContract>
    Public Class BouncedChequeData

        <DataMember> Public VCode As Integer
        <DataMember> Public ID As Integer
        <DataMember> Public AccountNumber As String
        <DataMember> Public Amount As Decimal
        <DataMember> Public BackDate As Integer
        <DataMember> Public BankCode As String
        <DataMember> Public BranchCode As String
        <DataMember> Public BranchDescription As String
        <DataMember> Public [Date] As Integer
        <DataMember> Public Number As String

    End Class

End Namespace


