Imports System.Runtime.Serialization
Namespace Enums
    <DataContract>
    Public Enum SMSTypeEnum
        <EnumMember> NONE = 0
        <EnumMember> OTP = 1
        <EnumMember> CONFIRMATION = 2
        <EnumMember> MESSAGE = 3
        <EnumMember> REPORT_LINK = 4
        <EnumMember> REPORT_REPAIR_LINK = 5
        <EnumMember> ADVERTISING = 6
        <EnumMember> REPAIR_MESSAGE = 7
    End Enum
End Namespace