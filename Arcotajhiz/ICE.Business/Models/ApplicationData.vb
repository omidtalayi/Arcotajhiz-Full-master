Imports System.Runtime.Serialization

Namespace Models

    <DataContract>
    Public Class ApplicationData

        <DataMember> Public Property VCode As Integer
        <DataMember> Public Property Name As String
        Public Property DefaultUserActive As Boolean
        Public Property DefaultUserApprove As Boolean
        Public Property FailedLoginAttemptCount As Byte
        Public Property UnlockingUserTimeInMinute As Byte

    End Class

End Namespace