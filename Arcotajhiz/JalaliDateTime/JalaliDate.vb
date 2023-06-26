Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Imports Microsoft.SqlServer.Server

Partial Public Class JalaliDate
    <Microsoft.SqlServer.Server.SqlFunction(DataAccess:=DataAccessKind.None)>
    Public Shared Function ConvertToJalaliDate(<SqlFacet(MaxSize:=0)> ByVal GDate As Nullable(Of DateTime)) As <SqlFacet(MaxSize:=8)> SqlChars
        If IsNothing(GDate) OrElse IsDBNull(GDate) Then Return SqlChars.Null
        If GDate.Value.Year < 622 OrElse (GDate.Value.Year = 622 AndAlso GDate.Value.Month < 3) OrElse (GDate.Value.Year = 622 AndAlso GDate.Value.Month = 3 AndAlso GDate.Value.Day < 21) Then Return SqlChars.Null
        Dim m_Calendar As New Globalization.PersianCalendar
        Return New SqlChars(String.Format("{0}{1}{2}", m_Calendar.GetYear(GDate.Value).ToString.PadLeft(4, "0"c), m_Calendar.GetMonth(GDate.Value).ToString.PadLeft(2, "0"c), m_Calendar.GetDayOfMonth(GDate.Value).ToString.PadLeft(2, "0"c)))
    End Function

    <Microsoft.SqlServer.Server.SqlFunction(DataAccess:=DataAccessKind.None)>
    Public Shared Function ConvertFromJalaliDate(<SqlFacet(MaxSize:=8)> ByVal JDate As SqlChars) As <SqlFacet(MaxSize:=0)> Nullable(Of DateTime)
        If IsNothing(JDate) OrElse IsDBNull(JDate) OrElse Not IsNumeric(JDate.ToSqlString.Value.Trim) OrElse JDate.ToSqlString.Value.Trim.Length <> 8 Then Return Nothing
        Dim m_PersianYear As Short, m_PersianMonth As Byte, m_PersianDay As Byte
        m_PersianYear = CType(JDate.ToSqlString.Value.Substring(0, 4), Short)
        m_PersianMonth = CType(JDate.ToSqlString.Value.Substring(4, 2), Byte)
        m_PersianDay = CType(JDate.ToSqlString.Value.Substring(6, 2), Byte)
        If m_PersianYear > 9378 OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth > 10) OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth = 10 AndAlso m_PersianDay > 10) Then Return Nothing
        Dim m_Calendar As New Globalization.PersianCalendar
        Return m_Calendar.ToDateTime(m_PersianYear, m_PersianMonth, m_PersianDay, 0, 0, 0, 0)
    End Function

    <Microsoft.SqlServer.Server.SqlFunction()>
    Public Shared Function Today() As <SqlFacet(MaxSize:=8)> SqlChars
        Dim m_Calendar As New Globalization.PersianCalendar, m_Date As Date = Date.Now
        Return New SqlChars(String.Format("{0}{1}{2}", m_Calendar.GetYear(m_Date).ToString.PadLeft(4, "0"c), m_Calendar.GetMonth(m_Date).ToString.PadLeft(2, "0"c), m_Calendar.GetDayOfMonth(m_Date).ToString.PadLeft(2, "0"c)))
    End Function

    <Microsoft.SqlServer.Server.SqlFunction()>
    Public Shared Function GetDayOfWeek(<SqlFacet(MaxSize:=8)> ByVal JDate As SqlChars) As SqlInt16
        If IsNothing(JDate) OrElse IsDBNull(JDate) OrElse Not IsNumeric(JDate.ToSqlString.Value.Trim) OrElse JDate.ToSqlString.Value.Trim.Length <> 8 Then Return SqlInt16.Null
        Dim m_PersianYear As Short, m_PersianMonth As Byte, m_PersianDay As Byte
        m_PersianYear = CType(JDate.ToSqlString.Value.Substring(0, 4), Short)
        m_PersianMonth = CType(JDate.ToSqlString.Value.Substring(4, 2), Byte)
        m_PersianDay = CType(JDate.ToSqlString.Value.Substring(6, 2), Byte)
        If m_PersianYear > 9378 OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth > 10) OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth = 10 AndAlso m_PersianDay > 10) Then Return SqlInt16.Null
        Dim m_Calendar As New Globalization.PersianCalendar, m_Date As Date = m_Calendar.ToDateTime(m_PersianYear, m_PersianMonth, m_PersianDay, 0, 0, 0, 0)
        Select Case m_Calendar.GetDayOfWeek(m_Date)
            Case DayOfWeek.Saturday
                Return New SqlInt16(1)
            Case DayOfWeek.Sunday
                Return New SqlInt16(2)
            Case DayOfWeek.Monday
                Return New SqlInt16(3)
            Case DayOfWeek.Tuesday
                Return New SqlInt16(4)
            Case DayOfWeek.Wednesday
                Return New SqlInt16(5)
            Case DayOfWeek.Thursday
                Return New SqlInt16(6)
            Case DayOfWeek.Friday
                Return New SqlInt16(7)
        End Select
    End Function

    <Microsoft.SqlServer.Server.SqlFunction()>
    Public Shared Function GetDayOfWeekName(ByVal WeekDay As SqlInt16) As <SqlFacet(MaxSize:=11)> SqlString
        If IsNothing(WeekDay) OrElse IsDBNull(WeekDay) Then Return SqlString.Null
        Select Case WeekDay.Value
            Case 1
                Return New SqlString("‌شنبه‌")
            Case 2
                Return New SqlString("‌یک‌شنبه‌")
            Case 3
                Return New SqlString("‌دو‌شنبه‌")
            Case 4
                Return New SqlString("‌سه‌شنبه‌")
            Case 5
                Return New SqlString("‌چهار‌شنبه‌")
            Case 6
                Return New SqlString("‌پنج‌شنبه‌")
            Case 7
                Return New SqlString("‌جمعه‌")
            Case Else
                Return SqlString.Null
        End Select
    End Function

    <Microsoft.SqlServer.Server.SqlFunction()>
    Public Shared Function GetDayOfMonth(<SqlFacet(MaxSize:=8)> ByVal JDate As SqlChars) As SqlInt16
        If IsNothing(JDate) OrElse IsDBNull(JDate) OrElse Not IsNumeric(JDate.ToSqlString.Value.Trim) OrElse JDate.ToSqlString.Value.Trim.Length <> 8 Then Return SqlInt16.Null
        Dim m_PersianYear As Short, m_PersianMonth As Byte, m_PersianDay As Byte
        m_PersianYear = CType(JDate.ToSqlString.Value.Substring(0, 4), Short)
        m_PersianMonth = CType(JDate.ToSqlString.Value.Substring(4, 2), Byte)
        m_PersianDay = CType(JDate.ToSqlString.Value.Substring(6, 2), Byte)
        If m_PersianYear > 9378 OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth > 10) OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth = 10 AndAlso m_PersianDay > 10) Then Return SqlInt16.Null
        Return New SqlInt16(CType(m_PersianDay, Short))
    End Function

    <Microsoft.SqlServer.Server.SqlFunction()>
    Public Shared Function GetDaysInMonth(<SqlFacet(MaxSize:=8)> ByVal JDate As SqlChars) As SqlInt16
        If IsNothing(JDate) OrElse IsDBNull(JDate) OrElse Not IsNumeric(JDate.ToSqlString.Value.Trim) OrElse JDate.ToSqlString.Value.Trim.Length <> 8 Then Return SqlInt16.Null
        Dim m_PersianYear As Short, m_PersianMonth As Byte, m_PersianDay As Byte
        m_PersianYear = CType(JDate.ToSqlString.Value.Substring(0, 4), Short)
        m_PersianMonth = CType(JDate.ToSqlString.Value.Substring(4, 2), Byte)
        m_PersianDay = CType(JDate.ToSqlString.Value.Substring(6, 2), Byte)
        If m_PersianYear > 9378 OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth > 10) OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth = 10 AndAlso m_PersianDay > 10) Then Return SqlInt16.Null
        Dim m_Calendar As New Globalization.PersianCalendar
        Return New SqlInt16(CType(m_Calendar.GetDaysInMonth(m_PersianYear, m_PersianMonth), Short))
    End Function

    <Microsoft.SqlServer.Server.SqlFunction()>
    Public Shared Function GetDayOfQuarter(<SqlFacet(MaxSize:=8)> ByVal JDate As SqlChars) As SqlInt16
        If IsNothing(JDate) OrElse IsDBNull(JDate) OrElse Not IsNumeric(JDate.ToSqlString.Value.Trim) OrElse JDate.ToSqlString.Value.Trim.Length <> 8 Then Return SqlInt16.Null
        Dim m_PersianYear As Short, m_PersianMonth As Byte, m_PersianDay As Short
        m_PersianYear = CType(JDate.ToSqlString.Value.Substring(0, 4), Short)
        m_PersianMonth = CType(JDate.ToSqlString.Value.Substring(4, 2), Byte)
        m_PersianDay = CType(JDate.ToSqlString.Value.Substring(6, 2), Short)
        If m_PersianYear > 9378 OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth > 10) OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth = 10 AndAlso m_PersianDay > 10) Then Return SqlInt16.Null
        Dim m_Calendar As New Globalization.PersianCalendar
        While m_PersianMonth Mod 3 <> 1
            m_PersianMonth -= CByte(1)
            m_PersianDay += CShort(m_Calendar.GetDaysInMonth(m_PersianYear, m_PersianMonth))
        End While
        Return New SqlInt16(CShort(m_PersianDay))
    End Function

    <Microsoft.SqlServer.Server.SqlFunction()>
    Public Shared Function GetDaysInQuarter(<SqlFacet(MaxSize:=8)> ByVal JDate As SqlChars) As SqlInt16
        If IsNothing(JDate) OrElse IsDBNull(JDate) OrElse Not IsNumeric(JDate.ToSqlString.Value.Trim) OrElse JDate.ToSqlString.Value.Trim.Length <> 8 Then Return SqlInt16.Null
        Dim m_PersianYear As Short, m_PersianMonth As Byte, m_PersianDay As Short
        m_PersianYear = CType(JDate.ToSqlString.Value.Substring(0, 4), Short)
        m_PersianMonth = CType(JDate.ToSqlString.Value.Substring(4, 2), Byte)
        m_PersianDay = CType(JDate.ToSqlString.Value.Substring(6, 2), Short)
        If m_PersianYear > 9378 OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth > 10) OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth = 10 AndAlso m_PersianDay > 10) Then Return SqlInt16.Null
        Dim m_Calendar As New Globalization.PersianCalendar
        Select Case m_PersianMonth
            Case 1, 2, 3, 4, 5, 6
                Return New SqlInt16(93)
            Case 7, 8, 9
                Return New SqlInt16(90)
            Case 10, 11, 12
                Return If(m_Calendar.IsLeapYear(m_PersianYear), New SqlInt16(90), New SqlInt16(89))
        End Select
    End Function

    <Microsoft.SqlServer.Server.SqlFunction()>
    Public Shared Function GetDayOfYear(<SqlFacet(MaxSize:=8)> ByVal JDate As SqlChars) As SqlInt16
        If IsNothing(JDate) OrElse IsDBNull(JDate) OrElse Not IsNumeric(JDate.ToSqlString.Value.Trim) OrElse JDate.ToSqlString.Value.Trim.Length <> 8 Then Return SqlInt16.Null
        Dim m_PersianYear As Short, m_PersianMonth As Byte, m_PersianDay As Byte
        m_PersianYear = CType(JDate.ToSqlString.Value.Substring(0, 4), Short)
        m_PersianMonth = CType(JDate.ToSqlString.Value.Substring(4, 2), Byte)
        m_PersianDay = CType(JDate.ToSqlString.Value.Substring(6, 2), Byte)
        If m_PersianYear > 9378 OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth > 10) OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth = 10 AndAlso m_PersianDay > 10) Then Return SqlInt16.Null
        Dim m_Calendar As New Globalization.PersianCalendar, m_Date As Date = m_Calendar.ToDateTime(m_PersianYear, m_PersianMonth, m_PersianDay, 0, 0, 0, 0)
        Return New SqlInt16(CType(m_Calendar.GetDayOfYear(m_Date), Short))
    End Function

    <Microsoft.SqlServer.Server.SqlFunction()>
    Public Shared Function GetDaysInYear(<SqlFacet(MaxSize:=8)> ByVal JDate As SqlChars) As SqlInt16
        If IsNothing(JDate) OrElse IsDBNull(JDate) OrElse Not IsNumeric(JDate.ToSqlString.Value.Trim) OrElse JDate.ToSqlString.Value.Trim.Length <> 8 Then Return SqlInt16.Null
        Dim m_PersianYear As Short, m_PersianMonth As Byte, m_PersianDay As Byte
        m_PersianYear = CType(JDate.ToSqlString.Value.Substring(0, 4), Short)
        m_PersianMonth = CType(JDate.ToSqlString.Value.Substring(4, 2), Byte)
        m_PersianDay = CType(JDate.ToSqlString.Value.Substring(6, 2), Byte)
        If m_PersianYear > 9378 OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth > 10) OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth = 10 AndAlso m_PersianDay > 10) Then Return SqlInt16.Null
        Dim m_Calendar As New Globalization.PersianCalendar
        Return New SqlInt16(CType(m_Calendar.GetDaysInYear(m_PersianYear), Short))
    End Function

    <Microsoft.SqlServer.Server.SqlFunction()>
    Public Shared Function GetWeekOfMonth(<SqlFacet(MaxSize:=8)> ByVal JDate As SqlChars) As SqlInt16
        If IsNothing(JDate) OrElse IsDBNull(JDate) OrElse Not IsNumeric(JDate.ToSqlString.Value.Trim) OrElse JDate.ToSqlString.Value.Trim.Length <> 8 Then Return SqlInt16.Null
        Dim m_PersianYear As Short, m_PersianMonth As Byte, m_PersianDay As Short
        m_PersianYear = CType(JDate.ToSqlString.Value.Substring(0, 4), Short)
        m_PersianMonth = CType(JDate.ToSqlString.Value.Substring(4, 2), Byte)
        m_PersianDay = CType(JDate.ToSqlString.Value.Substring(6, 2), Short)
        If m_PersianYear > 9378 OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth > 10) OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth = 10 AndAlso m_PersianDay > 10) Then Return SqlInt16.Null
        Dim m_Calendar As New Globalization.PersianCalendar, m_FirstWeekDayOfMonth As Byte = 0
        Dim m_Date As Date = m_Calendar.ToDateTime(m_PersianYear, m_PersianMonth, m_PersianDay, 0, 0, 0, 0)
        Select Case m_Calendar.GetDayOfWeek(m_Date)
            Case DayOfWeek.Saturday
                m_FirstWeekDayOfMonth = 1
            Case DayOfWeek.Sunday
                m_FirstWeekDayOfMonth = 2
            Case DayOfWeek.Monday
                m_FirstWeekDayOfMonth = 3
            Case DayOfWeek.Tuesday
                m_FirstWeekDayOfMonth = 4
            Case DayOfWeek.Wednesday
                m_FirstWeekDayOfMonth = 5
            Case DayOfWeek.Thursday
                m_FirstWeekDayOfMonth = 6
            Case DayOfWeek.Friday
                m_FirstWeekDayOfMonth = 7
        End Select
        m_PersianDay += CShort(7) - m_FirstWeekDayOfMonth
        Return New SqlInt16((m_PersianDay \ CShort(7)) + CShort(1))
    End Function

    <Microsoft.SqlServer.Server.SqlFunction()>
    Public Shared Function GetWeeksInMonth(<SqlFacet(MaxSize:=8)> ByVal JDate As SqlChars) As SqlInt16
        If IsNothing(JDate) OrElse IsDBNull(JDate) OrElse Not IsNumeric(JDate.ToSqlString.Value.Trim) OrElse JDate.ToSqlString.Value.Trim.Length <> 8 Then Return SqlInt16.Null
        Dim m_PersianYear As Short, m_PersianMonth As Byte, m_PersianDay As Short
        m_PersianYear = CType(JDate.ToSqlString.Value.Substring(0, 4), Short)
        m_PersianMonth = CType(JDate.ToSqlString.Value.Substring(4, 2), Byte)
        m_PersianDay = CType(JDate.ToSqlString.Value.Substring(6, 2), Short)
        If m_PersianYear > 9378 OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth > 10) OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth = 10 AndAlso m_PersianDay > 10) Then Return SqlInt16.Null
        Dim m_Calendar As New Globalization.PersianCalendar, m_FirstWeekDayOfMonth As Byte = 0
        m_PersianDay = CShort(m_Calendar.GetDaysInMonth(m_PersianYear, m_PersianMonth))
        Dim m_Date As Date = m_Calendar.ToDateTime(m_PersianYear, m_PersianMonth, 1, 0, 0, 0, 0)
        Select Case m_Calendar.GetDayOfWeek(m_Date)
            Case DayOfWeek.Saturday
                m_FirstWeekDayOfMonth = 1
            Case DayOfWeek.Sunday
                m_FirstWeekDayOfMonth = 2
            Case DayOfWeek.Monday
                m_FirstWeekDayOfMonth = 3
            Case DayOfWeek.Tuesday
                m_FirstWeekDayOfMonth = 4
            Case DayOfWeek.Wednesday
                m_FirstWeekDayOfMonth = 5
            Case DayOfWeek.Thursday
                m_FirstWeekDayOfMonth = 6
            Case DayOfWeek.Friday
                m_FirstWeekDayOfMonth = 7
        End Select
        m_PersianDay += m_FirstWeekDayOfMonth - CShort(1)
        Return New SqlInt16(CShort(Math.Ceiling((m_PersianDay / CShort(7)))))
    End Function

    <Microsoft.SqlServer.Server.SqlFunction()>
    Public Shared Function GetWeekOfQuarter(<SqlFacet(MaxSize:=8)> ByVal JDate As SqlChars) As SqlInt16
        If IsNothing(JDate) OrElse IsDBNull(JDate) OrElse Not IsNumeric(JDate.ToSqlString.Value.Trim) OrElse JDate.ToSqlString.Value.Trim.Length <> 8 Then Return SqlInt16.Null
        Dim m_PersianYear As Short, m_PersianMonth As Byte, m_PersianDay As Short
        m_PersianYear = CType(JDate.ToSqlString.Value.Substring(0, 4), Short)
        m_PersianMonth = CType(JDate.ToSqlString.Value.Substring(4, 2), Byte)
        m_PersianDay = CType(JDate.ToSqlString.Value.Substring(6, 2), Short)
        If m_PersianYear > 9378 OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth > 10) OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth = 10 AndAlso m_PersianDay > 10) Then Return SqlInt16.Null
        Dim m_Calendar As New Globalization.PersianCalendar, m_FirstWeekDayOfMonth As Byte = 0
        Dim m_QuarterDay As SqlInt16 = GetDayOfQuarter(JDate)
        If m_QuarterDay.IsNull Then Return SqlInt16.Null
        Select Case m_PersianMonth
            Case 1, 2, 3
                m_PersianMonth = 1
            Case 4, 5, 6
                m_PersianMonth = 4
            Case 7, 8, 9
                m_PersianMonth = 7
            Case 10, 11, 12
                m_PersianMonth = 10
        End Select
        Dim m_Date As Date = m_Calendar.ToDateTime(m_PersianYear, m_PersianMonth, 1, 0, 0, 0, 0)
        Select Case m_Calendar.GetDayOfWeek(m_Date)
            Case DayOfWeek.Saturday
                m_FirstWeekDayOfMonth = 1
            Case DayOfWeek.Sunday
                m_FirstWeekDayOfMonth = 2
            Case DayOfWeek.Monday
                m_FirstWeekDayOfMonth = 3
            Case DayOfWeek.Tuesday
                m_FirstWeekDayOfMonth = 4
            Case DayOfWeek.Wednesday
                m_FirstWeekDayOfMonth = 5
            Case DayOfWeek.Thursday
                m_FirstWeekDayOfMonth = 6
            Case DayOfWeek.Friday
                m_FirstWeekDayOfMonth = 7
        End Select
        m_PersianDay = m_QuarterDay.Value + m_FirstWeekDayOfMonth - CShort(1)
        Return New SqlInt16(CShort(Math.Ceiling((m_PersianDay / CShort(7)))))
    End Function

    <Microsoft.SqlServer.Server.SqlFunction()>
    Public Shared Function GetWeeksInQuarter(<SqlFacet(MaxSize:=8)> ByVal JDate As SqlChars) As SqlInt16
        If IsNothing(JDate) OrElse IsDBNull(JDate) OrElse Not IsNumeric(JDate.ToSqlString.Value.Trim) OrElse JDate.ToSqlString.Value.Trim.Length <> 8 Then Return SqlInt16.Null
        Dim m_PersianYear As Short, m_PersianMonth As Byte, m_PersianDay As Short
        m_PersianYear = CType(JDate.ToSqlString.Value.Substring(0, 4), Short)
        m_PersianMonth = CType(JDate.ToSqlString.Value.Substring(4, 2), Byte)
        m_PersianDay = CType(JDate.ToSqlString.Value.Substring(6, 2), Short)
        If m_PersianYear > 9378 OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth > 10) OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth = 10 AndAlso m_PersianDay > 10) Then Return SqlInt16.Null
        Dim m_Calendar As New Globalization.PersianCalendar, m_FirstWeekDayOfMonth As Byte = 0
        Dim m_QuarterDays As SqlInt16 = GetDaysInQuarter(JDate)
        If m_QuarterDays.IsNull Then Return SqlInt16.Null
        Select Case m_PersianMonth
            Case 1, 2, 3
                m_PersianMonth = 1
            Case 4, 5, 6
                m_PersianMonth = 4
            Case 7, 8, 9
                m_PersianMonth = 7
            Case 10, 11, 12
                m_PersianMonth = 10
        End Select
        Dim m_Date As Date = m_Calendar.ToDateTime(m_PersianYear, m_PersianMonth, 1, 0, 0, 0, 0)
        Select Case m_Calendar.GetDayOfWeek(m_Date)
            Case DayOfWeek.Saturday
                m_FirstWeekDayOfMonth = 1
            Case DayOfWeek.Sunday
                m_FirstWeekDayOfMonth = 2
            Case DayOfWeek.Monday
                m_FirstWeekDayOfMonth = 3
            Case DayOfWeek.Tuesday
                m_FirstWeekDayOfMonth = 4
            Case DayOfWeek.Wednesday
                m_FirstWeekDayOfMonth = 5
            Case DayOfWeek.Thursday
                m_FirstWeekDayOfMonth = 6
            Case DayOfWeek.Friday
                m_FirstWeekDayOfMonth = 7
        End Select
        m_PersianDay = m_QuarterDays.Value + m_FirstWeekDayOfMonth - CShort(1)
        Return New SqlInt16(CShort(Math.Ceiling((m_PersianDay / CShort(7)))))
    End Function

    <Microsoft.SqlServer.Server.SqlFunction()>
    Public Shared Function GetWeekOfYear(<SqlFacet(MaxSize:=8)> ByVal JDate As SqlChars) As SqlInt16
        If IsNothing(JDate) OrElse IsDBNull(JDate) OrElse Not IsNumeric(JDate.ToSqlString.Value.Trim) OrElse JDate.ToSqlString.Value.Trim.Length <> 8 Then Return SqlInt16.Null
        Dim m_PersianYear As Short, m_PersianMonth As Byte, m_PersianDay As Short
        m_PersianYear = CType(JDate.ToSqlString.Value.Substring(0, 4), Short)
        m_PersianMonth = CType(JDate.ToSqlString.Value.Substring(4, 2), Byte)
        m_PersianDay = CType(JDate.ToSqlString.Value.Substring(6, 2), Short)
        If m_PersianYear > 9378 OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth > 10) OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth = 10 AndAlso m_PersianDay > 10) Then Return SqlInt16.Null
        Dim m_Calendar As New Globalization.PersianCalendar, m_Date As Byte = 0
        Return New SqlInt16(CShort(m_Calendar.GetWeekOfYear(m_Calendar.ToDateTime(m_PersianYear, m_PersianMonth, m_PersianDay, 0, 0, 0, 0), Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Saturday)))
    End Function

    <Microsoft.SqlServer.Server.SqlFunction()>
    Public Shared Function GetWeeksInYear(<SqlFacet(MaxSize:=8)> ByVal JDate As SqlChars) As SqlInt16
        If IsNothing(JDate) OrElse IsDBNull(JDate) OrElse Not IsNumeric(JDate.ToSqlString.Value.Trim) OrElse JDate.ToSqlString.Value.Trim.Length <> 8 Then Return SqlInt16.Null
        Dim m_PersianYear As Short, m_PersianMonth As Byte, m_PersianDay As Short
        m_PersianYear = CType(JDate.ToSqlString.Value.Substring(0, 4), Short)
        m_PersianMonth = CType(JDate.ToSqlString.Value.Substring(4, 2), Byte)
        m_PersianDay = CType(JDate.ToSqlString.Value.Substring(6, 2), Short)
        If m_PersianYear > 9378 OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth > 10) OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth = 10 AndAlso m_PersianDay > 10) Then Return SqlInt16.Null
        Dim m_Calendar As New Globalization.PersianCalendar, m_FirstWeekDayOfMonth As Byte = 0
        m_PersianDay = CShort(m_Calendar.GetDaysInMonth(m_PersianYear, 12))
        Return New SqlInt16(CShort(m_Calendar.GetWeekOfYear(m_Calendar.ToDateTime(m_PersianYear, 12, m_PersianDay, 0, 0, 0, 0), Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Saturday)))
    End Function

    <Microsoft.SqlServer.Server.SqlFunction()>
    Public Shared Function GetMonth(<SqlFacet(MaxSize:=8)> ByVal JDate As SqlChars) As SqlInt16
        If IsNothing(JDate) OrElse IsDBNull(JDate) OrElse Not IsNumeric(JDate.ToSqlString.Value.Trim) OrElse JDate.ToSqlString.Value.Trim.Length <> 8 Then Return SqlInt16.Null
        Dim m_PersianYear As Short, m_PersianMonth As Byte, m_PersianDay As Byte
        m_PersianYear = CType(JDate.ToSqlString.Value.Substring(0, 4), Short)
        m_PersianMonth = CType(JDate.ToSqlString.Value.Substring(4, 2), Byte)
        m_PersianDay = CType(JDate.ToSqlString.Value.Substring(6, 2), Byte)
        If m_PersianYear > 9378 OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth > 10) OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth = 10 AndAlso m_PersianDay > 10) Then Return SqlInt16.Null
        Dim m_Calendar As New Globalization.PersianCalendar, m_Date As Date = m_Calendar.ToDateTime(m_PersianYear, m_PersianMonth, m_PersianDay, 0, 0, 0, 0)
        Return New SqlInt16(CType(m_PersianMonth, Short))
    End Function

    <Microsoft.SqlServer.Server.SqlFunction()>
    Public Shared Function GetMonthName(ByVal Month As SqlInt16) As <SqlFacet(MaxSize:=10)> SqlString
        If IsNothing(Month) OrElse IsDBNull(Month) Then Return SqlString.Null
        Select Case Month.Value
            Case 1
                Return New SqlString("‌فروردین‌")
            Case 2
                Return New SqlString("‌اردیبهشت‌")
            Case 3
                Return New SqlString("‌خرداد‌")
            Case 4
                Return New SqlString("‌تیر‌")
            Case 5
                Return New SqlString("‌مرداد‌")
            Case 6
                Return New SqlString("‌شهریور‌")
            Case 7
                Return New SqlString("‌مهر‌")
            Case 8
                Return New SqlString("‌آبان‌")
            Case 9
                Return New SqlString("‌آذر‌")
            Case 10
                Return New SqlString("‌دی‌")
            Case 11
                Return New SqlString("‌بهمن‌")
            Case 12
                Return New SqlString("‌اسفند‌")
            Case Else
                Return SqlString.Null
        End Select
    End Function

    <Microsoft.SqlServer.Server.SqlFunction()>
    Public Shared Function GetMonthOfQuarter(<SqlFacet(MaxSize:=8)> ByVal JDate As SqlChars) As SqlInt16
        If IsNothing(JDate) OrElse IsDBNull(JDate) OrElse Not IsNumeric(JDate.ToSqlString.Value.Trim) OrElse JDate.ToSqlString.Value.Trim.Length <> 8 Then Return SqlInt16.Null
        Dim m_PersianYear As Short, m_PersianMonth As Byte, m_PersianDay As Byte
        m_PersianYear = CType(JDate.ToSqlString.Value.Substring(0, 4), Short)
        m_PersianMonth = CType(JDate.ToSqlString.Value.Substring(4, 2), Byte)
        m_PersianDay = CType(JDate.ToSqlString.Value.Substring(6, 2), Byte)
        If m_PersianYear > 9378 OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth > 10) OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth = 10 AndAlso m_PersianDay > 10) Then Return SqlInt16.Null
        Return New SqlInt16(CType(m_PersianMonth Mod 3, Short) + CShort(1))
    End Function

    <Microsoft.SqlServer.Server.SqlFunction()>
    Public Shared Function GetQuarter(<SqlFacet(MaxSize:=8)> ByVal JDate As SqlChars) As SqlInt16
        If IsNothing(JDate) OrElse IsDBNull(JDate) OrElse Not IsNumeric(JDate.ToSqlString.Value.Trim) OrElse JDate.ToSqlString.Value.Trim.Length <> 8 Then Return SqlInt16.Null
        Dim m_PersianYear As Short, m_PersianMonth As Byte, m_PersianDay As Byte
        m_PersianYear = CType(JDate.ToSqlString.Value.Substring(0, 4), Short)
        m_PersianMonth = CType(JDate.ToSqlString.Value.Substring(4, 2), Byte)
        m_PersianDay = CType(JDate.ToSqlString.Value.Substring(6, 2), Byte)
        If m_PersianYear > 9378 OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth > 10) OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth = 10 AndAlso m_PersianDay > 10) Then Return SqlInt16.Null
        Dim m_Calendar As New Globalization.PersianCalendar, m_Date As Date = m_Calendar.ToDateTime(m_PersianYear, m_PersianMonth, m_PersianDay, 0, 0, 0, 0)
        If m_PersianMonth > 9 Then Return New SqlInt16(4) Else If m_PersianMonth > 6 Then Return New SqlInt16(3) Else If m_PersianMonth > 3 Then Return New SqlInt16(2) Else Return New SqlInt16(1)
    End Function

    <Microsoft.SqlServer.Server.SqlFunction()>
    Public Shared Function GetQuarterName(ByVal Quarter As SqlInt16) As <SqlFacet(MaxSize:=9)> SqlString
        If IsNothing(Quarter) OrElse IsDBNull(Quarter) Then Return SqlString.Null
        Select Case Quarter.Value
            Case 1
                Return New SqlString("‌بهار‌")
            Case 2
                Return New SqlString("‌تابستان‌")
            Case 3
                Return New SqlString("‌پاییز‌")
            Case 4
                Return New SqlString("‌زمستان‌")
            Case Else
                Return SqlString.Null
        End Select
    End Function

    <Microsoft.SqlServer.Server.SqlFunction()>
    Public Shared Function GetYear(<SqlFacet(MaxSize:=8)> ByVal JDate As SqlChars) As SqlInt16
        If IsNothing(JDate) OrElse IsDBNull(JDate) OrElse Not IsNumeric(JDate.ToSqlString.Value.Trim) OrElse JDate.ToSqlString.Value.Trim.Length <> 8 Then Return SqlInt16.Null
        Dim m_PersianYear As Short, m_PersianMonth As Byte, m_PersianDay As Byte
        m_PersianYear = CType(JDate.ToSqlString.Value.Substring(0, 4), Short)
        m_PersianMonth = CType(JDate.ToSqlString.Value.Substring(4, 2), Byte)
        m_PersianDay = CType(JDate.ToSqlString.Value.Substring(6, 2), Byte)
        If m_PersianYear > 9378 OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth > 10) OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth = 10 AndAlso m_PersianDay > 10) Then Return SqlInt16.Null
        Return New SqlInt16(m_PersianYear)
    End Function

    <Microsoft.SqlServer.Server.SqlFunction()>
    Public Shared Function IsLeapYear(<SqlFacet(MaxSize:=8)> ByVal JDate As SqlChars) As SqlBoolean
        If IsNothing(JDate) OrElse IsDBNull(JDate) OrElse Not IsNumeric(JDate.ToSqlString.Value.Trim) OrElse JDate.ToSqlString.Value.Trim.Length <> 8 Then Return SqlBoolean.Null
        Dim m_PersianYear As Short, m_PersianMonth As Byte, m_PersianDay As Byte
        m_PersianYear = CType(JDate.ToSqlString.Value.Substring(0, 4), Short)
        m_PersianMonth = CType(JDate.ToSqlString.Value.Substring(4, 2), Byte)
        m_PersianDay = CType(JDate.ToSqlString.Value.Substring(6, 2), Byte)
        If m_PersianYear > 9378 OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth > 10) OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth = 10 AndAlso m_PersianDay > 10) Then Return SqlBoolean.Null
        Dim m_Calendar As New Globalization.PersianCalendar
        Return New SqlBoolean(m_Calendar.IsLeapYear(m_PersianYear))
    End Function

    <Microsoft.SqlServer.Server.SqlFunction()>
    Public Shared Function IsLeapQuarter(<SqlFacet(MaxSize:=8)> ByVal JDate As SqlChars) As SqlBoolean
        If IsNothing(JDate) OrElse IsDBNull(JDate) OrElse Not IsNumeric(JDate.ToSqlString.Value.Trim) OrElse JDate.ToSqlString.Value.Trim.Length <> 8 Then Return SqlBoolean.Null
        Dim m_PersianYear As Short, m_PersianMonth As Byte, m_PersianDay As Byte
        m_PersianYear = CType(JDate.ToSqlString.Value.Substring(0, 4), Short)
        m_PersianMonth = CType(JDate.ToSqlString.Value.Substring(4, 2), Byte)
        m_PersianDay = CType(JDate.ToSqlString.Value.Substring(6, 2), Byte)
        If m_PersianYear > 9378 OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth > 10) OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth = 10 AndAlso m_PersianDay > 10) Then Return SqlBoolean.Null
        Dim m_Calendar As New Globalization.PersianCalendar
        Return New SqlBoolean(m_Calendar.IsLeapYear(m_PersianYear) AndAlso m_PersianMonth > 9)
    End Function

    <Microsoft.SqlServer.Server.SqlFunction()>
    Public Shared Function IsLeapMonth(<SqlFacet(MaxSize:=8)> ByVal JDate As SqlChars) As SqlBoolean
        If IsNothing(JDate) OrElse IsDBNull(JDate) OrElse Not IsNumeric(JDate.ToSqlString.Value.Trim) OrElse JDate.ToSqlString.Value.Trim.Length <> 8 Then Return SqlBoolean.Null
        Dim m_PersianYear As Short, m_PersianMonth As Byte, m_PersianDay As Byte
        m_PersianYear = CType(JDate.ToSqlString.Value.Substring(0, 4), Short)
        m_PersianMonth = CType(JDate.ToSqlString.Value.Substring(4, 2), Byte)
        m_PersianDay = CType(JDate.ToSqlString.Value.Substring(6, 2), Byte)
        If m_PersianYear > 9378 OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth > 10) OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth = 10 AndAlso m_PersianDay > 10) Then Return SqlBoolean.Null
        Dim m_Calendar As New Globalization.PersianCalendar
        Return New SqlBoolean(m_Calendar.IsLeapYear(m_PersianYear) AndAlso m_PersianMonth = 12)
    End Function

    <Microsoft.SqlServer.Server.SqlFunction()>
    Public Shared Function IsLeapWeek(<SqlFacet(MaxSize:=8)> ByVal JDate As SqlChars) As SqlBoolean
        If IsNothing(JDate) OrElse IsDBNull(JDate) OrElse Not IsNumeric(JDate.ToSqlString.Value.Trim) OrElse JDate.ToSqlString.Value.Trim.Length <> 8 Then Return SqlBoolean.Null
        Dim m_PersianYear As Short, m_PersianMonth As Byte, m_PersianDay As Byte
        m_PersianYear = CType(JDate.ToSqlString.Value.Substring(0, 4), Short)
        m_PersianMonth = CType(JDate.ToSqlString.Value.Substring(4, 2), Byte)
        m_PersianDay = CType(JDate.ToSqlString.Value.Substring(6, 2), Byte)
        If m_PersianYear > 9378 OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth > 10) OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth = 10 AndAlso m_PersianDay > 10) Then Return SqlBoolean.Null
        Dim m_Calendar As New Globalization.PersianCalendar
        If m_Calendar.IsLeapYear(m_PersianYear) AndAlso m_Calendar.GetWeekOfYear(m_Calendar.ToDateTime(m_PersianYear, m_PersianMonth, m_PersianDay, 0, 0, 0, 0), Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Saturday) = m_Calendar.GetWeekOfYear(m_Calendar.ToDateTime(m_PersianYear, 12, 30, 0, 0, 0, 0), Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Saturday) Then
            Return New SqlBoolean(True)
        Else
            Return New SqlBoolean(False)
        End If
    End Function

    <Microsoft.SqlServer.Server.SqlFunction()>
    Public Shared Function IsLeapDay(<SqlFacet(MaxSize:=8)> ByVal JDate As SqlChars) As SqlBoolean
        If IsNothing(JDate) OrElse IsDBNull(JDate) OrElse Not IsNumeric(JDate.ToSqlString.Value.Trim) OrElse JDate.ToSqlString.Value.Trim.Length <> 8 Then Return SqlBoolean.Null
        Dim m_PersianYear As Short, m_PersianMonth As Byte, m_PersianDay As Byte
        m_PersianYear = CType(JDate.ToSqlString.Value.Substring(0, 4), Short)
        m_PersianMonth = CType(JDate.ToSqlString.Value.Substring(4, 2), Byte)
        m_PersianDay = CType(JDate.ToSqlString.Value.Substring(6, 2), Byte)
        If m_PersianYear > 9378 OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth > 10) OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth = 10 AndAlso m_PersianDay > 10) Then Return SqlBoolean.Null
        Dim m_Calendar As New Globalization.PersianCalendar
        Return New SqlBoolean(m_Calendar.IsLeapYear(m_PersianYear) AndAlso m_PersianMonth = 12 AndAlso m_PersianDay = 30)
    End Function

    <Microsoft.SqlServer.Server.SqlFunction()>
    Public Shared Function AddDays(<SqlFacet(MaxSize:=8)> ByVal JDate As SqlChars, ByVal Count As SqlInt32) As <SqlFacet(MaxSize:=8)> SqlChars
        If IsNothing(JDate) OrElse IsDBNull(JDate) OrElse Not IsNumeric(JDate.ToSqlString.Value.Trim) OrElse JDate.ToSqlString.Value.Trim.Length <> 8 Then Return SqlChars.Null
        Dim m_PersianYear As Short, m_PersianMonth As Byte, m_PersianDay As Byte
        m_PersianYear = CType(JDate.ToSqlString.Value.Substring(0, 4), Short)
        m_PersianMonth = CType(JDate.ToSqlString.Value.Substring(4, 2), Byte)
        m_PersianDay = CType(JDate.ToSqlString.Value.Substring(6, 2), Byte)
        If m_PersianYear > 9378 OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth > 10) OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth = 10 AndAlso m_PersianDay > 10) Then Return SqlChars.Null
        Dim m_Calendar As New Globalization.PersianCalendar, m_Date As Date = m_Calendar.ToDateTime(m_PersianYear, m_PersianMonth, m_PersianDay, 0, 0, 0, 0)
        Try
            m_Date = m_Calendar.AddDays(m_Date, If(Count.IsNull, 0, Count.Value))
        Catch
            Return SqlChars.Null
        End Try
        If m_Date.Year < 622 OrElse (m_Date.Year = 622 AndAlso m_Date.Month < 3) OrElse (m_Date.Year = 622 AndAlso m_Date.Month = 3 AndAlso m_Date.Day < 21) Then Return SqlChars.Null
        Return New SqlChars(String.Format("{0}{1}{2}", m_Calendar.GetYear(m_Date).ToString.PadLeft(4, "0"c), m_Calendar.GetMonth(m_Date).ToString.PadLeft(2, "0"c), m_Calendar.GetDayOfMonth(m_Date).ToString.PadLeft(2, "0"c)))
    End Function

    <Microsoft.SqlServer.Server.SqlFunction()>
    Public Shared Function AddWeeks(<SqlFacet(MaxSize:=8)> ByVal JDate As SqlChars, ByVal Count As SqlInt32) As <SqlFacet(MaxSize:=8)> SqlChars
        If IsNothing(JDate) OrElse IsDBNull(JDate) OrElse Not IsNumeric(JDate.ToSqlString.Value.Trim) OrElse JDate.ToSqlString.Value.Trim.Length <> 8 Then Return SqlChars.Null
        Dim m_PersianYear As Short, m_PersianMonth As Byte, m_PersianDay As Byte
        m_PersianYear = CType(JDate.ToSqlString.Value.Substring(0, 4), Short)
        m_PersianMonth = CType(JDate.ToSqlString.Value.Substring(4, 2), Byte)
        m_PersianDay = CType(JDate.ToSqlString.Value.Substring(6, 2), Byte)
        If m_PersianYear > 9378 OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth > 10) OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth = 10 AndAlso m_PersianDay > 10) Then Return SqlChars.Null
        Dim m_Calendar As New Globalization.PersianCalendar, m_Date As Date = m_Calendar.ToDateTime(m_PersianYear, m_PersianMonth, m_PersianDay, 0, 0, 0, 0)
        Try
            m_Date = m_Calendar.AddWeeks(m_Date, If(Count.IsNull, 0, Count.Value))
        Catch
            Return SqlChars.Null
        End Try
        If m_Date.Year < 622 OrElse (m_Date.Year = 622 AndAlso m_Date.Month < 3) OrElse (m_Date.Year = 622 AndAlso m_Date.Month = 3 AndAlso m_Date.Day < 21) Then Return SqlChars.Null
        Return New SqlChars(String.Format("{0}{1}{2}", m_Calendar.GetYear(m_Date).ToString.PadLeft(4, "0"c), m_Calendar.GetMonth(m_Date).ToString.PadLeft(2, "0"c), m_Calendar.GetDayOfMonth(m_Date).ToString.PadLeft(2, "0"c)))
    End Function

    <Microsoft.SqlServer.Server.SqlFunction()>
    Public Shared Function AddMonths(<SqlFacet(MaxSize:=8)> ByVal JDate As SqlChars, ByVal Count As SqlInt32) As <SqlFacet(MaxSize:=8)> SqlChars
        If IsNothing(JDate) OrElse IsDBNull(JDate) OrElse Not IsNumeric(JDate.ToSqlString.Value.Trim) OrElse JDate.ToSqlString.Value.Trim.Length <> 8 Then Return SqlChars.Null
        Dim m_PersianYear As Short, m_PersianMonth As Byte, m_PersianDay As Byte
        m_PersianYear = CType(JDate.ToSqlString.Value.Substring(0, 4), Short)
        m_PersianMonth = CType(JDate.ToSqlString.Value.Substring(4, 2), Byte)
        m_PersianDay = CType(JDate.ToSqlString.Value.Substring(6, 2), Byte)
        If m_PersianYear > 9378 OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth > 10) OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth = 10 AndAlso m_PersianDay > 10) Then Return SqlChars.Null
        Dim m_Calendar As New Globalization.PersianCalendar, m_Date As Date = m_Calendar.ToDateTime(m_PersianYear, m_PersianMonth, m_PersianDay, 0, 0, 0, 0)
        m_Date = m_Calendar.AddMonths(m_Date, If(Count.IsNull, 0, Count.Value))
        If m_Date.Year < 622 OrElse (m_Date.Year = 622 AndAlso m_Date.Month < 3) OrElse (m_Date.Year = 622 AndAlso m_Date.Month = 3 AndAlso m_Date.Day < 21) Then Return SqlChars.Null
        Return New SqlChars(String.Format("{0}{1}{2}", m_Calendar.GetYear(m_Date).ToString.PadLeft(4, "0"c), m_Calendar.GetMonth(m_Date).ToString.PadLeft(2, "0"c), m_Calendar.GetDayOfMonth(m_Date).ToString.PadLeft(2, "0"c)))
    End Function

    <Microsoft.SqlServer.Server.SqlFunction()>
    Public Shared Function AddQuarters(<SqlFacet(MaxSize:=8)> ByVal JDate As SqlChars, ByVal Count As SqlInt32) As <SqlFacet(MaxSize:=8)> SqlChars
        If IsNothing(JDate) OrElse IsDBNull(JDate) OrElse Not IsNumeric(JDate.ToSqlString.Value.Trim) OrElse JDate.ToSqlString.Value.Trim.Length <> 8 Then Return SqlChars.Null
        Dim m_PersianYear As Short, m_PersianMonth As Byte, m_PersianDay As Byte
        m_PersianYear = CType(JDate.ToSqlString.Value.Substring(0, 4), Short)
        m_PersianMonth = CType(JDate.ToSqlString.Value.Substring(4, 2), Byte)
        m_PersianDay = CType(JDate.ToSqlString.Value.Substring(6, 2), Byte)
        If m_PersianYear > 9378 OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth > 10) OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth = 10 AndAlso m_PersianDay > 10) Then Return SqlChars.Null
        Dim m_Calendar As New Globalization.PersianCalendar, m_Date As Date = m_Calendar.ToDateTime(m_PersianYear, m_PersianMonth, m_PersianDay, 0, 0, 0, 0)
        m_Date = m_Calendar.AddMonths(m_Date, If(Count.IsNull, 0, Count.Value * 3))
        If m_Date.Year < 622 OrElse (m_Date.Year = 622 AndAlso m_Date.Month < 3) OrElse (m_Date.Year = 622 AndAlso m_Date.Month = 3 AndAlso m_Date.Day < 21) Then Return SqlChars.Null
        Return New SqlChars(String.Format("{0}{1}{2}", m_Calendar.GetYear(m_Date).ToString.PadLeft(4, "0"c), m_Calendar.GetMonth(m_Date).ToString.PadLeft(2, "0"c), m_Calendar.GetDayOfMonth(m_Date).ToString.PadLeft(2, "0"c)))
    End Function

    <Microsoft.SqlServer.Server.SqlFunction()>
    Public Shared Function AddYears(<SqlFacet(MaxSize:=8)> ByVal JDate As SqlChars, ByVal Count As SqlInt32) As <SqlFacet(MaxSize:=8)> SqlChars
        If IsNothing(JDate) OrElse IsDBNull(JDate) OrElse Not IsNumeric(JDate.ToSqlString.Value.Trim) OrElse JDate.ToSqlString.Value.Trim.Length <> 8 Then Return SqlChars.Null
        Dim m_PersianYear As Short, m_PersianMonth As Byte, m_PersianDay As Byte
        m_PersianYear = CType(JDate.ToSqlString.Value.Substring(0, 4), Short)
        m_PersianMonth = CType(JDate.ToSqlString.Value.Substring(4, 2), Byte)
        m_PersianDay = CType(JDate.ToSqlString.Value.Substring(6, 2), Byte)
        If m_PersianYear > 9378 OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth > 10) OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth = 10 AndAlso m_PersianDay > 10) Then Return SqlChars.Null
        Dim m_Calendar As New Globalization.PersianCalendar, m_Date As Date = m_Calendar.ToDateTime(m_PersianYear, m_PersianMonth, m_PersianDay, 0, 0, 0, 0)
        m_Date = m_Calendar.AddYears(m_Date, If(Count.IsNull, 0, Count.Value))
        If m_Date.Year < 622 OrElse (m_Date.Year = 622 AndAlso m_Date.Month < 3) OrElse (m_Date.Year = 622 AndAlso m_Date.Month = 3 AndAlso m_Date.Day < 21) Then Return SqlChars.Null
        Return New SqlChars(String.Format("{0}{1}{2}", m_Calendar.GetYear(m_Date).ToString.PadLeft(4, "0"c), m_Calendar.GetMonth(m_Date).ToString.PadLeft(2, "0"c), m_Calendar.GetDayOfMonth(m_Date).ToString.PadLeft(2, "0"c)))
    End Function

    <Microsoft.SqlServer.Server.SqlFunction()>
    Public Shared Function DateDifferenceInDays(<SqlFacet(MaxSize:=8)> ByVal JDate1 As SqlChars, <SqlFacet(MaxSize:=8)> ByVal JDate2 As SqlChars) As SqlInt32
        If IsNothing(JDate1) OrElse IsDBNull(JDate1) OrElse Not IsNumeric(JDate1.ToSqlString.Value.Trim) OrElse JDate1.ToSqlString.Value.Trim.Length <> 8 Then Return SqlInt32.Null
        If IsNothing(JDate2) OrElse IsDBNull(JDate2) OrElse Not IsNumeric(JDate2.ToSqlString.Value.Trim) OrElse JDate2.ToSqlString.Value.Trim.Length <> 8 Then Return SqlInt32.Null
        Dim m_PersianYear As Short, m_PersianMonth As Byte, m_PersianDay As Byte
        m_PersianYear = CType(JDate1.ToSqlString.Value.Substring(0, 4), Short)
        m_PersianMonth = CType(JDate1.ToSqlString.Value.Substring(4, 2), Byte)
        m_PersianDay = CType(JDate1.ToSqlString.Value.Substring(6, 2), Byte)
        If m_PersianYear > 9378 OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth > 10) OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth = 10 AndAlso m_PersianDay > 10) Then Return SqlInt32.Null
        Dim m_Calendar As New Globalization.PersianCalendar
        Dim m_Date1 As Date = m_Calendar.ToDateTime(m_PersianYear, m_PersianMonth, m_PersianDay, 0, 0, 0, 0)
        m_PersianYear = CType(JDate2.ToSqlString.Value.Substring(0, 4), Short)
        m_PersianMonth = CType(JDate2.ToSqlString.Value.Substring(4, 2), Byte)
        m_PersianDay = CType(JDate2.ToSqlString.Value.Substring(6, 2), Byte)
        If m_PersianYear > 9378 OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth > 10) OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth = 10 AndAlso m_PersianDay > 10) Then Return SqlInt32.Null
        Dim m_Date2 As Date = m_Calendar.ToDateTime(m_PersianYear, m_PersianMonth, m_PersianDay, 0, 0, 0, 0)
        Dim m_Difference As System.TimeSpan = m_Date1.Subtract(m_Date2)
        Return New SqlInt32(m_Difference.Days)
    End Function

    <Microsoft.SqlServer.Server.SqlFunction()>
    Public Shared Function DateDifferenceInMonths(<SqlFacet(MaxSize:=8)> ByVal JDate1 As SqlChars, <SqlFacet(MaxSize:=8)> ByVal JDate2 As SqlChars) As <SqlFacet(Precision:=10, Scale:=2)> SqlDecimal
        If IsNothing(JDate1) OrElse IsDBNull(JDate1) OrElse Not IsNumeric(JDate1.ToSqlString.Value.Trim) OrElse JDate1.ToSqlString.Value.Trim.Length <> 8 Then Return SqlDecimal.Null
        If IsNothing(JDate2) OrElse IsDBNull(JDate2) OrElse Not IsNumeric(JDate2.ToSqlString.Value.Trim) OrElse JDate2.ToSqlString.Value.Trim.Length <> 8 Then Return SqlDecimal.Null
        Dim m_PersianYear1 As Integer, m_PersianMonth1 As Integer, m_PersianDay1 As Integer
        m_PersianYear1 = CType(JDate1.ToSqlString.Value.Substring(0, 4), Integer)
        m_PersianMonth1 = CType(JDate1.ToSqlString.Value.Substring(4, 2), Integer)
        m_PersianDay1 = CType(JDate1.ToSqlString.Value.Substring(6, 2), Integer)
        If m_PersianYear1 > 9378 OrElse (m_PersianYear1 = 9378 AndAlso m_PersianMonth1 > 10) OrElse (m_PersianYear1 = 9378 AndAlso m_PersianMonth1 = 10 AndAlso m_PersianDay1 > 10) Then Return SqlDecimal.Null
        Dim m_PersianYear2 As Integer, m_PersianMonth2 As Integer, m_PersianDay2 As Integer
        m_PersianYear2 = CType(JDate2.ToSqlString.Value.Substring(0, 4), Integer)
        m_PersianMonth2 = CType(JDate2.ToSqlString.Value.Substring(4, 2), Integer)
        m_PersianDay2 = CType(JDate2.ToSqlString.Value.Substring(6, 2), Integer)
        If m_PersianYear2 > 9378 OrElse (m_PersianYear2 = 9378 AndAlso m_PersianMonth2 > 10) OrElse (m_PersianYear2 = 9378 AndAlso m_PersianMonth2 = 10 AndAlso m_PersianDay2 > 10) Then Return SqlDecimal.Null
        Dim m_Result As Decimal
        If m_PersianYear1 > m_PersianYear2 OrElse (m_PersianYear1 = m_PersianYear2 AndAlso m_PersianMonth1 > m_PersianMonth2) OrElse (m_PersianYear1 = m_PersianYear2 AndAlso m_PersianMonth1 = m_PersianMonth2 AndAlso m_PersianDay1 >= m_PersianDay2) Then
            m_Result = CType(((m_PersianYear1 - m_PersianYear2) * 12) + (m_PersianMonth1 - m_PersianMonth2) + ((m_PersianDay1 - m_PersianDay2) / 30), Decimal)
        Else
            m_Result = -1 * CType(((m_PersianYear2 - m_PersianYear1) * 12) + (m_PersianMonth2 - m_PersianMonth1) + ((m_PersianDay2 - m_PersianDay1) / 30), Decimal)
        End If
        Return New SqlDecimal(Math.Round(m_Result, 2))
    End Function

    <Microsoft.SqlServer.Server.SqlFunction()>
    Public Shared Function DateDifferenceInYears(<SqlFacet(MaxSize:=8)> ByVal JDate1 As SqlChars, <SqlFacet(MaxSize:=8)> ByVal JDate2 As SqlChars) As <SqlFacet(Precision:=10, Scale:=2)> SqlDecimal
        If IsNothing(JDate1) OrElse IsDBNull(JDate1) OrElse Not IsNumeric(JDate1.ToSqlString.Value.Trim) OrElse JDate1.ToSqlString.Value.Trim.Length <> 8 Then Return SqlDecimal.Null
        If IsNothing(JDate2) OrElse IsDBNull(JDate2) OrElse Not IsNumeric(JDate2.ToSqlString.Value.Trim) OrElse JDate2.ToSqlString.Value.Trim.Length <> 8 Then Return SqlDecimal.Null
        Dim m_PersianYear1 As Integer, m_PersianMonth1 As Integer, m_PersianDay1 As Integer
        m_PersianYear1 = CType(JDate1.ToSqlString.Value.Substring(0, 4), Integer)
        m_PersianMonth1 = CType(JDate1.ToSqlString.Value.Substring(4, 2), Integer)
        m_PersianDay1 = CType(JDate1.ToSqlString.Value.Substring(6, 2), Integer)
        If m_PersianYear1 > 9378 OrElse (m_PersianYear1 = 9378 AndAlso m_PersianMonth1 > 10) OrElse (m_PersianYear1 = 9378 AndAlso m_PersianMonth1 = 10 AndAlso m_PersianDay1 > 10) Then Return SqlDecimal.Null
        Dim m_PersianYear2 As Integer, m_PersianMonth2 As Integer, m_PersianDay2 As Integer
        m_PersianYear2 = CType(JDate2.ToSqlString.Value.Substring(0, 4), Integer)
        m_PersianMonth2 = CType(JDate2.ToSqlString.Value.Substring(4, 2), Integer)
        m_PersianDay2 = CType(JDate2.ToSqlString.Value.Substring(6, 2), Integer)
        If m_PersianYear2 > 9378 OrElse (m_PersianYear2 = 9378 AndAlso m_PersianMonth2 > 10) OrElse (m_PersianYear2 = 9378 AndAlso m_PersianMonth2 = 10 AndAlso m_PersianDay2 > 10) Then Return SqlDecimal.Null
        Dim m_Result As Decimal
        If m_PersianYear1 > m_PersianYear2 OrElse (m_PersianYear1 = m_PersianYear2 AndAlso m_PersianMonth1 > m_PersianMonth2) OrElse (m_PersianYear1 = m_PersianYear2 AndAlso m_PersianMonth1 = m_PersianMonth2 AndAlso m_PersianDay1 >= m_PersianDay2) Then
            m_Result = CType((m_PersianYear1 - m_PersianYear2) + ((m_PersianMonth1 - m_PersianMonth2) / 12) + ((m_PersianDay1 - m_PersianDay2) / 365), Decimal)
        Else
            m_Result = -1 * CType((m_PersianYear2 - m_PersianYear1) + ((m_PersianMonth2 - m_PersianMonth1) / 12) + ((m_PersianDay2 - m_PersianDay1) / 365), Decimal)
        End If
        Return New SqlDecimal(Math.Round(m_Result, 2))
    End Function

    <Microsoft.SqlServer.Server.SqlFunction()>
    Public Shared Function CorrectJalaliDate(<SqlFacet(MaxSize:=8)> ByVal JDate As SqlChars) As <SqlFacet(MaxSize:=8)> SqlChars
        If IsNothing(JDate) OrElse IsDBNull(JDate) OrElse Not IsNumeric(JDate.ToSqlString.Value.Trim) OrElse JDate.ToSqlString.Value.Trim.Length <> 8 Then Return SqlChars.Null
        Dim m_PersianYear As Short, m_PersianMonth As Byte, m_PersianDay As Short
        m_PersianYear = CType(JDate.ToSqlString.Value.Substring(0, 4), Short)
        m_PersianMonth = CType(JDate.ToSqlString.Value.Substring(4, 2), Byte)
        m_PersianDay = CType(JDate.ToSqlString.Value.Substring(6, 2), Short)
        If m_PersianYear > 9378 OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth > 10) OrElse (m_PersianYear = 9378 AndAlso m_PersianMonth = 10 AndAlso m_PersianDay > 10) Then Return SqlChars.Null
        Dim m_Calendar As New Globalization.PersianCalendar, m_FirstWeekDayOfMonth As Byte = 0
        If m_PersianYear < 1 Then m_PersianYear = 1
        If m_PersianMonth < 1 Then m_PersianMonth = 1
        If m_PersianDay < 1 Then m_PersianDay = 1
        If m_PersianMonth > 12 Then m_PersianMonth = 12
        If m_PersianMonth = 12 Then
            If m_Calendar.IsLeapYear(m_PersianYear) Then
                If m_PersianDay > 30 Then m_PersianDay = 30
            Else
                If m_PersianDay > 29 Then m_PersianDay = 29
            End If
        ElseIf m_PersianMonth > 6 Then
            If m_PersianDay > 30 Then m_PersianDay = 30
        Else
            If m_PersianDay > 31 Then m_PersianDay = 31
        End If
        Return New SqlChars(m_PersianYear.ToString.PadLeft(4, "0"c) & m_PersianMonth.ToString.PadLeft(2, "0"c) & m_PersianDay.ToString.PadLeft(2, "0"c))
    End Function

    <Microsoft.SqlServer.Server.SqlFunction()>
    Public Shared Function FormatDate(<SqlFacet(MaxSize:=8)> ByVal JDate As SqlChars) As <SqlFacet(MaxSize:=10)> SqlChars
        If IsNothing(JDate) OrElse IsDBNull(JDate) Then Return SqlChars.Null
        Return New SqlChars(CInt(JDate.ToSqlString.Value.Trim).ToString("####/##/##"))
    End Function

End Class
