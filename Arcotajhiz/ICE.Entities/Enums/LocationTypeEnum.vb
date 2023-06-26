Imports System.Runtime.Serialization

Namespace Enums

    <DataContract>
    Public Enum LocationTypeEnum
        <EnumMember> NONE = 0
        <EnumMember> COUNTRY = 1
        <EnumMember> PROVINCE = 2
        <EnumMember> COUNTY = 3
        <EnumMember> CITY = 4
        <EnumMember> DISTRICT = 5
        <EnumMember> AREA = 6
        <EnumMember> BLOCK = 7
    End Enum

End Namespace