Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Web
Imports ARCO.Business.Models

Public Class LogBusiness
    Public Shared Function IsMobileAppFN(Optional ByRef Cookies As HttpCookieCollection = Nothing, Optional ByVal UserAgent As String = Nothing, Optional ByRef isAppleDevice As Boolean = False) As Boolean
        If Cookies Is Nothing Then
            If HttpContext.Current IsNot Nothing Then
                Cookies = HttpContext.Current.Request.Cookies
            Else
                Return False
            End If
        End If
        If Cookies Is Nothing Then
            Return False
        End If
        Return LocalIsMobileApp(Cookies, UserAgent, isAppleDevice)
    End Function
    Friend Shared Function IsAjaxRequest(request As System.Web.HttpRequest) As Boolean
        If (request("X-Requested-With") = "XMLHttpRequest") Then
            Return True
        End If
        If request.Headers IsNot Nothing Then
            Return (request.Headers("X-Requested-With") = "XMLHttpRequest")
        End If
        Return False
    End Function
    Private Shared Function LocalIsMobileApp(ByVal Cookies As HttpCookieCollection, ByVal UserAgent As String, ByRef isAppleDevice As Boolean) As Boolean
        Dim objCookie = Cookies("SystemMode")
        If objCookie IsNot Nothing Then
            If objCookie.Value IsNot Nothing Then
                If objCookie.Value = "1" Then
                    If Not String.IsNullOrEmpty(UserAgent) Then
                        isAppleDevice = IsAppleDevicee(UserAgent:=UserAgent)
                    End If
                    Return True
                End If
            End If
        End If
        Return False
    End Function
    Public Shared Function IsAppleDevicee(Optional ByVal UserAgent As String = Nothing) As Boolean
        If String.IsNullOrEmpty(UserAgent) Then
            UserAgent = HttpContext.Current.Request.UserAgent
        End If
        If UserAgent.IndexOf("iphone", StringComparison.CurrentCultureIgnoreCase) >= 0 OrElse UserAgent.IndexOf("ipad", StringComparison.CurrentCultureIgnoreCase) >= 0 OrElse (UserAgent.IndexOf("CFNetwork", StringComparison.CurrentCultureIgnoreCase) >= 0 AndAlso UserAgent.IndexOf("Darwin", StringComparison.CurrentCultureIgnoreCase) >= 0) Then
            Return True
        End If
        Return False
    End Function
    Public Function GetLogData(request As System.Web.HttpRequest, response As System.Web.HttpResponse, server As System.Web.HttpServerUtility) As VisitLogData
        Dim IP = request.ServerVariables("REMOTE_ADDR")
        If IP = "::1" Then
            IP = "127.0.0.1"
        End If
        Dim objLogData = New VisitLogData
        objLogData.Code = Guid.NewGuid()
        HttpContext.Current.Items.Add("VisitLogCode", objLogData.Code)
        objLogData.Url = request.Url.ToString()
        objLogData.IP = IP
        objLogData.Browser = request.Browser.Id & " " & request.Browser.MajorVersion & "." & request.Browser.MinorVersion
        If request.UrlReferrer IsNot Nothing Then
            objLogData.ReferrerUrl = request.UrlReferrer.ToString()
        Else
            objLogData.ReferrerUrl = ""
        End If
        Dim isMobileApp As Boolean
        Dim isAppleDevice As Boolean
        isMobileApp = IsMobileAppFN(request.Cookies, request.UserAgent, isAppleDevice)
        Dim guidCookie = request.Cookies("GUID")
        Dim myGuid As Guid
        If guidCookie Is Nothing Then
            myGuid = Guid.NewGuid
            response.Cookies.Add(New HttpCookie("GUID") With {.Value = myGuid.ToString(), .Expires = DateTime.Today.AddYears(1)})
        Else
            myGuid = Guid.Parse(guidCookie.Value)
        End If
        objLogData.Guid = myGuid
        Dim strForm As String = String.Empty
        Dim keys As String() = request.Form.AllKeys
        For i = 0 To keys.Length - 1
            strForm &= "|" & keys(i) & ":" & request.Form(keys(i))
        Next
        objLogData.PostData = If(String.IsNullOrEmpty(strForm), String.Empty, strForm.Substring(1))

        Dim strCookies As String = String.Empty
        keys = request.Cookies.AllKeys
        For i = 0 To keys.Length - 1
            strCookies = strCookies & "|" & keys(i) & ":" & request.Cookies(keys(i)).Value
        Next
        objLogData.RequestCookies = If(String.IsNullOrEmpty(strCookies), String.Empty, strCookies.Substring(1))
        objLogData.DeviceTypeVCode = If(isMobileApp, If(isAppleDevice, ARCO.Entities.Enums.DeviceTypeEnum.IOS, ARCO.Entities.Enums.DeviceTypeEnum.ANDROID), ARCO.Entities.Enums.DeviceTypeEnum.WEBSITE)
        objLogData.IsAjaxRequest = IsAjaxRequest(request:=request)
        objLogData.PostData = server.UrlDecode(request.Form.ToString())
        objLogData.RegisterDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")
        Return objLogData
    End Function
    Public Sub VisitLogIns(visitlog As VisitLogData, Optional ConnectionName As String = "ICELogConnection")
        Dim _command As SqlCommand = Nothing
        Try
            _command = New SqlCommand("AZ.VisitLogIns", New SqlConnection(ConfigurationManager.ConnectionStrings(ConnectionName).ConnectionString)) _
            With {.CommandType = CommandType.StoredProcedure}
            _command.Connection.Open()
            _command.CommandTimeout = 30
            _command.Parameters.Add("@Code", SqlDbType.UniqueIdentifier).Value = visitlog.Code
            _command.Parameters.Add("@Url", SqlDbType.VarChar, 300).Value = visitlog.Url
            _command.Parameters.Add("@IP", SqlDbType.VarChar, 30).Value = visitlog.IP
            _command.Parameters.Add("@Browser", SqlDbType.VarChar, 100).Value = visitlog.Browser
            _command.Parameters.Add("@ReferrerUrl", SqlDbType.VarChar, 300).Value = visitlog.ReferrerUrl
            _command.Parameters.Add("@UserVCode", SqlDbType.Int).Value = If(visitlog.UserVCode = 0, DBNull.Value, visitlog.UserVCode)
            _command.Parameters.Add("@Guid", SqlDbType.UniqueIdentifier).Value = visitlog.Guid
            _command.Parameters.Add("@LoadDate", SqlDbType.DateTime).Value = If(String.IsNullOrEmpty(visitlog.LoadDate), DBNull.Value, visitlog.LoadDate)
            _command.Parameters.Add("@DropDate", SqlDbType.DateTime).Value = If(String.IsNullOrEmpty(visitlog.DropDate), DBNull.Value, visitlog.DropDate)
            _command.Parameters.Add("@DeviceTypeVCode", SqlDbType.Int).Value = visitlog.DeviceTypeVCode
            _command.Parameters.Add("@IsAjaxRequest", SqlDbType.Bit).Value = visitlog.IsAjaxRequest
            _command.Parameters.Add("@PageTitle", SqlDbType.VarChar, 256).Value = If(String.IsNullOrEmpty(visitlog.PageTitle), String.Empty, visitlog.PageTitle)
            _command.Parameters.Add("@PostData", SqlDbType.VarChar).Value = visitlog.PostData
            _command.Parameters.Add("@RegisterDate", SqlDbType.DateTime).Value = visitlog.RegisterDate
            _command.Parameters.Add("@IP_API_COM", SqlDbType.VarChar).Value = visitlog.IpApiCom
            _command.Parameters.Add("@RequestCookies", SqlDbType.VarChar).Value = visitlog.RequestCookies
            _command.CommandTimeout = 30
            _command.ExecuteNonQuery()
        Catch ex As Exception
            Throw ex
        Finally
            If _command IsNot Nothing Then
                _command.Connection.Close()
                _command.Connection.Dispose()
                _command.Dispose()
            End If
        End Try
    End Sub
    Public Sub ApiLogIns(apilog As ApiLogData, Optional ConnectionName As String = "ICELogConnection")
        Dim _command As SqlCommand = Nothing
        Try
            _command = New SqlCommand("AZ.ApiLogIns", New SqlConnection(ConfigurationManager.ConnectionStrings(ConnectionName).ConnectionString)) _
            With {.CommandType = CommandType.StoredProcedure}
            _command.Connection.Open()
            _command.CommandTimeout = 30
            _command.Parameters.Add("@Request", SqlDbType.NVarChar).Value = apilog.Request
            _command.Parameters.Add("@Response", SqlDbType.NVarChar).Value = apilog.Response
            _command.Parameters.Add("@State", SqlDbType.NVarChar).Value = apilog.State
            _command.Parameters.Add("@IP", SqlDbType.VarChar, 30).Value = apilog.IP
            _command.Parameters.Add("@UserVCode", SqlDbType.BigInt).Value = apilog.UserVCode
            _command.Parameters.Add("@MethodName", SqlDbType.NVarChar).Value = apilog.MethodName
            _command.Parameters.Add("@Token", SqlDbType.NVarChar).Value = If(String.IsNullOrEmpty(apilog.Token), DBNull.Value, apilog.Token)
            _command.CommandTimeout = 30
            _command.ExecuteNonQuery()
        Catch ex As Exception
            Throw ex
        Finally
            If _command IsNot Nothing Then
                _command.Connection.Close()
                _command.Connection.Dispose()
                _command.Dispose()
            End If
        End Try
    End Sub
End Class
