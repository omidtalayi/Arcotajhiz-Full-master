Imports ARCO.Business.Models
Imports RestSharp
Imports System.Collections.Concurrent
Imports System.Data.SqlClient
Imports System.Threading
Imports System.Web.Script.Serialization
Imports System.Web.WebPages.Html
Imports ARCO.Entities.Enums
Imports Microsoft.VisualBasic.CompilerServices

Namespace Businesses
    Public Class SmsBusiness
        Implements IDisposable
#Region "Singleton Pattern"

        Private Shared ReadOnly _lockObj As New Object
        Private Shared _instance As SmsBusiness = Nothing

        Public Shared ReadOnly Property Instance As SmsBusiness
            Get
                If _instance Is Nothing Then
                    SyncLock _lockObj
                        _instance = New SmsBusiness
                    End SyncLock
                End If
                Return _instance
            End Get
        End Property

        Private Sub New()
            _centerNumbers = New List(Of String)
            _waitingCellPhones = New ConcurrentQueue(Of SmsData)
            _workingPool = New List(Of Task)
            _cellPhoneListener = New Task(AddressOf Listener)
            _pendingCellPhone = New ManualResetEvent(False)
            LoadAccountInformation()
            _cellPhoneListener.Start()
        End Sub

#End Region

        Private _username As String = String.Empty
        Private _password As String = String.Empty
        Private _apiKey As String = String.Empty
        Private ReadOnly _centerNumbers As List(Of String) = Nothing
        Private _waitingCellPhones As ConcurrentQueue(Of SmsData)
        Private ReadOnly _cellPhoneListener As Task
        Private _workingPool As List(Of Task)
        Private ReadOnly _pendingCellPhone As ManualResetEvent
        Private _stopRequested As Boolean = False
        Private _validData As Boolean = False
        Public _providerID As Long
        Private Sub Listener()
            Do
                _pendingCellPhone.WaitOne()
                _workingPool.ForEach(Sub(sms) If sms.Status = TaskStatus.RanToCompletion Then sms.Dispose())
                _workingPool.RemoveAll(Function(sms) sms.Status = TaskStatus.RanToCompletion)
                If _stopRequested Then Exit Do
                Dim cellPhone As SmsData = Nothing
                If _waitingCellPhones.TryDequeue(cellPhone) Then
                    Dim sms As New Task(Sub()
                                            Try
                                                Select Case cellPhone.OperatorType
                                                    Case OperatorTypeEnum.GHASEDAK
                                                        SendSms(cellPhone)
                                                    Case OperatorTypeEnum.ICS_IRANSMS
                                                        SendSmsICS(cellPhone)
                                                End Select

                                            Catch ex As Exception
                                                Throw ex
                                            End Try
                                        End Sub)
                    sms.Start()
                    _workingPool.Add(sms)
                Else
                    _pendingCellPhone.Reset()
                End If
            Loop
        End Sub
        Public Sub SendSms(phoneNumber As String, message As String, LogVCode As Long, Optional identificationVCode As Long = 0, Optional typeVCode As SMSTypeEnum = SMSTypeEnum.NONE, Optional operatorType As OperatorTypeEnum = OperatorTypeEnum.GHASEDAK)
            If Not _validData Then Return
            _waitingCellPhones.Enqueue(New SmsData() With {.CenterNumber = _centerNumbers.FirstOrDefault, .Number = phoneNumber, .Message = message, .LogVCode = LogVCode, .Type = 2, .IdentificationVCode = identificationVCode, .TypeVCode = typeVCode, .OperatorType = operatorType})
            _pendingCellPhone.Set()
        End Sub
        Private Function SendSms(sms As SmsData, Optional tryCount As Integer = 0) As Long
            Try
                If Not ((sms.Number.Length = 11 AndAlso sms.Number.ToString.Substring(0, 2) = "09") OrElse (sms.Number.Length = 10 AndAlso sms.Number.ToString.Substring(0, 1) = "9") OrElse (sms.Number.Length = 12 AndAlso sms.Number.ToString.Substring(0, 2) = "98")) Then
                    Return -1
                End If
                Dim url As String = ARCO.Business.Engine.Instance.ApplicationBusiness.GetSetting("ParsaSmsUrl")
                Dim client = New RestClient(url)
                Dim request = New RestRequest(Method.POST)
                request.AddHeader("apikey", _apiKey)
                request.AddParameter("senddate", "unixtime")
                request.AddParameter("receptor", sms.Number)
                request.AddParameter("message", sms.Message)
                request.AddParameter("sender", _centerNumbers.FirstOrDefault)
                request.AddParameter("output", "xml/Json")
                Dim response As IRestResponse = client.Execute(request)
                Dim ResponseString = response.Content
                Dim s = New JavaScriptSerializer()
                Dim jObj = s.Deserialize(Of Object)(ResponseString)
                Dim success = (String.Compare(jObj("result"), "success", True) = 0)
                If success Then
                    Dim SMSLog = New SMSLogData() With
                            {
                                .EntryDate = DateTime.Now,
                                .FromNumber = _centerNumbers.FirstOrDefault.ToString(),
                                .IdentificationVCode = sms.IdentificationVCode,
                                .Message = sms.Message,
                                .SendStateMessage = "ارسال شده به سرویس sms",
                                .Send_ID = If(jObj("messageids") IsNot Nothing, CType(jObj("messageids"), Long), 0),
                                .State = 0,
                                .ToNumber = sms.Number,
                                .SMSLogTypeVCode = sms.TypeVCode
                            }
                    SmsResponseLogIns(SMSLog:=SMSLog)
                Else
                    _waitingCellPhones.Enqueue(New SmsData() With {.CenterNumber = _centerNumbers.FirstOrDefault, .Number = sms.Number, .Message = sms.Message, .LogVCode = sms.LogVCode, .Type = 2, .IdentificationVCode = sms.IdentificationVCode, .TypeVCode = sms.TypeVCode, .OperatorType = sms.OperatorType})
                    _pendingCellPhone.Set()
                End If
                If success Then
                    Return jObj("messageids")
                Else
                    Return -1
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Function
        Private Function SendSmsICS(sms As SmsData) As Long
            Try
                If Not ((sms.Number.Length = 11 AndAlso sms.Number.ToString.Substring(0, 2) = "09") OrElse (sms.Number.Length = 10 AndAlso sms.Number.ToString.Substring(0, 1) = "9") OrElse (sms.Number.Length = 12 AndAlso sms.Number.ToString.Substring(0, 2) = "98")) Then
                    Return -1
                End If
                Dim url As String = "https://RestfulSms.com/api/Token"
                Dim client = New RestClient(url)
                Dim RestRequest = New RestRequest(Method.POST)
                RestRequest.AddHeader("cache-control", "no-cache")
                RestRequest.AddHeader("Content-Type", "application/json")
                RestRequest.AddParameter("application/json; charset=utf-8", "{""UserApiKey"" : """ + "c4043514be4c2cc5d5002239" + """,""SecretKey"" : """ + "405060fhkCBM$&^" + """}", ParameterType.RequestBody)
                RestRequest.RequestFormat = DataFormat.Json
                Dim response As IRestResponse = client.Execute(RestRequest)
                Dim ResponseString = response.Content
                Dim s = New JavaScriptSerializer()
                Dim jObj = s.Deserialize(Of Object)(ResponseString)
                Dim success = CType(jObj("IsSuccessful"), Boolean)
                If success Then
                    url = "https://RestfulSms.com/api/MessageSend"
                    client = New RestClient(url)
                    RestRequest = New RestRequest(Method.POST)
                    RestRequest.AddHeader("cache-control", "no-cache")
                    RestRequest.AddHeader("Content-Type", "application/json")
                    RestRequest.AddHeader("x-sms-ir-secure-token", jObj("TokenKey"))
                    RestRequest.AddParameter("application/json; charset=utf-8", "{""Messages"" : [""" + sms.Message + """],""MobileNumbers"" : [""" + sms.Number + """],""LineNumber"" : ""30002187"",""SendDateTime"" : """",""CanContinueInCaseOfError"" : ""false""}", ParameterType.RequestBody)
                    RestRequest.RequestFormat = DataFormat.Json
                    response = client.Execute(RestRequest)
                    ResponseString = response.Content
                    s = New JavaScriptSerializer()
                    jObj = s.Deserialize(Of Object)(ResponseString)
                    success = CType(jObj("IsSuccessful"), Boolean)
                    If success Then
                        Dim SMSLog = New SMSLogData() With
                            {
                                .EntryDate = DateTime.Now,
                                .FromNumber = "30002187",
                                .IdentificationVCode = sms.IdentificationVCode,
                                .Message = sms.Message,
                                .SendStateMessage = "ارسال شده به سرویس sms",
                                .Send_ID = If(jObj("Ids") IsNot Nothing, CType(jObj("Ids")(0)("ID"), Long), 0),
                                .State = 0,
                                .ToNumber = sms.Number,
                                .SMSLogTypeVCode = sms.TypeVCode
                            }
                        SmsResponseLogIns(SMSLog:=SMSLog)
                        Return jObj("Ids")(0)("ID").ToString()
                    Else
                        Return -1
                    End If
                Else
                    Return -1
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Function
        Private Shared Function GetSmsLogData(reader As IDataRecord) As SMSLogData
            Try
                Dim rptTrackingUser = New SMSLogData With
                        {
                        .VCode = If(IsDBNull(reader("VCode")), 0, reader("VCode")),
                        .EntryDate = If(IsDBNull(reader("EntryDate")), DateTime.MinValue, reader("EntryDate")),
                        .FromNumber = If(IsDBNull(reader("SendFromNumber")), 0, reader("SendFromNumber")),
                        .ToNumber = If(IsDBNull(reader("SendToNumber")), 0, reader("SendToNumber")),
                        .IdentificationVCode = If(IsDBNull(reader("IdentificationVCode")), 0, reader("IdentificationVCode")),
                        .Message = If(IsDBNull(reader("SendMessage")), Nothing, reader("SendMessage")),
                        .SendStateMessage = If(IsDBNull(reader("SendDelivery")), Nothing, reader("SendDelivery")),
                        .Send_ID = If(IsDBNull(reader("SendID")), Nothing, reader("SendID")),
                        .State = If(IsDBNull(reader("SendState")), Nothing, reader("SendState")),
                        .SMSLogTypeVCode = If(IsDBNull(reader("SMSLogTypeVCode")), Nothing, reader("SMSLogTypeVCode"))
                        }
                Return rptTrackingUser
            Catch ex As Exception
                Throw ex
            End Try
        End Function
        Public Function LoadAccountInformation() As Boolean
            Dim command As SqlCommand = Nothing
            Dim reader As SqlDataReader = Nothing
            Try
                command = ARCOConnection.Command("AZ.GetSMSProviderSetting")
                reader = command.ExecuteReader()
                If reader.Read() Then
                    _centerNumbers.Clear()
                    _username = reader("Username")
                    _apiKey = reader("SMSAPIKey")
                    _password = reader("Password")
                    _centerNumbers.AddRange(reader("CenterNumbers").ToString().Split(";"))
                End If
                If reader("Username").ToString() = String.Empty OrElse reader("Password").ToString() = String.Empty OrElse reader("CenterNumbers").ToString() = String.Empty Then
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
        Public Shared Function SmsResponseLogIns(SMSLog As SMSLogData) As Boolean
            Dim command As SqlCommand = Nothing
            Try
                command = ARCOConnection.Command("AZ.SmsLogIns")
                command.Parameters("@VCode").Direction = ParameterDirection.Output
                command.Parameters("@Send_ID").Value = If(SMSLog.Send_ID <> 0, SMSLog.Send_ID, DBNull.Value)
                command.Parameters("@Send_FromNumber").Value = If(Not String.IsNullOrEmpty(SMSLog.ToNumber), SMSLog.FromNumber, DBNull.Value)
                command.Parameters("@Send_ToNumber").Value = If(Not String.IsNullOrEmpty(SMSLog.ToNumber), SMSLog.ToNumber, DBNull.Value)
                command.Parameters("@Send_Message").Value = If(Not String.IsNullOrEmpty(SMSLog.Message), SMSLog.Message, DBNull.Value)
                command.Parameters("@Send_State").Value = SMSLog.State
                command.Parameters("@Send_Delivery").Value = If(Not String.IsNullOrEmpty(SMSLog.SendStateMessage), SMSLog.SendStateMessage, DBNull.Value)
                command.Parameters("@IdentificationVCode").Value = If(SMSLog.IdentificationVCode <> 0, SMSLog.IdentificationVCode, DBNull.Value)
                command.Parameters("@SMSLogTypeVCode").Value = If(SMSLog.SMSLogTypeVCode <> SMSTypeEnum.NONE, CType(SMSLog.SMSLogTypeVCode,Integer), DBNull.Value)
                command.ExecuteNonQuery()
                SMSLog.VCode = command.Parameters("@VCode").Value
                Return True
            Catch ex As Exception
                Throw
            Finally
                If command IsNot Nothing Then
                    command.Connection.Close()
                    command.Connection.Dispose()
                    command.Dispose()
                End If
            End Try
        End Function
        Public Function GetSMSLogs(optional stateVCode As SMSLogStateEnum = SMSLogStateEnum.ON_QUEUE, Optional typeVCode As SMSTypeEnum = SMSTypeEnum.NONE) As List(Of SMSLogData)
            Dim SMSLogs As New List(Of SMSLogData)
            Dim command As SqlCommand = Nothing
            Dim reader As SqlDataReader = Nothing
            Try
                command = ARCOConnection.Command("AZ.GetSmsLogs")
                command.Parameters("@SMSLogStateVCode").Value = stateVCode
                command.Parameters("@SMSTypeVCode").Value = If(typeVCode = SMSTypeEnum.NONE, DBNull.Value, typeVCode)
                reader = command.ExecuteReader()
                While reader.Read()
                    SMSLogs.Add(GetSMSLogData(reader))
                End While
                Return SMSLogs
            Catch ex As Exception
                Throw ex
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

        Public Function UpdateSMSLogStates() As Boolean
            Try
                Dim smsLogs = Businesses.SmsBusiness.Instance.GetSMSLogs(stateVCode := SMSLogStateEnum.ON_QUEUE)
                If smsLogs IsNot Nothing AndAlso smsLogs.Any() Then
                    Dim takeIndex = 100
                    For i As Integer = 0 To Math.Ceiling((smsLogs.Count / takeIndex))
                        Dim skipCount = i * takeIndex
                        dim Send_IDs =New List(Of String)
                        Send_IDs.AddRange(smsLogs.Select(Function(x) x.Send_ID).ToArray().Skip(skipCount).Take(takeIndex))
                        If Send_IDs.Any() Then
                            Dim client = New RestClient("https://api.smsapp.ir/v2/sms/status")
                            Dim request = New RestRequest(Method.POST)
                            request.AddHeader("Content-Type", "application/json")
                            request.AddHeader("Cache-Control", "no-cache")
                            request.AddHeader("apikey", _apiKey)
                            request.AddParameter("application/x-www-form-urlencoded", "messageids=" & String.Join(",", Send_IDs), ParameterType.RequestBody)
                            Dim response As IRestResponse = client.Execute(request)
                            Dim s = New JavaScriptSerializer()
                            Dim jObj = s.Deserialize(Of Object)(response.Content)
                            Dim success = (String.Compare(jObj("result"), "success", True) = 0)
                            If success Then
                                Dim responseSmsLog = New List(Of SMSLogData)
                                Dim resultList = jObj("list").ToString().Split(",")
                                Dim smsLogsTemp = smsLogs.Skip(skipCount).Take(takeIndex).ToList()
                                responseSmsLog.AddRange(smsLogsTemp.Select(Function(x) New SMSLogData With{.VCode = x.VCode ,.State = CType(resultList(smsLogsTemp.IndexOf(x)),integer) }).ToList())
                                SmsLogStateUpd(responseSmsLog)
                            End If
                        End If
                    Next
                End If
                Return True
            Catch ex As Exception
                throw ex
            End Try
        End Function
        Private Shared Function ConvertSmsLogsDataToXml(smsLogs As List(Of SMSLogData)) As String
            Return smsLogs.Aggregate(String.Empty, Function(current, smsLog) current & String.Format("<SL V=""{0}"" S=""{1}""/>", smsLog.VCode,smsLog.State))
        End Function
        Public Shared Function SmsLogStateUpd(smsLogs As List(Of SMSLogData)) As Boolean
            Dim command As SqlCommand = Nothing
            Try
                command = ARCOConnection.Command("AZ.SmsLogStateUpd")
                command.Parameters("@Smslogs").Value = ConvertSmsLogsDataToXml(smsLogs)
                command.ExecuteNonQuery()
                Return True
            Catch ex As Exception
                Throw ex
            Finally
                If command IsNot Nothing Then
                    command.Connection.Close()
                    command.Connection.Dispose()
                    command.Dispose()
                End If
            End Try
        End Function
        Public Function GetReportHasBeenSent(identificationVCode As Long) As Boolean
            Dim command As SqlCommand = Nothing
            Try
                command = ARCOConnection.Command("AZ.GetReportHasBeenSent")
                command.Parameters("@IdentificationVCode").Value = identificationVCode
                Return Ctype(command.ExecuteScalar(),Boolean)
            Catch ex As Exception
                Throw ex
            Finally
                If command IsNot Nothing Then
                    command.Connection.Close()
                    command.Connection.Dispose()
                    command.Dispose()
                End If
            End Try
        End Function
#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

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
End Namespace