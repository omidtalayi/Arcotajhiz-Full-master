Imports System.Runtime.Serialization

Namespace Enums

    <DataContract>
    Public Enum DeviceTypeEnum
        <EnumMember> NONE = 0
        <EnumMember> WEBSITE = 1
        <EnumMember> IOS = 2
        <EnumMember> ANDROID = 3
        <EnumMember> WINDOWSPHONE = 4
        <EnumMember> CHROME = 5
        <EnumMember> FIREFOX = 6
        <EnumMember> OPERA = 7
        <EnumMember> SAFARI = 8
        <EnumMember> IE = 9
        <EnumMember> UNDEFIEND = 10
    End Enum

End Namespace
