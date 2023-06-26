Imports System.Runtime.Serialization
Namespace Enums
    <DataContract>
    Public Enum PasswordPolicyEnum
        <EnumMember> BLANK = 0
        <EnumMember> OK = 1
        <EnumMember> TOOSHORT = -1
        <EnumMember> NODIGITS = -2
        <EnumMember> NOLOWERCASECHARACTER = -3
        <EnumMember> NOUPPERCASECHARACTER = -4
        <EnumMember> NOSPECIALCHARACTER = -5
        <EnumMember> NOLETTERCHARACTER = -6
    End Enum
End Namespace

