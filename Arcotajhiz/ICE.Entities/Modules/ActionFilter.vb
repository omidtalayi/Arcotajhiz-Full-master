Imports System.IO
Imports System.Net
Imports System.Web.Mvc
Imports System.Web

Namespace Modules
    Public Class UserAuthorize
        Inherits ActionFilterAttribute
        Public Overrides Sub OnActionExecuting(filterContext As ActionExecutingContext)
            Dim IsAuthorized = False
            If HttpContext.Current IsNot Nothing AndAlso HttpContext.Current.User IsNot Nothing AndAlso HttpContext.Current.User.Identity IsNot Nothing AndAlso HttpContext.Current.User.Identity.IsAuthenticated Then
                IsAuthorized = True
            End If
            If Not IsAuthorized Then
                filterContext.Result = New HttpStatusCodeResult(401)
            End If
            'CheckAppAuthorize(filterContext:=filterContext, SubSystem:=SubSystemEnum.BUYER)
        End Sub
    End Class
    Public Class AjaxActionFilter
        Inherits ActionFilterAttribute
        Public Overrides Sub OnActionExecuting(filterContext As ActionExecutingContext)
            If Not filterContext.HttpContext.Request.IsAjaxRequest() Then
                filterContext.Result = New HttpStatusCodeResult(403)
            End If
        End Sub
    End Class
    'Public Class IranOnly
    '    Inherits ActionFilterAttribute
    '    Public Overrides Sub OnActionExecuting(filterContext As ActionExecutingContext)
    '        If Not filterContext.HttpContext.Request.IsLocal Then
    '            Dim isIran As Integer = 0
    '            Try
    '                Dim IP = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")
    '                If String.Compare(IP, "::1", True) = 0 OrElse String.Compare(IP, "127.0.0.1", True) = 0 Then
    '                    Return
    '                End If
    '                Dim IpApiCom As Dictionary(Of String, String) = HttpContext.Current.Application("IpApiCom")
    '                Dim j As JObject = Nothing
    '                Dim strResult As String = Nothing
    '                If Not IpApiCom.TryGetValue(IP, strResult) Then
    '                    Dim req = WebRequest.Create("http://ip-api.com/json/" & IP)
    '                    Dim res = CType(req.GetResponse(), HttpWebResponse)
    '                    Using sr = New StreamReader(res.GetResponseStream())
    '                        strResult = sr.ReadToEnd()
    '                    End Using
    '                    IpApiCom.Add(IP, strResult)
    '                End If
    '                If Not String.IsNullOrEmpty(strResult) Then
    '                    j = Newtonsoft.Json.Linq.JObject.Parse(strResult)
    '                    If String.Compare(j("countryCode").ToString(), "IR", True) = 0 Then
    '                        isIran = 1
    '                    End If
    '                End If
    '            Catch ex As Exception
    '            End Try
    '            If isIran <> 1 Then
    '                filterContext.Result = New HttpStatusCodeResult(403)
    '            End If
    '        End If
    '    End Sub
    'End Class
End Namespace