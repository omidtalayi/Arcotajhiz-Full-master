Imports System.Runtime.Serialization
Namespace Enums
    <DataContract>
    Public Enum ApiErrorCodeEnum
        <EnumMember> NONE = 0
        <EnumMember> SERVER_ERROR = 1000
        <EnumMember> USERNAME_OR_PASSWORD_IS_INVALID = 1001
        <EnumMember> USER_IS_LOCKED = 1002
        <EnumMember> USER_IS_NOT_APPROVED = 1003
        <EnumMember> NATIONAL_CODE_IS_INVALID = 1004
        <EnumMember> CELLPHONE_IS_INVALID = 1005
        <EnumMember> REPORT_SOURCE_IS_INVALID = 1006
        <EnumMember> SENDING_SMS_HAS_FAILED = 1007
        <EnumMember> CREDIT_IS_NOT_ENOUGH = 1008
        <EnumMember> REPORT_ALREADY_EXIST_IN_LAST_24HOURS = 1009
        <EnumMember> REPORT_REQUEST_ALREADY_SUBMITTED = 1010
        <EnumMember> API_IS_DISABLED = 1011
        <EnumMember> COMPANY_NATIONAL_ID_IS_INVALID = 1012
        <EnumMember> REPORT_ID_IS_INVALID = 1013
        <EnumMember> SALES_REFID_IS_INVALID = 1014
        <EnumMember> SALES_AMOUNT_IS_INVALID = 1015
        <EnumMember> REPORT_IS_NOT_IN_RIGHT_STATE = 1016
        <EnumMember> REPORT_IS_EXPIRED = 1017
    End Enum
End Namespace