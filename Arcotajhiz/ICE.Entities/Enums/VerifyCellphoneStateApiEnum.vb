Imports System.Runtime.Serialization
Namespace Enums
    <DataContract>
    Public Enum VerifyCellphoneStateApiEnum
        <EnumMember> NONE = 0
        <EnumMember> SUCCESSFUL = 1
        <EnumMember> VERIFICATION_CODE_IS_NOT_VALID = 2
        <EnumMember> VERIFICATION_CODE_IS_EXPIRED = 3
        <EnumMember> SHAHKAR_FAILED = 4
        <EnumMember> IS_LEGAL_PERSON_FAILED = 5
        <EnumMember> REQUST_IS_LOCKED = 6
        <EnumMember> REQUST_NOT_VALID_STATE = 7
    End Enum
End Namespace