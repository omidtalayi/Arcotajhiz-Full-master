Imports System.Runtime.Serialization
Namespace Enums
    <DataContract>
    Public Enum ControlTypeEnum
        <EnumMember> none = 0
        <EnumMember> checkbox = 1
        <EnumMember> datetime = 2
        <EnumMember> email = 3
        <EnumMember> text = 4
        <EnumMember> tel = 5
    End Enum
End Namespace

