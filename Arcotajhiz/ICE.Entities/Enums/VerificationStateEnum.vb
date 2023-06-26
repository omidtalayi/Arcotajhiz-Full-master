Imports System.Runtime.Serialization
Namespace Enums
    <DataContract>
    Public Enum VerificationStateEnum
        <EnumMember> NONE = 0
        <EnumMember> SUCCESSFUL = 1
        <EnumMember> VERIFICATION_CODE_IS_NOT_VALID = 2
        <EnumMember> VERIFICATION_CODE_IS_EXPIRED = 3
        <EnumMember> CELLPHONE_IS_NOT_FOUND = 4
        <EnumMember> LINK_IS_EXPIRED = 5
        <EnumMember> CELLPHONE_OR_NATIONAL_CODE_IS_NOT_FOUND = 6
        <EnumMember> LINK_IS_REJECTED_BY_INDIVIDUAL = 7
        <EnumMember> TOO_MUCH_INCORRECT_NATIONAL_CODE_INPUT = 8
        <EnumMember> SUCCESSFUL_VERIFY_CELLPHONE = 9
    End Enum
End Namespace