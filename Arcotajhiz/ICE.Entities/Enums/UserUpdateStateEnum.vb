Imports System.Runtime.Serialization

Namespace Enums

    <DataContract>
    Public Enum UserUpdateStateEnum
        <EnumMember> UNDEFINED = -1
        <EnumMember> NONE = 0
        <EnumMember> SUCCESSFUL = 1
        <EnumMember> LOGINFAILED = 2
        <EnumMember> DUPLICATEUSERNAME = 3
        <EnumMember> DUPLICATEEMAIL = 4
        <EnumMember> DUPLICATECELLPHONE = 5
        <EnumMember> USERNOTFOUND = 6
        <EnumMember> CELLPHONECHANGENOTPERMITTED = 7
        <EnumMember> EMAILCHANGENOTPERMITTED = 8
        <EnumMember> WEAKPASSWORD = 9
        <EnumMember> DUPLICATECERTNUMBER = 10
        <EnumMember> INVALIDCELLPHONE = 11
        <EnumMember> INVALIDEMAIL = 12
        <EnumMember> USERNAMEISNOTSUFFICIENT = 12
    End Enum

End Namespace