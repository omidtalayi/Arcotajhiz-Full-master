Imports System.Runtime.Serialization
Namespace Enums
    <DataContract>
    Public Enum PaymentTypeEnum
        <EnumMember> NONE = 0
        <EnumMember> CURRENT_ACCOUNT = 1
        <EnumMember> BILL_OF_EXCHANGE = 2
        <EnumMember> BANKING_RECEIPT = 3
        <EnumMember> DIRECT_REMITTANCE = 4
        <EnumMember> AUTHORIZATION_TO_DIRECT_CURRENT_ACCOUNT_DEBIT = 5
    End Enum
End Namespace
