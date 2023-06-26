Imports System.Runtime.Serialization

Namespace Enums

    <DataContract>
    Public Enum PortalPaymentTypeEnum
        <EnumMember> NONE = 0
        <EnumMember> PAYMENT = 1
        <EnumMember> CREDIT = 2
        <EnumMember> PRESENTER_PAYMENT = 3
        <EnumMember> API_FIRM_PAYMENT = 4
    End Enum

End Namespace
