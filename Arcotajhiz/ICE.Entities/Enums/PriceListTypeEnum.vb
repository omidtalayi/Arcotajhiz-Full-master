Imports System.Runtime.Serialization
Namespace Enums
    <DataContract>
    Public Enum PriceListTypeEnum
        <EnumMember> NONE = 0
        <EnumMember> FOR_ME = 1
        <EnumMember> FOR_SENDING_TO_OTHERS = 2
    End Enum
End Namespace
