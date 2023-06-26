Imports System.Globalization
Imports System.Runtime.Serialization
Imports ARCO.Entities.Enums

Namespace Models

    <DataContract>
    Public Class RptGetTransferFileShebaData
        <DataMember> Public Property TransferFileShebaVCode As Long
        <DataMember> Public Property Row As Integer
        <DataMember> Public Property Amount As Decimal
        <DataMember> Public Property Description As String
        <DataMember> Public Property JDate As String
        <DataMember> Public Property PersonageName As String
        <DataMember> Public Property ShebaNumber As String
        <DataMember> Public Property CompanyAccount As String
        <DataMember> Public Property DocumentNo As String
        <DataMember> Public Property ClearingNumber As String
        <DataMember> Public ReadOnly Property FormattedAmount As String
            Get
                Return Amount.ToString("#,#")
            End Get
        End Property
    End Class

End Namespace



