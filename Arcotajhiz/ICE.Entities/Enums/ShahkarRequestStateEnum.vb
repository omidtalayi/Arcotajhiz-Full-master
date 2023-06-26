Imports System.Runtime.Serialization
Namespace Enums
    <DataContract>
    Public Enum ShahkarRequestStateEnum
        <EnumMember> NONE = 0
        <EnumMember> PENDING = 1
        <EnumMember> RESPONDED = 2
        <EnumMember> NOT_RESPONDED = 3
    End Enum
End Namespace

