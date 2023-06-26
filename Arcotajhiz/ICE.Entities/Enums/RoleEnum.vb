Imports System.Runtime.Serialization
Namespace Enums
    <DataContract>
    Public Enum RoleEnum
        <EnumMember> NONE = 0
        <EnumMember> DASHBOARD = 1
        <EnumMember> FINANCIAL = 2
        <EnumMember> REPORT = 3
        <EnumMember> FIRM = 4
        <EnumMember> TRACKING = 5
        <EnumMember> CONTENT = 6
        <EnumMember> USER = 7
        <EnumMember> Menu = 8
        <EnumMember> SETTING = 9
        <EnumMember> PARTNER = 10
        <EnumMember> RS_DASHBOARD = 11
        <EnumMember> RS_REPORTOFCOMPLAINT = 12
    End Enum
End Namespace
