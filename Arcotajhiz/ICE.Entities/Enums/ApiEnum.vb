Imports System.Runtime.Serialization
Namespace Enums
    <DataContract>
    Public Enum ApiEnum
        <EnumMember> NONE = 0
        <EnumMember> AUTHENTICATE = 1
        <EnumMember> PRODUCTS = 2
        <EnumMember> PAGES = 3
        '<EnumMember> FIRM_REPORT_EXISTENCE = 3
        '<EnumMember> GET_INDIVIDUAL_INFORMATION = 4
        '<EnumMember> SEND_REPORT_URL_TO_CUSTOMER = 5
        '<EnumMember> KYC_CHECK = 6
        '<EnumMember> SEND_SMS = 7
        '<EnumMember> GET_ICS_REPORT_SOURCES = 8
        '<EnumMember> ADD_IDENTIFICATION_FROM_FIRM = 9
        '<EnumMember> GET_REPORT_DATA = 10
        '<EnumMember> SET_INDIVIDUAL_INFORMATION_REQUEST = 11
        '<EnumMember> VERIFY_CELLPHONE = 12
        '<EnumMember> GET_REPORT_STATUS = 13
        '<EnumMember> GET_IDENTIFICATION_LIST = 14
        '<EnumMember> GROUP_OF_PERSONS = 15
        '<EnumMember> JAAM = 16
        '<EnumMember> COMPANY_PERSON = 17
    End Enum
End Namespace
