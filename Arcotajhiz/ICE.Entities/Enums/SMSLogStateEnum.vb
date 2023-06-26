Imports System.Runtime.Serialization
Namespace Enums
    <DataContract>
    Public Enum SMSLogStateEnum
        <EnumMember> ON_QUEUE = 0
        <EnumMember> REACHED_THE_PHONE = 1
        <EnumMember> NOT_REACHED_THE_PHONE = 2
        <EnumMember> REACHED_TELECOMMUNICATIONS = 8
        <EnumMember> NOT_REACHED_TELECOMMUNICATIONS = 16
        <EnumMember> THE_RECIPIENT_NUMBER_IS_IN_BLACKLIST = 27
        <EnumMember> INVALID_ID = -1
    End Enum
End Namespace



