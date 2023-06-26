Imports System
Imports System.Data.SqlTypes
Imports Microsoft.SqlServer.Server

Partial Public Class Time
    <Microsoft.SqlServer.Server.SqlFunction()>
    Public Shared Function AddTime(<SqlFacet(MaxSize:=11)> ByVal Time1 As SqlString, <SqlFacet(MaxSize:=11)> ByVal Time2 As SqlString) As <SqlFacet(MaxSize:=11)> SqlString
        If Time1.IsNull OrElse Time2.IsNull OrElse Not Integer.TryParse(Time1.Value, Nothing) OrElse Not Integer.TryParse(Time2.Value, Nothing) OrElse Time1.Value.Length < 3 OrElse Time2.Value.Length < 3 Then Return SqlString.Null
        Dim m_Result As Long = CInt(Time1.Value.Substring(0, Time1.Value.Length - 2)) * 60 + CInt(Time1.Value.Substring(Time1.Value.Length - 2))
        m_Result += CInt(Time2.Value.Substring(0, Time2.Value.Length - 2)) * 60 + CInt(Time2.Value.Substring(Time2.Value.Length - 2))
        Return New SqlString((m_Result \ 60).ToString("########0#") & (m_Result Mod 60).ToString("0#"))
    End Function

    <Microsoft.SqlServer.Server.SqlFunction()>
    Public Shared Function AddHours(<SqlFacet(MaxSize:=11)> ByVal Time As SqlString, ByVal Count As SqlInt32) As <SqlFacet(MaxSize:=11)> SqlString
        If Time.IsNull OrElse Not Integer.TryParse(Time.Value, Nothing) OrElse Time.Value.Length < 3 Then Return SqlString.Null
        Dim m_Result As Long = (CInt(Time.Value.Substring(0, Time.Value.Length - 2)) + If(Count.IsNull, 0, Count.Value)) * 60 + CInt(Time.Value.Substring(Time.Value.Length - 2))
        Return New SqlString((m_Result \ 60).ToString("########0#") & (m_Result Mod 60).ToString("0#"))
    End Function

    <Microsoft.SqlServer.Server.SqlFunction()>
    Public Shared Function AddMinutes(<SqlFacet(MaxSize:=11)> ByVal Time As SqlString, ByVal Count As SqlInt32) As <SqlFacet(MaxSize:=11)> SqlString
        If Time.IsNull OrElse Not Integer.TryParse(Time.Value, Nothing) OrElse Time.Value.Length < 3 Then Return SqlString.Null
        Dim m_Result As Long = CInt(Time.Value.Substring(0, Time.Value.Length - 2)) * 60 + CInt(Time.Value.Substring(Time.Value.Length - 2)) + If(Count.IsNull, 0, Count.Value)
        Return New SqlString((m_Result \ 60).ToString("########0#") & (m_Result Mod 60).ToString("0#"))
    End Function

    <Microsoft.SqlServer.Server.SqlFunction()>
    Public Shared Function GetHour(<SqlFacet(MaxSize:=11)> ByVal Time As SqlString) As SqlInt32
        If Time.IsNull OrElse Not Integer.TryParse(Time.Value, Nothing) OrElse Time.Value.Length < 3 Then Return SqlInt32.Null
        Dim m_Result As Integer = CInt(Time.Value.Substring(0, Time.Value.Length - 2))
        Return New SqlInt32(m_Result)
    End Function

    <Microsoft.SqlServer.Server.SqlFunction()>
    Public Shared Function GetMinute(<SqlFacet(MaxSize:=11)> ByVal Time As SqlString) As SqlInt16
        If Time.IsNull OrElse Not Integer.TryParse(Time.Value, Nothing) OrElse Time.Value.Length < 3 Then Return SqlInt16.Null
        Dim m_Result As Short = CShort(Time.Value.Substring(Time.Value.Length - 2))
        Return New SqlInt16(m_Result)
    End Function

    <Microsoft.SqlServer.Server.SqlFunction()>
    Public Shared Function TimeDifference(<SqlFacet(MaxSize:=11)> ByVal Time1 As SqlString, <SqlFacet(MaxSize:=11)> ByVal Time2 As SqlString) As <SqlFacet(MaxSize:=12)> SqlString
        If Time1.IsNull OrElse Time2.IsNull OrElse Not Integer.TryParse(Time1.Value, Nothing) OrElse Not Integer.TryParse(Time2.Value, Nothing) OrElse Time1.Value.Length < 3 OrElse Time2.Value.Length < 3 Then Return SqlString.Null
        Dim m_Result As Long = CInt(Time1.Value.Substring(0, Time1.Value.Length - 2)) * 60 + CInt(Time1.Value.Substring(Time1.Value.Length - 2))
        m_Result -= CInt(Time2.Value.Substring(0, Time2.Value.Length - 2)) * 60 + CInt(Time2.Value.Substring(Time2.Value.Length - 2))
        Return New SqlString(If(m_Result < 0, "-", "") & Math.Abs(m_Result \ 60).ToString("########0#") & Math.Abs(m_Result Mod 60).ToString("0#"))
    End Function

    <Microsoft.SqlServer.Server.SqlFunction()>
    Public Shared Function FormatTime(<SqlFacet(MaxSize:=11)> ByVal Input As SqlString) As <SqlFacet(MaxSize:=12)> SqlString
        If Input.IsNull OrElse Not Integer.TryParse(Input.Value, Nothing) OrElse Input.Value.Length < 3 Then Return SqlString.Null
        Return New SqlString(Input.Value.Substring(0, Input.Value.Length - 2).PadLeft(2, "0"c) & ":" & Input.Value.Substring(Input.Value.Length - 2))
    End Function

    <Microsoft.SqlServer.Server.SqlFunction()>
    Public Shared Function Now() As <SqlFacet(MaxSize:=4)> SqlString
        Return New SqlString(DateTime.Now.Hour.ToString.PadLeft(2, "0"c) & DateTime.Now.Minute.ToString.PadLeft(2, "0"c), String.Empty)
    End Function

End Class