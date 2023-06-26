Imports System.Globalization
Imports System.Runtime.Serialization
Imports ARCO.Entities.Enums

Namespace Models

    <DataContract>
    Public Class RptOnlinePaymentConfirmationCreditData
        <DataMember> Public Property Row As Integer
        <DataMember> Public Property Username As String
        <DataMember> Public Property Name As String
        <DataMember> Public Property CardPAN As String
        <DataMember> Public Property Amount As Decimal
        <DataMember> Public Property BankPortalName As String
        <DataMember> Public Property ConfirmedJDate As String
        <DataMember> Public Property RefID As String
        <DataMember> Public Property APIPaymentCharge As Boolean
        <DataMember> Public Property EntryDate As DateTime
        <DataMember> Public ReadOnly Property FormattedJDate As String
            Get
                Dim p = New PersianCalendar()
                If EntryDate = DateTime.MinValue Then
                    Return ""
                End If
                Return p.GetYear(EntryDate).ToString() & "/" & p.GetMonth(EntryDate).ToString().PadLeft(2, "0") & "/" & p.GetDayOfMonth(EntryDate).ToString().PadLeft(2, "0")
            End Get
        End Property
        <DataMember> Public ReadOnly Property FormattedTime As String
            Get
                Dim p = New PersianCalendar()
                Return p.GetHour(EntryDate).ToString().PadLeft(2, "0") & ":" & p.GetMinute(EntryDate).ToString().PadLeft(2, "0")
            End Get
        End Property
        <DataMember> Public ReadOnly Property FormattedAmount As String
            Get
                Return Amount.ToString("#,#")
            End Get
        End Property
        <DataMember> Public ReadOnly Property ConfirmedJDateFormatted As String
            Get
                Return CType(ConfirmedJDate, Integer).ToString("####/##/##")
            End Get
        End Property
    End Class

End Namespace


