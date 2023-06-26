Imports System.Runtime.Serialization
Imports System.Web

Namespace Models

    <DataContract>
    Public Class ClientErrorData
        <DataMember> Public Property visitLogCode As Guid
        <DataMember> Public Property column As String
        <DataMember> Public Property lineNumber As String
        <DataMember> Public Property message As String
        <DataMember> Public Property stack As String
        <DataMember> Public Property url As String
        <DataMember> Public Property errorDate As DateTime
        <DataMember> Public Property isMobileDevice As Boolean
        <DataMember> Public Property isMobileApp As Boolean

    End Class
End Namespace