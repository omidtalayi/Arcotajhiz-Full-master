Imports System.Runtime.Serialization
Imports ARCO.Entities.Enums

Namespace Models

    <DataContract>
    Public Class KycData
        <DataMember> Public Property IsValid As Boolean
        <DataMember> Public Property ErrorCodeEnum As ApiErrorCodeEnum
    End Class
End Namespace
