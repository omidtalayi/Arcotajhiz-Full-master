Imports System.Runtime.Serialization

Namespace Models
    <DataContract>
    Public Class ApplicationSettingData
        <DataMember> Public Property VCode As Integer
        <DataMember> Public Property Key As String
        <DataMember> Public Property Value As String
        <DataMember> Public Property Description As String
        <DataMember> Public Property EntryDate As Date

    End Class
End Namespace

