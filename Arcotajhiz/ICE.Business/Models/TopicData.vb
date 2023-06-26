Imports System.Runtime.Serialization
Imports ARCO.Entities.Enums

Namespace Models

    <DataContract>
    Public Class TopicData
        <DataMember> Public Property VCode As Integer
        <DataMember> Public Property Name As String
        <DataMember> Public Property IsEnable As Boolean
        <DataMember> Public Property EntryDate As DateTime
    End Class

End Namespace
