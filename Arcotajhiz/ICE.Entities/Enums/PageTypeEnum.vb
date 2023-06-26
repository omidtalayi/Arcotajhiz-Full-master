Imports System.Runtime.Serialization

Namespace Enums

    <DataContract>
    Public Enum PageTypeEnum
        <EnumMember> NONE = 0
        <EnumMember> BLOG = 1
        <EnumMember> PAGE = 2
        <EnumMember> PARTNER = 3
    End Enum

End Namespace