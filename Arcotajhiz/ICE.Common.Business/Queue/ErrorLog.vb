Imports System.Collections.Concurrent
Imports System.Threading
Imports ARCO
Imports ICE.Common.Business.Models

Namespace Queue
    Public Class ErrorLog

#Region "Singleton Pattern"
        Private Shared ReadOnly _lockObj As New Object
        Private Shared _instance As ErrorLog = Nothing

        Public Shared ReadOnly Property Instance As ErrorLog
            Get
                If _instance Is Nothing Then
                    SyncLock _lockObj
                        _instance = New ErrorLog()
                    End SyncLock
                End If
                Return _instance
            End Get
        End Property

        Private Sub New()
            _waitingErrorLogs = New ConcurrentQueue(Of ErrorLogData)
            _waitingErrorLogsException = New ConcurrentQueue(Of Object)
            _waitingErrorLogsClient = New ConcurrentQueue(Of ClientErrorData)
            _workingPool = New List(Of Task)
            _ErrorLogListener = New Task(AddressOf Listener)
            _pendingErrorLog = New ManualResetEvent(False)
            _ErrorLogListener.Start()
        End Sub

#End Region
        'Private _key As String = "49a16d0e6c49d00ab69fe39c42facf0a"
        Private IpApiCom As Dictionary(Of String, String) = Nothing
        Private _waitingErrorLogs As ConcurrentQueue(Of ErrorLogData)
        Private _waitingErrorLogsException As ConcurrentQueue(Of Object)
        Private _waitingErrorLogsClient As ConcurrentQueue(Of ClientErrorData)
        Private ReadOnly _ErrorLogListener As Task
        Private _workingPool As List(Of Task)
        Private ReadOnly _pendingErrorLog As ManualResetEvent
        Private _stopRequested As Boolean = False

        Private Sub Listener()
            Do
                _pendingErrorLog.WaitOne()
                _workingPool.ForEach(Sub(ErrorLog) If ErrorLog.Status = TaskStatus.RanToCompletion Then ErrorLog.Dispose())
                _workingPool.RemoveAll(Function(ErrorLog) ErrorLog.Status = TaskStatus.RanToCompletion)
                If _stopRequested Then Exit Do
                Dim data As ErrorLogData = Nothing
                If _waitingErrorLogs.TryDequeue(data) Then
                    Dim ErrorLog As New Task(Sub()
                                                 Engine.Instance.ErrorBusiness.ErrorLogIns(errorlog:=data)
                                             End Sub)
                    ErrorLog.Start()
                    _workingPool.Add(ErrorLog)
                Else
                    _pendingErrorLog.Reset()
                End If
                Dim data2 As Object = Nothing
                If _waitingErrorLogsException.TryDequeue(data2) Then
                    Dim ErrorLog As New Task(Sub()
                                                 Engine.Instance.ErrorBusiness.ErrorLogIns(error:=data2.error, Request:=data2.request, ModuleName:=data2.ModuleName)
                                             End Sub)
                    ErrorLog.Start()
                    _workingPool.Add(ErrorLog)
                Else
                    _pendingErrorLog.Reset()
                End If
                Dim data3 As ClientErrorData = Nothing
                If _waitingErrorLogsClient.TryDequeue(data3) Then
                    Dim ErrorLog As New Task(Sub()
                                                 Engine.Instance.ErrorBusiness.ClientErrorLogIns(clientErrorlog:=data3)
                                             End Sub)
                    ErrorLog.Start()
                    _workingPool.Add(ErrorLog)
                Else
                    _pendingErrorLog.Reset()
                End If
            Loop
        End Sub
        Public Sub Insert(errorlog As ErrorLogData)
            _waitingErrorLogs.Enqueue(errorlog)
            _pendingErrorLog.Set()
        End Sub
        Public Sub Insert([error] As Exception, Optional ByVal Request As Object = Nothing, Optional ModuleName As String = Nothing, Optional ErrorLogTypeVCode As ARCO.Entities.Enums.ErrorLogTypeEnum = Entities.Enums.ErrorLogTypeEnum.DEFAULT_TYPE)
            _waitingErrorLogsException.Enqueue(New With {.error = [error], .request = Request, .ModuleName = ModuleName})
            _pendingErrorLog.Set()
        End Sub
        Public Sub Insert(clientErrorlog As ClientErrorData)
            _waitingErrorLogsClient.Enqueue(clientErrorlog)
            _pendingErrorLog.Set()
        End Sub
        Public Sub Dispose()
            _stopRequested = True
            _pendingErrorLog.Set()
            While _ErrorLogListener.Status = TaskStatus.Running
                Thread.Sleep(1000)
            End While
            _ErrorLogListener.Dispose()
            _waitingErrorLogs = Nothing
            _workingPool = Nothing
            GC.SuppressFinalize(Me)
        End Sub
    End Class
End Namespace