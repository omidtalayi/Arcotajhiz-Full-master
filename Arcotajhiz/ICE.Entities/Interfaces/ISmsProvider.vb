Imports System.Collections.Generic

Namespace Interfaces
    Public Interface ISmsProvider
        Function LoadAccountInformation() As Boolean
        ReadOnly Property CenterNumbers() As List(Of String)
        Sub SendSms(phoneNumber As String, message As String, LogVCode As Long)
        Sub SendSms(centerNumber As String, phoneNumber As String, message As String, LogVCode As Long)
    End Interface
End Namespace