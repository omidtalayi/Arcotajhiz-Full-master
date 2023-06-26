Imports System.Globalization
Imports System.Runtime.Serialization
Imports ARCO.Entities.Enums

Namespace Models
    Public Class FAQData
        <DataMember> Public Property Row As Integer
        <DataMember> Public Property VCode As Integer
        <DataMember> Public Property Question As String
        <DataMember> Public Property Answer As String
        <DataMember> Public Property Link As String
        <DataMember> Public Property ImageLink As String
        <DataMember> Public Property EntryDate As DateTime
        <DataMember> Public Property LastModifiedDate As DateTime
        <DataMember> Public Property IsDeleted As Boolean
        <DataMember> Public Property TopicVCode As Integer
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

    End Class
End Namespace
