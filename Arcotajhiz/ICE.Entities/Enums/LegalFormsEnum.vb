Imports System.Runtime.Serialization
Namespace Enums
    <DataContract>
    Public Enum LegalFormsEnum
        <EnumMember> NONE = 0
        <EnumMember> STATE_ENTERPRISE = 1
        <EnumMember> THE_PUBLIC_JOINT_STOCK_COMPANY = 2
        <EnumMember> THE_PRIVATE_JOINT_STOCK_COMPANY = 3
        <EnumMember> THE_FULL_LIABILITY_COMPANY = 4
        <EnumMember> THE_LIMITED_LIABILITY_COMPANY = 5
        <EnumMember> THE_PARTNERSHIP_IN_COMMENDUM = 6
        <EnumMember> THE_GENERAL_PARTNERSHIP_COMPANY = 7
        <EnumMember> THE_LIMITED_PARTNERSHIP_COMPANY = 8
        <EnumMember> THE_ADDITIONAL_LIABILITY_PARTNERSHIP = 9
        <EnumMember> THE_JOINT_STOCK_PARTNERSHIP_COMPANY = 10
        <EnumMember> THE_PROPORTIONAL_LIABILITY_COMPANY = 11
        <EnumMember> PRODUCTION_COOPERATIVE = 12
        <EnumMember> CONSUMER_COOPERATIVE = 13
        <EnumMember> INSTITUTION = 14
        <EnumMember> STATE_INSTITUTION = 15
        <EnumMember> PUBLIC_ASSOCIATION = 16
        <EnumMember> NON_PROFIT_JOINT_STOCK_COMPANY = 17
        <EnumMember> SOCIAL_FUND = 18
        <EnumMember> RELIGIOUS_ASSOCIATION = 19
        <EnumMember> OTHER_FORMS_STIPULATED_BY_LEGISLATIVE_ACT = 20
    End Enum
End Namespace
