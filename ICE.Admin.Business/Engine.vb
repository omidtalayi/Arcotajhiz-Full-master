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
    Private _contentBusiness As ContentBusinesses
    Private _settingBusiness As SettingBusiness
    Private Sub Initializer()
        _contentBusiness = New ContentBusinesses
        _settingBusiness = New SettingBusiness
    End Sub
    Public ReadOnly Property ContentBusiness As ContentBusinesses
        Get
            Return _contentBusiness
        End Get
    End Property
    Public ReadOnly Property SettingBusiness As SettingBusiness
        Get
            Return _settingBusiness
        End Get
    End Property
#End Region

End Class