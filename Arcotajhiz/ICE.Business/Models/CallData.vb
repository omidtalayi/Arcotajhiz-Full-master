Imports System.ComponentModel
Imports System.Runtime.Serialization

Namespace Models

    <DataContract>
    Public Class CallData
        <DataMember> Public Property Phone As String
        <DataMember> Public Property Message As String
        <DataMember> Public Property VoiceID As String
    End Class

End Namespace
