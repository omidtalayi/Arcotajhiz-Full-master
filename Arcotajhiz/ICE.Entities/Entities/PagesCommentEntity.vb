Imports System.Globalization
Imports System.Runtime.Serialization
Imports ARCO.Entities.Enums

Namespace Models
    Public Class PagesCommentEntity
        <DataMember> Public Property VCode As Long
        <DataMember> Public Property ParentVCode As Long
        <DataMember> Public Property PagesVCode As Integer
        <DataMember> Public Property ApprovalStateVCode As ApprovalStateEnum
        <DataMember> Public Property Name As String
        <DataMember> Public Property Message As String
        <DataMember> Public Property Email As String
        <DataMember> Public Property Website As String
        <DataMember> Public Property Cellphone As String
        <DataMember> Public Property EntryDate As DateTime
        <DataMember> Public Property LastModifiedDate As DateTime
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

