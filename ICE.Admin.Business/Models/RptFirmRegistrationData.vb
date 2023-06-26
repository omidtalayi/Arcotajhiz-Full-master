Imports System.Globalization
Imports System.Runtime.Serialization
Imports ARCO.Entities.Enums

Namespace Models
    <DataContract>
    Public Class RptFirmRegistrationData
        <DataMember> Public Property Row As Integer
        <DataMember> Public Property VCode As Long
        <DataMember> Public Property FirmRegisrationStateName As String
        <DataMember> Public Property FirmName As String
        <DataMember> Public Property RegisteredIdentificationNo As Long
        <DataMember> Public Property RegistrationNo As Long
        <DataMember> Public Property Email As String
        <DataMember> Public Property Cellphone As String
        <DataMember> Public Property ContactPointFullName As String
        <DataMember> Public Property ContactPointCellphone As String
        <DataMember> Public Property ConfirmedJDate As String
        <DataMember> Public Property ConfirmedJDateTime As String
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
        <DataMember> Public ReadOnly Property ConfirmedJDateFormatted As String
            Get
                Return CType(ConfirmedJDate, Integer).ToString("####/##/##")
            End Get
        End Property
    End Class
End Namespace

