Public Class Engine

#Region "Singleton Pattern"
    Private Shared ReadOnly _LockObj As New Object()
    Private Shared _Instance As Engine = Nothing
    Private Sub New()
        Initializer()
    End Sub
    Public Shared ReadOnly Property Instance As Engine
        Get
            If _Instance Is Nothing Then
                SyncLock _LockObj
                    If _Instance Is Nothing Then _Instance = New Engine()
                End SyncLock
            End If
            Return _Instance
        End Get
    End Property
#End Region
#Region "Businesses"
    Private _errorBusiness As ErrorBusiness
    Private Sub Initializer()
        _errorBusiness = New ErrorBusiness
    End Sub
    Public ReadOnly Property ErrorBusiness As ErrorBusiness
        Get
            Return _errorBusiness
        End Get
    End Property
#End Region

End Class