Imports System.Globalization
Imports System.Runtime.Serialization
Imports ARCO.Entities.Enums

Namespace Models

    <DataContract>
    Public Class RptGetTransferFileShebaTotalData
        <DataMember> Public Property DocumentNo As String
        <DataMember> Public Property JDate As String
        <DataMember> Public Property TotalRow As Integer
        <DataMember> Public Property TotalPrice As Decimal
        <DataMember> Public Property StateName As String
        <DataMember> Public ReadOnly Property FormattedTotalPrice As String
            Get
                Return TotalPrice.ToString("#,#")
            End Get
        End Property
        <DataMember> Public ReadOnly Property FormattedJDate As String
            Get
                Return CType(JDate, Integer).ToString("####/##/##")
            End Get
        End Property
    End Class

End Namespace



