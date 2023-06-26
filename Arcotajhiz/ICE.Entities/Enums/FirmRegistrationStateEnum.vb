Imports System.Runtime.Serialization
Namespace Enums
    <DataContract>
    Public Enum FirmRegistrationStateEnum
        <EnumMember> NONE = 0
        <EnumMember> SUCCESSFUL = 1
        <EnumMember> EMAIL_IS_DUPLICATE = 2
        <EnumMember> CELLPHONE_CONTACT_POINT_IS_DUPLICATE = 3
    End Enum
End Namespace

