Imports System.Globalization
Imports System.Runtime.Serialization
Imports ARCO.Entities.Enums

Namespace Models

    <DataContract>
    Public Class RptPartnerPaymentConfirmationData
        <DataMember> Public Property Row As Integer
        <DataMember> Public Property TrackingCode As Long
        <DataMember> Public Property Cellphone As String
        <DataMember> Public Property NationalCode As String
        <DataMember> Public Property Amount As Decimal
        <DataMember> Public Property JDate As String
        <DataMember> Public Property ConfirmedJDate As String
        <DataMember> Public Property SaleRefID As String
        <DataMember> Public Property EntryDate As DateTime
        <DataMember> Public Property Username As String
        <DataMember> Public Property ConfirmedAmount As Decimal
        <DataMember> Public Property BankPortal As String
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
        <DataMember> Public ReadOnly Property FormattedConfirmedAmount As String
            Get
                Return ConfirmedAmount.ToString("#,#")
            End Get
        End Property
        <DataMember> Public ReadOnly Property ConfirmedJDateFormatted As String
            Get
                Try
                    Return CType(ConfirmedJDate, Integer).ToString("####/##/##")
                Catch ex As Exception
                    Return 0
                End Try
                Return 0
            End Get
        End Property
        <DataMember> Public ReadOnly Property JDateFormatted As String
            Get
                Return CType(JDate, Integer).ToString("####/##/##")
            End Get
        End Property
    End Class

End Namespace



