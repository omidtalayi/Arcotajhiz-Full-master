Imports System.Globalization
Imports System.Runtime.Serialization
Imports ARCO.Entities.Enums

Namespace Models

    <DataContract>
    Public Class UserLastSevenDaysSeenData
        <DataMember> Public Property ReportDate As DateTime
        <DataMember> Public Property Cnt As Integer
        <DataMember> Public ReadOnly Property FormattedReportDate As String
            Get
                Dim p = New PersianCalendar()
                Return p.GetYear(ReportDate).ToString() & "/" & p.GetMonth(ReportDate).ToString().PadLeft(2, "0") & "/" & p.GetDayOfMonth(ReportDate).ToString().PadLeft(2, "0")
            End Get
        End Property
    End Class

End Namespace

