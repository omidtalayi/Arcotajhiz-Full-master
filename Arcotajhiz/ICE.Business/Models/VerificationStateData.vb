Imports System.Runtime.Serialization
Imports ARCO.Entities.Enums

Namespace Models

    <DataContract>
    Public Class VerificationStateData

        <DataMember> Public Property VerificationState As VerificationStateEnum
        <DataMember> Public Property Description As String

    End Class

End Namespace
