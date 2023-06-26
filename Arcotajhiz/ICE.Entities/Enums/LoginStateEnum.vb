Imports System.Runtime.Serialization

Namespace Enums
    <DataContract> _
    Public Enum LoginStateEnum
        <EnumMember> UNDEFINED = -1
        <EnumMember> NONE = 0
        <EnumMember> SUCCESSFUL = 1
        <EnumMember> INVALIDUSERNAME = 2
        <EnumMember> INVALIDPASSWORD = 3
        <EnumMember> USERLOCKED = 4
        <EnumMember> USERNOTAPPROVED = 5
    End Enum
End Namespace
