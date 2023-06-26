Imports System.Runtime.Serialization
Namespace Enums
    <DataContract>
    Public Enum CreditRiskReportEnum
        <EnumMember> NONE = 0
        <EnumMember> INDIVIDUAL_REPORT = 1
        <EnumMember> EMPTY_REPORT = 2
        <EnumMember> KYC_FAILD = 3
        <EnumMember> IS_LEGAL_PERSON_FAILD = 4
        <EnumMember> IS_EXPIRED = 5
        <EnumMember> INVALID_STATE = 6
        <EnumMember> INVALID_USER = 7
    End Enum
End Namespace

