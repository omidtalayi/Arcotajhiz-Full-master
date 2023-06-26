Imports System.Runtime.Serialization

Namespace Enums

    <DataContract>
    Public Enum AccountingTypeEnum
        <EnumMember> CREDIT = 1
        <EnumMember> PAY_CREDIT = 2
        <EnumMember> PAY_PRESENTER = 3
    End Enum

End Namespace
