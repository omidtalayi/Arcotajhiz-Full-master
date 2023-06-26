Imports System.Runtime.Serialization
Imports ARCO.Entities.Enums

Namespace Models

    <DataContract>
    Public Class ApiData

        <DataMember> Public Property VCode As Integer
        <DataMember> Public Property Code As Integer
        <DataMember> Public Property Name As String
        <DataMember> Public Property EnumName As String
    End Class

End Namespace

