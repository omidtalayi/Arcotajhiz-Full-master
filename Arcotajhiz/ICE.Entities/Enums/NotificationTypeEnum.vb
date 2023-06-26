Imports System.Runtime.Serialization

Namespace Enums

    <DataContract>
    Public Enum NotificationTypeEnum
        <EnumMember> NONE = 0
        <EnumMember> SMS = 1
        <EnumMember> PHONECALL = 2
    End Enum

End Namespace