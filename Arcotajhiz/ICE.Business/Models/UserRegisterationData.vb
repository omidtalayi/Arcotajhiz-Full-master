Imports System.Runtime.Serialization
Imports ARCO.Entities.Enums

Namespace Models

    <DataContract>
    Public Class UserRegistrationData
        <DataMember> Public Property UserRegisterationState As UserRegistrationStateEnum
        Public Property User As ARCO.Business.Models.UserData
        Public ReadOnly Property UserRegisterationStateMessage() As String
            Get
                Select Case UserRegisterationState
                    Case UserRegistrationStateEnum.DUPLICATECELLPHONE
                        Return "شماره موبایل تکراری است."
                    Case UserRegistrationStateEnum.DUPLICATEEMAIL
                        Return "ایمیل تکراری است."
                    Case UserRegistrationStateEnum.DUPLICATEUSERNAME
                        Return "نام کاربری تکراری است"
                    Case UserRegistrationStateEnum.DUPLICATETRACKINGCODE
                        Return "لینک تکراری است"
                    Case UserRegistrationStateEnum.WEAKPASSWORD
                        Return "رمزعبور شما بسیار آسان است. لطفا تغییر دهید"
                    Case UserRegistrationStateEnum.WEAKUSERNAME
                        Return "نام کاربری شما بسیار آسان است. لطفا تغییر دهید"
                    Case Else
                        Return "در روند مشکلی ایجاد شده است"
                End Select
            End Get
        End Property
    End Class
End Namespace

