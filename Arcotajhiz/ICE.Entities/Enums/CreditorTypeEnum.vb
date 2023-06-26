Imports System.Runtime.Serialization
Namespace Enums
    <DataContract>
    Public Enum CreditorTypeEnum
        <EnumMember> NONE = 0
        <EnumMember> BANK = 1
        <EnumMember> FUND = 2
        <EnumMember> INSURANCE = 3
        <EnumMember> ASSIGNMENT = 4
        <EnumMember> OTHER = 5
    End Enum
End Namespace

