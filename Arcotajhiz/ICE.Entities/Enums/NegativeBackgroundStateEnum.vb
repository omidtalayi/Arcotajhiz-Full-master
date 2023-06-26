Imports System.Runtime.Serialization
Namespace Enums
    <DataContract>
    Public Enum NegativeBackgroundStateEnum
        <EnumMember> NONE = 0
        <EnumMember> NO_NEGATIVE_STATUS = 1
        <EnumMember> TAX_DEBT = 2
        <EnumMember> OTHER_COURT_ACTION_BY_BANK = 3
        <EnumMember> DISHONORED_CHEQUE = 4
        <EnumMember> COURT_DECLARED_BANKRUPTCY = 5
        <EnumMember> WITH_NEGATIVE_STATUS = 6
        <EnumMember> ASSETS_FROZEN_OR_SEIZED = 7
        <EnumMember> CUSTOMER_UNTRACEABLE_OR_DECEASED = 8
    End Enum
End Namespace
