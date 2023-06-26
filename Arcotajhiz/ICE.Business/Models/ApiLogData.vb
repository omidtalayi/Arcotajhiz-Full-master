Imports System.Runtime.Serialization

Namespace Models
    <DataContract>
    Public Class ApiLogData

        <DataMember> Public Property VCode As Long
        <DataMember> Public Property Request As String
        <DataMember> Public Property Response As String
        <DataMember> Public Property State As String
        <DataMember> Public Property UserVCode As Long
        <DataMember> Public Property IP As String
        <DataMember> Public Property MethodName As String
        <DataMember> Public Property Token As String
        <DataMember> Public Property EntryDate As DateTime
    End Class
End Namespace