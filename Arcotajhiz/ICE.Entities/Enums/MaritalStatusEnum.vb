Imports System.Runtime.Serialization
Namespace Enums
    <DataContract>
    Public Enum MaritalStatusEnum
        <EnumMember> NONE = 0
        <EnumMember> [SINGLE] = 1
        <EnumMember> MARRIED = 2
        <EnumMember> DIVORCED = 3
        <EnumMember> WIDOWED = 4
    End Enum
End Namespace
