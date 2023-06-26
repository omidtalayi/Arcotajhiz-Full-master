Imports System.Runtime.Serialization
Namespace Enums
    <DataContract>
    Public Enum EmploymentTypeEnum
        <EnumMember> NONE = 0
        <EnumMember> PRIVATE_ENTERPRENEUR = 1
        <EnumMember> MANUAL_WORKER = 2
        <EnumMember> MIDDLE_MANAGER = 3
        <EnumMember> HIGHER_MANAGER = 4
        <EnumMember> TOP_MANAGER = 5
        <EnumMember> POLICE_MILITARY_FIRE_BRIGADE_ETC = 6
        <EnumMember> TEACHER = 7
        <EnumMember> STATE_CLERK = 8
        <EnumMember> MEDICINE = 9
        <EnumMember> OTHER = 10
    End Enum
End Namespace
