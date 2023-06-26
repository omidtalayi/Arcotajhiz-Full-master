Imports System.Runtime.Serialization
Namespace Enums
    <DataContract>
    Public Enum RequestRegistrationStateEnum
        <EnumMember> NONE = 0
        <EnumMember> NOT_PAID = 1
        <EnumMember> CONFIRMED = 2
        <EnumMember> REJECTED = 3
        <EnumMember> PENDING = 4
        <EnumMember> PAID = 5
        <EnumMember> PENDING_LIVE = 6
        <EnumMember> DONE = 7
    End Enum
End Namespace

