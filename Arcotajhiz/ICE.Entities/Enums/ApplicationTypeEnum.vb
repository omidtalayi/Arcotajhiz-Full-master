Imports System.Runtime.Serialization
Namespace Enums
    <DataContract>
    Public Enum ApplicationTypeEnum
        <EnumMember> NONE = 0
        <EnumMember> WEBSITE = 1
        <EnumMember> ANDROID = 2
        <EnumMember> IOS = 3
    End Enum
End Namespace
