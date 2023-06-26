Imports System.Runtime.Serialization
Namespace Enums
    <DataContract>
    Public Enum TokenStateEnum
        <EnumMember> NONE = 0
        <EnumMember> SUCCESSFUL = 1
        <EnumMember> TOKEN_IS_INVALID = 2
        <EnumMember> TOKEN_IS_EXPIRED = 3
    End Enum
End Namespace

