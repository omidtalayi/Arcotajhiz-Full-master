Imports System.Runtime.Serialization
Namespace Enums
    <DataContract>
    Public Enum NegativeContractStatusEnum
        <EnumMember> NONE = 0
        <EnumMember> NO_NEGATIVE_STATUS = 1
        <EnumMember> UNAUTORIZED_DEBIT_BALANCE_ON_CURRENT_ACCOUNT = 2
        <EnumMember> BLOCKED = 3
        <EnumMember> CANCELLED_DUE_TO_DELAYED_PAYMENTS = 4
        <EnumMember> PAST_DUE_DEBT = 5
        <EnumMember> FRAUD_TOWARDS_BANK = 6
        <EnumMember> CREDIT_REASSIGNED_TO_A_NEW_BORROWER = 7
        <EnumMember> ASSIGNMENT_OF_CREDIT_TO_THIRD_PARTY = 8
        <EnumMember> LOAN_WRITTEN_OFF_TO_OFF_BALANCE_SHEET_ACCOUNT = 9
        <EnumMember> DEFERRED_INSTALLMENT = 10
        <EnumMember> DOUBTFUL_DEBT = 11
    End Enum
End Namespace
