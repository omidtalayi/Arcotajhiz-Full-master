Imports System.Runtime.Serialization
Namespace Enums
    <DataContract>
    Public Enum ApprovalStateEnum
        <EnumMember> NONE = 0
        <EnumMember> PENDING = 1
        <EnumMember> APPROVED = 2
        <EnumMember> DISAPPROVED = 3
    End Enum
End Namespace
