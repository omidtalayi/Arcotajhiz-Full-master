Imports System.Runtime.Serialization
Namespace Enums
    <DataContract>
    Public Enum VerifyEmailStateEnum
        <EnumMember> NONE = 0
        <EnumMember> SUCCESSFUL = 1
        <EnumMember> LINK_IS_INVALID = 2
    End Enum
End Namespace


