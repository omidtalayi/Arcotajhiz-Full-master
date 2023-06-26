Imports System.Runtime.Serialization
Imports ARCO.Entities.Enums

Namespace Models

    <DataContract>
    Public Class BatchResultXmlData
        <DataMember> Public Property ResponseXml As String
        <DataMember> Public Property ResponseXmlScore As String
        <DataMember> Public Property ResponseXmlEmpty As String
        <DataMember> Public Property ResponseJson As String
    End Class

End Namespace
