Imports System.Runtime.Serialization
Namespace Enums
    <DataContract>
    Public Enum UserTypeEnum
        <EnumMember> NONE = 0
        <EnumMember> PARTNER = 1
        <EnumMember> NORMAL_USER = 2
        <EnumMember> TRACKING = 3
    End Enum
End Namespace
