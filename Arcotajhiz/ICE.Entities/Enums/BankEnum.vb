Imports System.Runtime.Serialization
Namespace Enums
    <DataContract>
    Public Enum JaamRequestStatusEnum
        <EnumMember> NONE = 0
        <EnumMember> PENDING = 1
        <EnumMember> ACCEPTED = 2
        <EnumMember> REJECTED = 3
    End Enum
End Namespace
