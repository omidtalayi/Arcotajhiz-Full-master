Imports System.ComponentModel
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports System.Configuration
Imports Newtonsoft.Json
Imports System.Text
Imports ARCO.Entities
Imports ICE.Common.Business.Models

Public Class ErrorBusiness
    Private _LogPath As String
    Public ReadOnly Property LogPath As String
        Get
            If String.IsNullOrEmpty(_LogPath) AndAlso System.Web.HttpContext.Current IsNot Nothing Then
                _LogPath = System.Web.HttpContext.Current.Server.MapPath("/Log.txt")
            End If
            Return _LogPath
        End Get
    End Property
    Public Sub ErrorLogIns([error] As Exception, Optional ByVal Request As Object = Nothing, Optional ModuleName As String = Nothing, Optional ConnectionName As String = "ICELogConnection", Optional errorLogTypeVCode As Enums.ErrorLogTypeEnum = Enums.ErrorLogTypeEnum.DEFAULT_TYPE)
        Try
            Dim err As Exception = [error]
            Dim errorCode As Integer = 500
            Dim message = String.Empty
            Dim stackTrace As String = If(([error].StackTrace Is Nothing), "", Regex.Replace([error].StackTrace.Replace("'", """"), "\t|\n|\r", ""))
            While err IsNot Nothing
                Try
                    errorCode = CType(err, Win32Exception).ErrorCode
                Catch ex As Exception
                End Try
                If err.Message.IndexOf("Exception of type 'System.Web.HttpUnhandledException' was thrown", StringComparison.Ordinal) = -1 Then
                    If (message.Length > 0) Then
                        message = "\t\n" + message
                    End If
                    message = err.Message + message
                End If
                err = err.InnerException
            End While
            message = Regex.Replace(message.Replace("'", """"), "\t|\n|\r", "")
            If message.Length > 1000 Then
                message = message.Substring(0, 1000)
            End If
            Dim errorlog = New ErrorLogData
            errorlog.Code = errorCode
            errorlog.Message = message
            errorlog.StackTrace = stackTrace
            errorlog.Source = [error].Source
            errorlog.IP = String.Empty
            errorlog.RequestUrl = String.Empty
            errorlog.WebBrowser = String.Empty
            errorlog.ErrorLogTypeVCode = errorLogTypeVCode
            Dim st As New StackTrace(True)
            st = New StackTrace([error], True)
            Dim frames = st.GetFrames()
            Dim traceString = New StringBuilder()
            If frames Is Nothing OrElse frames.Count() = 0 Then
                traceString.Append("Ice: Frames object is nothing or count is zero")
            Else
                For Each frame As StackFrame In frames
                    If (frame.GetFileLineNumber() < 1) Then
                        Continue For
                    End If
                    traceString.Append("File: " & frame.GetFileName())
                    traceString.Append(", Method:" & frame.GetMethod().Name)
                    traceString.Append(", LineNumber: " & frame.GetFileLineNumber())
                    traceString.Append("  -->  ")
                Next
            End If

            If traceString.ToString().Length = 0 Then
                traceString.Append("Ice: traceString is empty")
            End If

            'errorlog.ErrorLine = "Line: " & st.GetFrame(0).GetFileLineNumber().ToString

            errorlog.ErrorLine = traceString.ToString()
            errorlog.MethodName = [error].TargetSite.Name
            If String.IsNullOrEmpty(ModuleName) Then
                errorlog.ModuleName = If([error].TargetSite.ReflectedType Is Nothing OrElse String.IsNullOrEmpty([error].TargetSite.ReflectedType.FullName), [error].TargetSite.Module.Name, [error].TargetSite.ReflectedType.FullName)
            Else
                errorlog.ModuleName = ModuleName
            End If
            ErrorLogIns(errorlog:=errorlog, ConnectionName:=ConnectionName)
        Catch ex As Exception
            LogOnText([error]:=[error])
        End Try
    End Sub
    Public Sub ErrorLogIns(errorlog As ErrorLogData, Optional ConnectionName As String = "ICELogConnection")
        Dim _command As SqlCommand = Nothing
        Try
            _command = New SqlCommand("AZ.ErrorLogIns", New SqlConnection(ConfigurationManager.ConnectionStrings(ConnectionName).ConnectionString)) _
            With {.CommandType = CommandType.StoredProcedure}
            _command.Connection.Open()
            _command.CommandTimeout = 30
            _command.Parameters.Add("@ErrorCode", SqlDbType.Int).Value = errorlog.Code
            _command.Parameters.Add("@IP", SqlDbType.VarChar, 30).Value = errorlog.IP
            _command.Parameters.Add("@Message", SqlDbType.VarChar).Value = errorlog.Message
            _command.Parameters.Add("@StackTrace", SqlDbType.VarChar).Value = errorlog.StackTrace
            _command.Parameters.Add("@RequestUrl", SqlDbType.VarChar, 300).Value = errorlog.RequestUrl
            _command.Parameters.Add("@Source", SqlDbType.VarChar, 200).Value = If(errorlog.Source Is Nothing, "", errorlog.Source)
            _command.Parameters.Add("@WebBrowser", SqlDbType.VarChar, 50).Value = errorlog.WebBrowser
            _command.Parameters.Add("@VisitLogCode", SqlDbType.UniqueIdentifier).Value = errorlog.VisitLogCode
            _command.Parameters.Add("@ErrorLine", SqlDbType.VarChar, 100).Value = errorlog.ErrorLine
            _command.Parameters.Add("@MethodName", SqlDbType.VarChar, 200).Value = errorlog.MethodName
            _command.Parameters.Add("@ModuleName", SqlDbType.VarChar, 200).Value = errorlog.ModuleName
            _command.CommandTimeout = 5
            _command.ExecuteNonQuery()
        Catch ex As Exception
            LogOnText(errorlog:=errorlog)
        Finally
            If _command IsNot Nothing Then
                _command.Connection.Close()
                _command.Connection.Dispose()
                _command.Dispose()
            End If
        End Try
    End Sub
    Public Sub ClientErrorLogIns(clientErrorlog As ClientErrorData)
        Dim _command As SqlCommand = Nothing
        Try
            _command = New SqlCommand("AZ.ClientErrorLogIns", New SqlConnection(ConfigurationManager.ConnectionStrings("ICELogConnection").ConnectionString)) _
            With {.CommandType = CommandType.StoredProcedure}
            _command.Connection.Open()
            _command.CommandTimeout = 30
            _command.Parameters.Add("@VisitLogCode", SqlDbType.UniqueIdentifier).Value = clientErrorlog.visitLogCode
            _command.Parameters.Add("@Column", SqlDbType.VarChar, 16).Value = If(String.IsNullOrEmpty(clientErrorlog.column), 0, clientErrorlog.column)
            _command.Parameters.Add("@LineNumber", SqlDbType.VarChar, 16).Value = If(String.IsNullOrEmpty(clientErrorlog.lineNumber), 0, clientErrorlog.lineNumber)
            _command.Parameters.Add("@Message", SqlDbType.VarChar).Value = If(String.IsNullOrEmpty(clientErrorlog.message), 0, clientErrorlog.message)
            _command.Parameters.Add("@Stack", SqlDbType.VarChar).Value = If(String.IsNullOrEmpty(clientErrorlog.stack), String.Empty, clientErrorlog.stack)
            _command.Parameters.Add("@Url", SqlDbType.VarChar, 256).Value = If(String.IsNullOrEmpty(clientErrorlog.url), String.Empty, clientErrorlog.url)
            _command.Parameters.Add("@ErrorDate", SqlDbType.DateTime).Value = clientErrorlog.errorDate
            _command.Parameters.Add("@IsMobileApp", SqlDbType.Bit).Value = clientErrorlog.isMobileApp
            _command.Parameters.Add("@IsMobileDevice", SqlDbType.Bit).Value = clientErrorlog.isMobileDevice
            _command.CommandTimeout = 5
            _command.ExecuteNonQuery()
        Catch ex As Exception
            Throw
        Finally
            If _command IsNot Nothing Then
                _command.Connection.Close()
                _command.Connection.Dispose()
                _command.Dispose()
            End If
        End Try
    End Sub
    Public Sub LogOnText([error] As Exception)
        Try
            If Not String.IsNullOrEmpty(LogPath) Then
                System.IO.File.AppendAllText(LogPath, Date.Now.ToString() & " : " & JsonConvert.SerializeObject([error], New JsonSerializerSettings With {.ReferenceLoopHandling = ReferenceLoopHandling.Ignore}) & Environment.NewLine)
            End If
        Catch ex As Exception
        End Try
    End Sub
    Public Sub LogOnText(errorlog As Object)
        Try
            If Not String.IsNullOrEmpty(LogPath) Then
                System.IO.File.AppendAllText(LogPath, Date.Now.ToString() & " : " & JsonConvert.SerializeObject(errorlog, New JsonSerializerSettings With {.ReferenceLoopHandling = ReferenceLoopHandling.Ignore}) & Environment.NewLine)
            End If
        Catch ex As Exception
        End Try
    End Sub
End Class