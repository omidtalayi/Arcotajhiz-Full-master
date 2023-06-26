Imports System.Runtime.Serialization

Namespace Enums

    <DataContract>
    Public Enum SubSystemEnum
        <EnumMember> [NONE] = 0
        <EnumMember> [WEB_API] = 1
        <EnumMember> [WEBSITE] = 2
        <EnumMember> [ADMIN] = 3
        <EnumMember> [REPORTSOURCE] = 4
    End Enum
End Namespace