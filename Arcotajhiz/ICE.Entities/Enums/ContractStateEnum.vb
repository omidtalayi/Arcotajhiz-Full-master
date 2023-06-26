Imports System.Runtime.Serialization
Namespace Enums
    <DataContract>
    Public Enum ContractStateEnum
        <EnumMember> NONE = 0
        <EnumMember> EXISITING = 1
        <EnumMember> TERMINATED_ACCORDING_THE_CONRACT = 2
        <EnumMember> TERMINATED_IN_ADVANCE_CORRECTLY_BECAUSE_OF_CLIENTS_WISH = 3
        <EnumMember> TERMINATED_IN_ADVANCE_INCORRECTLY_BECAUSE_OF_CLIENTS_NEGATIVE_BEHAVIOUR = 4
    End Enum
End Namespace

