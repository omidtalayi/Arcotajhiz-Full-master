Imports System.Runtime.Serialization
Namespace Enums
    <DataContract>
    Public Enum UserPaymentTypeEnum
        <EnumMember> NONE = 0
        <EnumMember> PAY_BY_CUSTOMER = 1
        <EnumMember> PAY_FROM_CREDIT = 2
        <EnumMember> PAY_BY_RECEIVER = 3
    End Enum
End Namespace

