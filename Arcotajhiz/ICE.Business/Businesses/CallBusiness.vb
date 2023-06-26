Imports ARCO.Business.Models
Imports RestSharp
Imports System.Collections.Concurrent
Imports System.Data.SqlClient
Imports System.Net
Imports System.Threading
Imports System.Web
Imports System.Web.Script.Serialization
Imports Microsoft.VisualBasic.CompilerServices
Imports Newtonsoft.Json.Linq

Public Class CallBusiness
    Implements IDisposable
#Region "Singleton Pattern"
    Private Shared ReadOnly _lockObj As New Object
    Private Shared _instance As CallBusiness = Nothing
    Public Shared ReadOnly Property Instance As CallBusiness
        Get
            If _instance Is Nothing Then
                SyncLock _lockObj
                    _instance = New CallBusiness
                End SyncLock
            End If
            Return _instance
        End Get
    End Property

    Private Sub New()
        _centerNumbers = New List(Of String)
        _waitingCellPhones = New ConcurrentQueue(Of CallData)
        _workingPool = New List(Of Task)
        _cellPhoneListener = New Task(AddressOf Listener)
        _pendingCellPhone = New ManualResetEvent(False)
        LoadAccountInformation()
        _cellPhoneListener.Start()
    End Sub
    Private _voiceID As String = String.Empty
    Private _url As String = String.Empty
    Private _apiKey As String = String.Empty
    Private ReadOnly _centerNumbers As List(Of String) = Nothing
    Private _waitingCellPhones As ConcurrentQueue(Of CallData)
    Private ReadOnly _cellPhoneListener As Task
    Private _workingPool As List(Of Task)
    Private ReadOnly _pendingCellPhone As ManualResetEvent
    Private _stopRequested As Boolean = False
    Private _validData As Boolean = False
    Public _providerID As Long
    Private Sub Listener()
        Do
            _pendingCellPhone.WaitOne()
            _workingPool.ForEach(Sub(phoneCall) If phoneCall.Status = TaskStatus.RanToCompletion Then phoneCall.Dispose())
            _workingPool.RemoveAll(Function(phoneCall) phoneCall.Status = TaskStatus.RanToCompletion)
            If _stopRequested Then Exit Do
            Dim cellPhone As CallData = Nothing
            If _waitingCellPhones.TryDequeue(cellPhone) Then
                Dim phoneCall As New Task(Sub()
                                              Try
                                                  MakePhoneCall(cellPhone)
                                              Catch ex As Exception
                                                  Throw ex
                                              End Try
                                          End Sub)
                phoneCall.Start()
                _workingPool.Add(phoneCall)
            Else
                _pendingCellPhone.Reset()
            End If
        Loop
    End Sub
    Public Sub MakePhoneCall(phoneNumber As String,Optional message As String = Nothing)
        If Not _validData Then Return
        _waitingCellPhones.Enqueue(New CallData() With {.Phone = phoneNumber, .Message = message})
        _pendingCellPhone.Set()
    End Sub
    Private Function MakePhoneCall(phoneCall As CallData) As String
        Try
            Dim success = False
            If Not ((phoneCall.Phone.Length = 11 AndAlso phoneCall.Phone.ToString.Substring(0, 2) = "09") OrElse (phoneCall.Phone.Length = 10 AndAlso phoneCall.Phone.ToString.Substring(0, 1) = "9")) Then
                Return -1
            End If
            Dim request = New RestRequest(Method.POST)
            Dim url As String = _url & "/voice/single"
            Dim token As String = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMDExMCIsImV4cCI6MTg3NDczNTA3MiwiaXNzIjoiQml0ZWwiLCJhdWQiOiJCaXRlbCJ9.t0qU-kTCnOYn10-i77YGfodmi8CLoQ8M9RzmoiaRz_A"
            request.Parameters.Clear()
            If Not String.IsNullOrEmpty(phoneCall.Message) Then
                url = _url & "/voice/tts"
                request.AddParameter("application/json", "{""phoneNumber"": """ & phoneCall.Phone & """,""text"" : """ & phoneCall.Message & """}", ParameterType.RequestBody)
            Else
                If String.IsNullOrEmpty(phoneCall.VoiceID) Then
                    phoneCall.VoiceID = _voiceID
                End If
                request.AddParameter("application/json", "{""phoneNumber"": """ & phoneCall.Phone & """,""voiceId"" : """ & phoneCall.VoiceID & """}", ParameterType.RequestBody)
            End If
            Dim client = New RestClient(url)
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
            request.AddHeader("Accept", "application/json")
            request.AddHeader("Authorization", "Bearer " & _apiKey)
            Dim response = client.Execute(request)
            Dim jobj = JObject.Parse(response.Content)
            Dim responseResult = jobj("result")
            Dim responseErrors = jobj("error")
            success = Not String.IsNullOrEmpty(responseResult)
            'Biar.Business.Common.Engine.Instance.NotificationBusiness.BitelLogIns(phone:= phone,storeVCode := storeVCode,responseID := responseResult,err := responseErrors,voiceID := voiceId,message:=message)
            If success Then
                Return responseResult
            Else
                Return "-1"
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function LoadAccountInformation() As Boolean
        Dim command As SqlCommand = Nothing
        Dim reader As SqlDataReader = Nothing
        Try
            command = ARCOConnection.Command("AZ.GetCallProviderSetting")
            reader = command.ExecuteReader()
            If reader.Read() Then
                _centerNumbers.Clear()
                _apiKey = reader("CALLAPIKey")
                _voiceID = reader("CALLVoiceID")
                _url = reader("CALLAPIUrl")
            End If
            If reader("CALLAPIKey").ToString() = String.Empty OrElse reader("CALLVoiceID").ToString() = String.Empty OrElse reader("CALLAPIUrl").ToString() = String.Empty Then
                _validData = False
            Else
                _validData = True
            End If
            Return True
        Catch ex As Exception
            Throw
        Finally
            If reader IsNot Nothing Then
                reader.Close()
                reader.Dispose()
            End If
            If command IsNot Nothing Then
                command.Connection.Close()
                command.Connection.Dispose()
                command.Dispose()
            End If
        End Try
    End Function
#End Region
#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant phoneCalls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        disposedValue = True
    End Sub

    ' TODO: override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        ' TODO: uncomment the following line if Finalize() is overridden above.
        ' GC.SuppressFinalize(Me)
    End Sub
#End Region
End Class
