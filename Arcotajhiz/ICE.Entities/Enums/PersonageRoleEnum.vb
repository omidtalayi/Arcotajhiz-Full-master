Imports System.Runtime.Serialization
Namespace Enums
    <DataContract>
    Public Enum PersonageRoleEnum
        <EnumMember> NONE = 0
        <EnumMember> DEBTOR_MAIN_APPLICANT = 1
        <EnumMember> CO_DEBTOR_CO_APPLICANT = 2
        <EnumMember> GUARANTOR = 3
    End Enum
End Namespace

