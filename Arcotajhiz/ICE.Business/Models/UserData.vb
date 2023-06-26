Imports System.Runtime.Serialization
Imports ARCO.Entities.Enums

Namespace Models

    <DataContract>
    <Serializable>
    Public Class UserData

        <DataMember> Public Property VCode As Integer
        <DataMember> Public Property Username As String
        <DataMember> Property Password As String
        Friend Property PasswordSalt As String
        <DataMember> Property IsApproved As Boolean
        <DataMember> Property IsLock As Boolean
        <DataMember> Public Property IsSubscribed As Boolean
        <DataMember> Public Property Email As String
        <DataMember> Public Property CellPhone As String
        Friend Property FailedAttemptCount As Byte?
        Friend Property LastFailedAttemptDate As DateTime?
        Friend Property LastLoginDate As DateTime?
        Friend Property EntryDate As DateTime
        Friend Property LastModifiedDate As DateTime
        <DataMember> Property Token As String
        <DataMember> Property TokenExpirationDate As DateTime
        <DataMember> Property SecretCode As String
        <DataMember> Public Property DeviceTypeVCode As Short
        <DataMember> Public Property ip As String
        <DataMember> Public Property TokenState As TokenStateEnum
        <DataMember> Public Property TokenExpirationTime As Integer
        <DataMember> Public Property APIs As List(Of ApiData)
        <DataMember> Public Property SendLinkUrlSms As Boolean
        <DataMember> Public Property UserTypeVCode As UserTypeEnum
        <DataMember> Public Property UserPaymentTypeVCode As UserPaymentTypeEnum
        <DataMember> Public Property Name As String
        <DataMember> Public Property PartnerLogoUrl As String
        <DataMember> Public Property Credit As Decimal
        <DataMember> Public Property DocApproved As Boolean
        <DataMember> Public Property FirmRegistrationVCode As Long
        <DataMember> Public Property PresenterPaymentHasPaid As Boolean
        <DataMember> Public Property HasConfirmedRequest As Boolean
        <DataMember> Public Property Roles As List(Of RoleData)
        <DataMember> Public Property TrackingCode As String
        <DataMember> Public Property TrackingSendLink As Boolean
        <DataMember> Public Property VerificationCode As Integer
        <DataMember> Public Property VerificationExpireDate As DateTime
        <DataMember> Public Property ApiExpirationDate As DateTime
        <DataMember> Public Property VerificationCodeTryCount As Integer
        <DataMember> Public Property DeviceId As String
        <DataMember> Public Property IsApiFree As Boolean
        <DataMember> Public Property SelfOtp As Boolean
        <DataMember> Public Property AvailableUserPaymentTypeVCode As UserPaymentTypeEnum
    End Class

End Namespace