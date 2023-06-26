Imports System.Runtime.Serialization
Imports ARCO.Entities.Enums

Namespace Models

    <DataContract>
    <Serializable>
    Public Class RoleData

        <DataMember> Public Property VCode As Integer
        <DataMember> Public Property UserVCode As Long
        <DataMember> Public Property SubSystemVCode As SubSystemEnum
        <DataMember> Public Property RoleVCode As RoleEnum
        <DataMember> Public Property EntryDate As DateTime
    End Class

End Namespace
