Imports System.Runtime.Serialization

Namespace Enums

    <DataContract>
    Public Enum ErrorLogTypeEnum
        <EnumMember> NONE = 0
        <EnumMember> DEFAULT_TYPE = 1
        <EnumMember> ERROR_TYPE = 2
        <EnumMember> WARNIGN_TYPE = 3
        <EnumMember> INFORMATION_TYPE = 4
    End Enum

End Namespace
