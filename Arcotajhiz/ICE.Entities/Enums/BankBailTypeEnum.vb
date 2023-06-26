Imports System.Runtime.Serialization
Namespace Enums
    <DataContract>
    Public Enum BankBailTypeEnum
        <EnumMember> NONE = 0
        <EnumMember> DEPOSIT = 1
        <EnumMember> COLLATERISED_BY_SECURITIES_AND_PRECIOUS_METALS = 2
        <EnumMember> PERSONAL_REAL_PROPERTY = 3
        <EnumMember> COMMERCIAL_PROPERTY = 4
        <EnumMember> VEHICLES = 5
        <EnumMember> GUARANTEE_BAIL_OF_A_PHYSICAL_PERSON = 6
        <EnumMember> GUARANTEE_BAIL_OF_A_LEGAL_ENTITY = 7
        <EnumMember> EQUIPMENT = 8
        <EnumMember> GOODS = 9
        <EnumMember> PLEDGE_OF_RIGHT_OF_DEMAND = 10
        <EnumMember> PERSONAL_PROPERTY = 11
        <EnumMember> COLLATERAL_TO_BE_PROVIDED_IN_FUTURE = 12
        <EnumMember> BILL_OF_EXCHANGE = 13
    End Enum
End Namespace

