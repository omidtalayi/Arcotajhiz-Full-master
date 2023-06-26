Imports ARCO.Business.Businesses

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
    Private _identificationBusiness As IdentificationBusiness
    Private _callBusiness As CallBusiness
    Private _logBusiness As LogBusiness
    Private _contactBusiness As ContactUsBusiness
    Private _userBusiness As UserBusiness
    Private _applicationBusiness As ApplicationBusiness
    Private _notificationBusiness As NotificationBusiness
    Private _complaintBusiness As ComplaintBusiness
    Private _contentBusiness As ContentBusiness
    Private _cryptographyBusiness As CryptographyBusiness
    Private _productBusiness As ProductBusiness
    Private Sub Initializer()
        _identificationBusiness = New IdentificationBusiness
        _logBusiness = New LogBusiness
        _contactBusiness = New ContactUsBusiness
        _userBusiness = New UserBusiness
        _applicationBusiness = New ApplicationBusiness
        _notificationBusiness = New NotificationBusiness
        _complaintBusiness = New ComplaintBusiness
        _contentBusiness = New ContentBusiness
        _cryptographyBusiness = New CryptographyBusiness
        _productBusiness = New ProductBusiness
    End Sub
    Public ReadOnly Property ApplicationBusiness As ApplicationBusiness
        Get
            Return _applicationBusiness
        End Get
    End Property
    Public ReadOnly Property IdentificationBusiness As IdentificationBusiness
        Get
            Return _identificationBusiness
        End Get
    End Property
    Public ReadOnly Property LogBusiness As LogBusiness
        Get
            Return _logBusiness
        End Get
    End Property
    Public ReadOnly Property ContactUsBusiness As ContactUsBusiness
        Get
            Return _contactBusiness
        End Get
    End Property

    Public ReadOnly Property UserBusiness As UserBusiness
        Get
            Return _userBusiness
        End Get
    End Property
    Public ReadOnly Property NotificationBusiness As NotificationBusiness
        Get
            Return _notificationBusiness
        End Get
    End Property
    Public ReadOnly Property ComplaintBusiness As ComplaintBusiness
        Get
            Return _complaintBusiness
        End Get
    End Property
    Public ReadOnly Property ContentBusiness As ContentBusiness
        Get
            Return _contentBusiness
        End Get
    End Property
    Public ReadOnly Property CryptographyBusiness As CryptographyBusiness
        Get
            Return _cryptographyBusiness
        End Get
    End Property
    Public ReadOnly Property ProductBusiness As ProductBusiness
        Get
            Return _productBusiness
        End Get
    End Property
#End Region

End Class