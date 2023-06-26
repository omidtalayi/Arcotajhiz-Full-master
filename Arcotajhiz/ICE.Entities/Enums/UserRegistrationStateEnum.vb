Imports System.Runtime.Serialization

Namespace Enums

    <DataContract>
    Public Enum UserRegistrationStateEnum
        <EnumMember> UNDEFINED = -1
        <EnumMember> NONE = 0
        <EnumMember> SUCCESSFUL = 1
        <EnumMember> INVALIDINPUT = 2
        <EnumMember> DUPLICATEEMAIL = 3
        <EnumMember> DUPLICATEUSERNAME = 4
        <EnumMember> DUPLICATECELLPHONE = 5
        <EnumMember> APLICATIONNOTFOUND = 6
        <EnumMember> WEAKPASSWORD = 7
        <EnumMember> DUPLICATECERTNUMBER = 8
        <EnumMember> WEAKUSERNAME = 9
        <EnumMember> DUPLICATETRACKINGCODE = 10
    End Enum

End Namespace