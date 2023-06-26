Imports System.Runtime.Serialization
Namespace Enums
    <DataContract>
    Public Enum LogTypeEnum
        <EnumMember> [Debug] = -1
        <EnumMember> [Info] = 0
        <EnumMember> [Warning] = 1
        <EnumMember> [Error] = 2
    End Enum
End Namespace
