Imports System
Imports System.Data.SqlTypes
Imports System.IO
Imports Microsoft.SqlServer.Server

Friend Structure TimeSpan
        Private m_Minute As Integer
        Private m_Hour As Integer

        Friend Function TimeString() As String
            Return (m_Hour.ToString.PadLeft(2, "0"c) & m_Minute.ToString.PadLeft(2, "0"c))
        End Function

        Friend Sub Fill(ByVal TimeString As String)
            m_Minute = Integer.Parse(TimeString.Substring(TimeString.Length - 2))
            m_Hour = Integer.Parse(TimeString.Substring(0, TimeString.Length - 2))
        End Sub

        Friend Sub Add(ByVal TimeString As String)
            m_Hour += Integer.Parse(TimeString.Substring(0, TimeString.Length - 2)) + ((m_Minute + Integer.Parse(TimeString.Substring(TimeString.Length - 2))) \ 60)
            m_Minute = (m_Minute + Integer.Parse(TimeString.Substring(TimeString.Length - 2))) Mod 60
        End Sub

        Friend Sub Init()
            m_Minute = 0
            m_Hour = 0
        End Sub
    End Structure

<Serializable(), SqlUserDefinedAggregate(Format.UserDefined, IsInvariantToNulls:=True, IsInvariantToDuplicates:=False, IsInvariantToOrder:=False, MaxByteSize:=4000)>
Public Class TimeSum
        Implements IBinarySerialize

        Private m_Time As TimeSpan

        Public Sub Init()
            m_Time.Init()
        End Sub

    Public Sub Accumulate(<SqlFacet(MaxSize:=4000)> ByVal value As SqlString)
        If value.IsNull OrElse Not Integer.TryParse(value.Value, Nothing) OrElse value.Value.Length < 2 Then Return
        m_Time.Add(value.Value)
    End Sub

    Public Sub Merge(ByVal other As TimeSum)
            m_Time.Add(other.m_Time.TimeString)
        End Sub

    Public Function Terminate() As <SqlFacet(MaxSize:=4000)> SqlString
        Return New SqlString(m_Time.TimeString.PadLeft(4, "0"c))
    End Function

    Public Sub Read(ByVal r As BinaryReader) Implements IBinarySerialize.Read
            m_Time.Fill(r.ReadString())
        End Sub

        Public Sub Write(ByVal w As BinaryWriter) Implements IBinarySerialize.Write
            w.Write(m_Time.TimeString)
        End Sub
    End Class
