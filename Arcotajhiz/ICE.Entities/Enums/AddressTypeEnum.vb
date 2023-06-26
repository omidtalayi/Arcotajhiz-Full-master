Imports System.Runtime.Serialization
Namespace Enums
    <DataContract>
    Public Enum AddressTypeEnum
        <EnumMember> NONE = 0
        <EnumMember> ADDRESS_OF_PERMANENT_RESIDENCE_OR_REGISTRATION = 1
        <EnumMember> FACTUAL_ADDRESS = 2
        <EnumMember> POSTAL_ADDRESS = 3
        <EnumMember> ADDRESS_OF_THE_EMPLOYER = 4
    End Enum
End Namespace

