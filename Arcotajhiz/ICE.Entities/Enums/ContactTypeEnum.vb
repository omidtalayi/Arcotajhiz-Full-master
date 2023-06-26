Imports System.Runtime.Serialization
Namespace Enums
    <DataContract>
    Public Enum ContactTypeEnum
        <EnumMember> NONE = 0
        <EnumMember> PHONE_NR = 1
        <EnumMember> FAX_NR = 2
        <EnumMember> CELLULAR_PHONE_NR = 3
        <EnumMember> EMAIL_ADDRESS = 4
        <EnumMember> WEB_PAGE = 5
    End Enum
End Namespace
