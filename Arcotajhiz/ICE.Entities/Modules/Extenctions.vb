Imports System.ComponentModel
Imports System.Globalization
Imports System.Runtime.CompilerServices
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Text.RegularExpressions
Imports System.Web
Imports System.IO
Imports System.Net
Imports System.Reflection
Imports System.Runtime.InteropServices

Namespace Modules
    Public Module Extensions
        Private _p As PersianCalendar = Nothing
        Private ReadOnly Property p() As PersianCalendar
            Get
                If _p Is Nothing Then
                    _p = New PersianCalendar()
                End If
                Return _p
            End Get
        End Property
        Private Function LocalToJalali(ByVal input As Date?, PadLeft As Integer, Optional ByVal isFormatted As Boolean = False) As String
            Return p.GetYear(input).ToString() & If(isFormatted, "/", "") & p.GetMonth(input).ToString().PadLeft(PadLeft, "0") & If(isFormatted, "/", "") & p.GetDayOfMonth(input).ToString().PadLeft(PadLeft, "0")
        End Function
        Public Function FromJalali(JDate As Integer, Optional JTime As Integer = 0) As DateTime
            Return p.ToDateTime(
                year:=JDate.ToString.Substring(0, 4),
                month:=JDate.ToString.Substring(4, 2),
                day:=JDate.ToString.Substring(6, 2),
                hour:=If(JTime = 0, 0, JTime.ToString.PadLeft(4, "0").Substring(0, 2)),
                minute:=If(JTime = 0, 0, JTime.ToString.PadLeft(4, "0").Substring(2, 2)),
                second:=0,
                millisecond:=0)
        End Function
        <Extension>
        Public Function DateTimeToFormattedTime(ByVal input As DateTime?, Optional PadLeft As Integer = 2) As String
            Dim _input = CType(input, DateTime)
            Return _input.Hour.ToString().PadLeft(PadLeft, "0") & ":" & _input.Minute.ToString().PadLeft(PadLeft, "0")
        End Function
        <Extension>
        Public Function DateTimeToFormattedTime(ByVal input As DateTime, Optional PadLeft As Integer = 2) As String
            Return input.Hour.ToString().PadLeft(PadLeft, "0") & ":" & input.Minute.ToString().PadLeft(PadLeft, "0")
        End Function
        <Extension>
        Public Function DateTimeToJalali(ByVal input As DateTime, Optional PadLeft As Integer = 2) As Integer
            Return LocalToJalali(input, PadLeft)
        End Function
        <Extension>
        Public Function DateTimeToJalali(ByVal input As DateTime?, Optional PadLeft As Integer = 2) As Integer
            Return LocalToJalali(input, PadLeft)
        End Function
        <Extension>
        Public Function DayOfMonthPersian(input As Date) As Integer
            Return p.GetDayOfMonth(input)
        End Function
        <Extension>
        Public Function DayOfWeekPersian(ByVal time As Date) As String
            Select Case time.DayOfWeek
                Case DayOfWeek.Saturday
                    Return "شنبه"
                Case DayOfWeek.Sunday
                    Return "یکشنبه"
                Case DayOfWeek.Monday
                    Return "دوشنبه"
                Case DayOfWeek.Tuesday
                    Return "سه شنبه"
                Case DayOfWeek.Wednesday
                    Return "چهارشنبه"
                Case DayOfWeek.Thursday
                    Return "پنجشنبه"
                Case DayOfWeek.Friday
                    Return "جمعه"
            End Select
            Return Nothing
        End Function
        <Extension>
        Public Function DeepCopy(Of T)(obj As T) As T
            Using ms = New MemoryStream()
                Dim formatter = New BinaryFormatter()
                formatter.Serialize(ms, obj)
                ms.Position = 0
                Return CType(formatter.Deserialize(ms), T)
            End Using
        End Function
        Private Function LocalTimeDiff(input As Date, until As Date, handleDay As Boolean) As String
            Dim secondDiff = Math.Abs(DateDiff(DateInterval.Second, input, until))
            Dim stepSecond = 24 * 60 * 60
            Dim remainDiff As Long
            If handleDay AndAlso secondDiff > stepSecond Then
                remainDiff = secondDiff Mod stepSecond
                Dim hourDiff = Math.Round(remainDiff / 60 / 60, 0)
                Return ((secondDiff - remainDiff) / stepSecond) & " روز" & If(hourDiff > 0, " و " & hourDiff & " ساعت", String.Empty)
            Else
                stepSecond = (60 * 60)
                remainDiff = secondDiff Mod stepSecond
                Dim hour = (secondDiff - remainDiff) / stepSecond
                stepSecond = 60
                secondDiff = remainDiff
                remainDiff = secondDiff Mod stepSecond
                Dim minute = (secondDiff - remainDiff) / stepSecond
                Return hour.ToString("00") & ":" & minute.ToString("00") & ":" & remainDiff.ToString("00")
            End If
        End Function
        <Extension>
        Public Function TimeDiff(input As Date, until As Date, handleDay As Boolean) As String
            Return LocalTimeDiff(input:=input, until:=until, handleDay:=handleDay)
        End Function
        <Extension>
        Public Function TimeDiff(input As Date, until As Date) As String
            Return LocalTimeDiff(input:=input, until:=until, handleDay:=True)
        End Function
        <Extension>
        Public Sub Replace(Of T)(x As T, y As T)
            Dim myT = GetType(T)
            Dim properties = myT.GetProperties((BindingFlags.Public Or BindingFlags.Instance))
            For Each prop In properties
                Dim attrib As ReadOnlyAttribute = CType(Attribute.GetCustomAttribute(prop, GetType(ReadOnlyAttribute)), ReadOnlyAttribute)
                Dim ro As Boolean = (Not prop.CanWrite _
                            OrElse ((Not (attrib) Is Nothing) _
                            AndAlso attrib.IsReadOnly))
                If Not ro Then
                    x.GetType().GetProperty(prop.Name).SetValue(x, y.GetType().GetProperty(prop.Name).GetValue(y, Nothing))
                End If
            Next
        End Sub
        <Extension>
        Public Function Filter(input As String, pattern As String, replacement As Char) As String
            If input Is Nothing Then Return Nothing
            Return Regex.Replace(input, pattern, replacement)
        End Function
        <Extension>
        Public Function GetMonthNamePersian(input As Date) As String
            Return p.GetMonthName(input)
        End Function
        <Extension>
        Public Function GetYearPersian(input As Date) As Integer
            Return p.GetYear(input)
        End Function
        <Extension>
        Public Function GetMonthName(ByVal p As PersianCalendar, ByVal time As Date) As String
            Return LocalGetMonthName(p.GetMonth(time))
        End Function
        <Extension>
        Public Function ToPersianMonth(monthIndex As Integer) As String
            Return LocalGetMonthName(monthIndex)
        End Function
        Private Function LocalGetMonthName(monthIndex As Integer) As String
            Select Case monthIndex
                Case 1
                    Return "فروردین"
                Case 2
                    Return "اردیبهشت"
                Case 3
                    Return "خرداد"
                Case 4
                    Return "تیر"
                Case 5
                    Return "مرداد"
                Case 6
                    Return "شهریور"
                Case 7
                    Return "مهر"
                Case 8
                    Return "آبان"
                Case 9
                    Return "آذر"
                Case 10
                    Return "دی"
                Case 11
                    Return "بهمن"
                Case 12
                    Return "اسفند"
            End Select
            Return Nothing
        End Function
        <Extension>
        Public Function IsNull(input As String, replace As String) As String
            Return If(String.IsNullOrEmpty(input), replace, input)
        End Function
        <Extension>
        Public Function IsValidDate(ByVal p As PersianCalendar, ByVal JalaliDate As String) As Boolean
            Try
                If JalaliDate.Length <> 10 Then
                    Return False
                End If
                If Not IsNumeric(JalaliDate.Replace("/", "")) Then
                    Return False
                End If
                Dim Year As Integer = JalaliDate.Substring(0, 4)
                Dim Month As Integer = JalaliDate.Substring(5, 2)
                Dim Day As Integer = JalaliDate.Substring(8, 2)
                If Year > 1500 Then
                    Return False
                End If
                If Year < 1300 Then
                    Return False
                End If
                If Month > 12 Then
                    Return False
                End If
                If Month < 1 Then
                    Return False
                End If
                If (Month <= 6 And Day > 31) OrElse (Month > 6 And Day > 30) Then
                    Return False
                End If
                p.ToDateTime(Year, Month, Day, 0, 0, 0, 0)
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function
        <Extension>
        Public Sub Move(Of T)(ByRef list As List(Of T), OldIndex As Integer, NewIndex As Integer)
            Dim item = list(OldIndex)
            list.RemoveAt(OldIndex)
            list.Insert(NewIndex, item)
        End Sub
        <Extension>
        Public Function Normalize(ByVal input As String) As String
            If input Is Nothing Then Return Nothing
            Return Regex.Replace(Regex.Replace(input.Replace("ﻼ", "لا").Replace("ﻻ", "لا").Replace(" ", " ").Trim(" "c).Replace("آ", "ا").Replace("أ", "ا").Replace("إ", "ا").Replace("ي", "ی").Replace("ئ", "ی").Replace("ة", "ه").Replace("ۀ", "ه").Replace("ك", "ک").Replace("ؤ", "و").Replace("٠", "0").Replace("۰", "0").Replace("١", "1").Replace("۱", "1").Replace("٢", "2").Replace("۲", "2").Replace("٣", "3").Replace("۳", "3").Replace("٤", "4").Replace("۴", "4").Replace("٥", "5").Replace("۵", "5").Replace("٦", "6").Replace("۶", "6").Replace("٧", "7").Replace("۷", "7").Replace("٨", "8").Replace("۸", "8").Replace("٩", "9").Replace("۹", "9").Replace("الله", "اله").Replace("ا...", "اله").Replace("شرکت", ""), "[$-/@#:-?؟{-~!""^_`\[\]\\ًٌٍَُِّ،؛ء«»ـ]", ""), "[ ]+", " ").Trim
        End Function
        <Extension>
        Public Function ReplaceNumberToPersian(ByVal input As String) As String
            Return input.Replace("0", "۰").Replace("1", "۱").Replace("2", "۲").Replace("3", "۳").Replace("4", "۴").Replace("5", "۵").Replace("6", "۶").Replace("7", "۷").Replace("8", "۸").Replace("9", "۹")
        End Function
        <Extension>
        Public Function ReplacePersianNumber(ByVal input As String) As String
            Return input.Replace("٠", "0").Replace("۰", "0").Replace("١", "1").Replace("۱", "1").Replace("٢", "2").Replace("۲", "2").Replace("٣", "3").Replace("۳", "3").Replace("٤", "4").Replace("۴", "4").Replace("٥", "5").Replace("۵", "5").Replace("٦", "6").Replace("۶", "6").Replace("٧", "7").Replace("۷", "7").Replace("٨", "8").Replace("۸", "8").Replace("٩", "9").Replace("۹", "9")
        End Function
        <Extension>
        Public Function SearchUnify(ByVal input As String) As String
            If input Is Nothing Then Return Nothing
            'Return Regex.Replace(input.Replace(" ", " ").Replace("ا", "آ").Replace("ا", "أ").Replace("ا", "إ").Replace("ی", "ي").Replace("ی", "ئ").Replace("ة", "ه").Replace("ۀ", "ه").Replace("ک", "ك").Replace("ؤ", "و").Replace("٠", "0").Replace("۰", "0").Replace("١", "1").Replace("۱", "1").Replace("٢", "2").Replace("۲", "2").Replace("٣", "3").Replace("۳", "3").Replace("٤", "4").Replace("۴", "4").Replace("٥", "5").Replace("۵", "5").Replace("٦", "6").Replace("۶", "6").Replace("٧", "7").Replace("۷", "7").Replace("٨", "8").Replace("۸", "8").Replace("٩", "9").Replace("۹", "9"), "[ ]+", " ").Trim
            Return Regex.Replace(input.Replace(" ", " ").Replace("ی", "ي").Replace("٠", "0").Replace("۰", "0").Replace("١", "1").Replace("۱", "1").Replace("٢", "2").Replace("۲", "2").Replace("٣", "3").Replace("۳", "3").Replace("٤", "4").Replace("۴", "4").Replace("٥", "5").Replace("۵", "5").Replace("٦", "6").Replace("۶", "6").Replace("٧", "7").Replace("۷", "7").Replace("٨", "8").Replace("۸", "8").Replace("٩", "9").Replace("۹", "9"), "[ ]+", " ").Trim
        End Function
        <Extension>
        Public Function AutoCompleteUnify(ByVal input As String) As String
            If input Is Nothing Then Return Nothing
            Return Regex.Replace(input.Replace(" ", " ").Replace("ی", "ي").Replace("٠", "0").Replace("۰", "0").Replace("١", "1").Replace("۱", "1").Replace("٢", "2").Replace("۲", "2").Replace("٣", "3").Replace("۳", "3").Replace("٤", "4").Replace("۴", "4").Replace("٥", "5").Replace("۵", "5").Replace("٦", "6").Replace("۶", "6").Replace("٧", "7").Replace("۷", "7").Replace("٨", "8").Replace("۸", "8").Replace("٩", "9").Replace("۹", "9"), "[ ]+", " ").Trim
        End Function
        <Extension>
        Public Function ToFormattedJalali(ByVal input As Date, Optional PadLeft As Integer = 2) As String
            Return LocalToJalali(input, PadLeft, True)
        End Function
        <Extension>
        Public Function ToFormattedTime(ByVal input As Date, Optional PadLeft As Integer = 2) As String
            Return input.Hour.ToString().PadLeft(PadLeft, "0") & ":" & input.Minute.ToString().PadLeft(PadLeft, "0")
        End Function
        <Extension>
        Public Function ToFormattedTime(ByVal input As Date?, Optional PadLeft As Integer = 2) As String
            Dim _input = CType(input, Date)
            Return _input.Hour.ToString().PadLeft(PadLeft, "0") & ":" & _input.Minute.ToString().PadLeft(PadLeft, "0")
        End Function
        <Extension>
        Public Function ToJalali(ByVal input As Date, Optional PadLeft As Integer = 2) As Integer
            Return LocalToJalali(input, PadLeft)
        End Function
        <Extension>
        Public Function ConvertToMiladi(ByVal input As String) As DateTime
            If String.IsNullOrEmpty(input) Then
                Return New DateTime
            End If
            Dim year As Integer
            Dim month As Integer
            Dim day As Integer
            year = input.Split("/")(0)
            month = input.Split("/")(1)
            day = input.Split("/")(2)
            Dim dt As New DateTime(year:=year, month:=month, day:=day, calendar:=p)
            Return dt
        End Function
        <Extension>
        Public Function ConvertToJalali(ByVal input As DateTime) As String
            If String.IsNullOrEmpty(input) OrElse input = DateTime.MinValue Then
                Return New DateTime
            End If
            Dim year = p.GetYear(input)
            Dim month = p.GetMonth(input)
            Dim day = p.GetDayOfMonth(input)
            Return year & "/" & If(month < 10, "0" & month, month) & "/" & If(day < 10, "0" & day, day)
        End Function
        <Extension>
        Public Function ToJalali(ByVal input As Date?, Optional PadLeft As Integer = 2) As Integer
            Return LocalToJalali(input, PadLeft)
        End Function
        <Extension>
        Public Function ToJalali(ByVal input As Date, longText As Boolean, Optional PadLeft As Integer = 2, Optional SkipTime As Boolean = False) As String
            If longText Then
                Return (input.DayOfWeekPersian & " " & input.DayOfMonthPersian().ToString() & " " & input.GetMonthNamePersian() & " " & input.GetYearPersian().ToString() & If(SkipTime, String.Empty, " - " & input.Hour.ToString().PadLeft(2, "0") & ":" & input.Minute.ToString().PadLeft(2, "0"))).ReplaceNumberToPersian()
            Else
                Return LocalToJalali(input, PadLeft)
            End If
        End Function
        <Extension>
        Public Function ToNumeric(ByVal input As String) As Decimal
            If input Is Nothing Then Return Nothing
            Return CType(Regex.Replace(input.Replace("٠", "0").Replace("۰", "0").Replace("١", "1").Replace("۱", "1").Replace("٢", "2").Replace("۲", "2").Replace("٣", "3").Replace("۳", "3").Replace("٤", "4").Replace("۴", "4").Replace("٥", "5").Replace("۵", "5").Replace("٦", "6").Replace("۶", "6").Replace("٧", "7").Replace("۷", "7").Replace("٨", "8").Replace("۸", "8").Replace("٩", "9").Replace("۹", "9"), "[^\d]", "").PadLeft(1, "0"c), Decimal)
        End Function
        <Extension>
        Public Function ToTitle(input As String) As String
            If input Is Nothing Then Return Nothing
            Return input.Substring(0, 1).ToUpper() & If(input.Length > 1, input.Substring(1).ToLower(), "")
        End Function
        <Extension>
        Public Function ToUrl(input As String) As String
            Return input.Filter("[ ./\~!@#$%^&*_+|=:;""'<,>?]+", "-"c)
        End Function
        <Extension>
        Public Function Unify(ByVal input As String) As String
            If input Is Nothing Then Return Nothing
            Return Regex.Replace(input.Replace("ﻼ", "لا").Replace("ﻻ", "لا").Replace(" ", " ").Replace("أ", "ا").Replace("إ", "ا").Replace("ي", "ی").Replace("ة", "ه").Replace("ۀ", "ه").Replace("ك", "ک").Replace("ؤ", "و").Replace("٠", "0").Replace("۰", "0").Replace("١", "1").Replace("۱", "1").Replace("٢", "2").Replace("۲", "2").Replace("٣", "3").Replace("۳", "3").Replace("٤", "4").Replace("۴", "4").Replace("٥", "5").Replace("۵", "5").Replace("٦", "6").Replace("۶", "6").Replace("٧", "7").Replace("۷", "7").Replace("٨", "8").Replace("۸", "8").Replace("٩", "9").Replace("۹", "9"), "[ ]+", " ").Trim
        End Function
        <Extension>
        Public Function WebUnify(ByVal input As String) As String
            If input Is Nothing Then Return Nothing
            Return Regex.Replace(input.Replace("ﻼ", "لا").Replace("ﻻ", "لا").Replace(" ", " ").Replace("أ", "ا").Replace("إ", "ا").Replace("ي", "ی").Replace("ة", "ه").Replace("ۀ", "ه").Replace("ك", "ک").Replace("ؤ", "و"), "[ ]+", " ").Trim
        End Function
        <Extension>
        Public Function DateDiffSimple(input As Date, until As Date, Optional skipTitle As Boolean = False) As String
            Dim result = ""
            Dim str = ""
            Dim second = DateDiff(DateInterval.Second, input, until)
            Dim diff = second
            If (Math.Abs(second) < 60) Then
                str = "ثانيه"
            Else
                diff = Math.Round(second / 60, 0)
                If (Math.Abs(diff) < 60) Then
                    str = "دقيقه"
                Else
                    diff = Math.Round(second / 60 / 60, 0)
                    If (Math.Abs(diff) < 24) Then
                        str = "ساعت"
                    Else
                        diff = Math.Round(second / 60 / 60 / 24, 0)
                        If (Math.Abs(diff) < 31) Then
                            str = "روز"
                        Else
                            diff = Math.Round(second / 60 / 60 / 24 / 31, 0)
                            If (Math.Abs(diff) < 12) Then
                                str = "ماه"
                            Else
                                diff = Math.Round(second / 60 / 60 / 24 / 30 / 12, 0)
                                str = "سال"
                            End If
                        End If
                    End If
                End If
            End If
            If skipTitle Then
                result = Math.Abs(diff).ToString() + " " + str
            Else
                If diff >= 0 Then
                    result = diff.ToString() + " " + str + " قبل"
                Else
                    result = Math.Abs(diff).ToString() + " " + str + " بعد"
                End If
            End If
            Return result
        End Function
        <Extension>
        Public Function GetWords(input As String, count As Integer) As String
            Dim s = input.Split(" ")
            If s.Length < count Then
                count = s.Length
            End If
            Dim strResult = String.Empty
            For i = 0 To count - 1
                If s(i).Length > 0 Then
                    strResult &= If(strResult.Length > 0, " ", String.Empty) & s(i)
                End If
            Next
            If count < s.Length Then
                strResult &= " ..."
            End If
            Return strResult
        End Function
        <Extension()>
        Public Function Summery(ByVal text As String, length As Integer) As String
            If text.Length <= length Then
                Return text
            Else
                Return text.Substring(0, length) & "..."
            End If
        End Function
        <Extension>
        Public Function IsMobileDevice(ByVal Request As HttpRequestBase) As Boolean
            If Request.Browser IsNot Nothing AndAlso Request.Browser.IsMobileDevice Then
                Return True
            End If
            If Request.UserAgent IsNot Nothing AndAlso (
                Request.UserAgent.IndexOf("iphone", StringComparison.CurrentCultureIgnoreCase) >= 0 _
                OrElse Request.UserAgent.IndexOf("ipad", StringComparison.CurrentCultureIgnoreCase) >= 0 _
                OrElse Request.UserAgent.IndexOf("android", StringComparison.CurrentCultureIgnoreCase) >= 0) Then
                Return True
            End If
            Return False
        End Function
        <Extension>
        Public Function GetNthIndex(s As String, t As Char, n As Integer) As Integer
            Dim count As Integer = 0
            For i As Integer = 0 To s.Length - 1
                If s(i) = t Then
                    count += 1
                    If count = n Then
                        Return i
                    End If
                End If
            Next
            Return -1
        End Function
    End Module
End Namespace