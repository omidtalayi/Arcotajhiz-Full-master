Imports System.Runtime.Serialization
Namespace Enums
    <DataContract>
    Public Enum ResponseStateEnum
        <EnumMember> FAILED = 0
        <EnumMember> SUCCESS = 1
        <EnumMember> NOTAUTHORIZED = 2
    End Enum
End Namespace
