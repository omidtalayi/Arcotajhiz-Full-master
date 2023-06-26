Imports System.Runtime.Serialization

Namespace Enums

    <DataContract>
    Public Enum KycStateEnum
        <EnumMember> NONE = 0
        <EnumMember> SUBMITTED = 1
        <EnumMember> REQUESTED = 2
        <EnumMember> RECEIVED = 3
        <EnumMember> SENT = 4
        <EnumMember> FAILED = 5
    End Enum

End Namespace

