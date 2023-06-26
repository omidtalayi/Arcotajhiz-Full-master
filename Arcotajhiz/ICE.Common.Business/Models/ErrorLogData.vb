Imports System.Runtime.Serialization
Imports ARCO.Entities.Enums

Namespace Models

    <DataContract>
    Public Class ErrorLogData

        <DataMember> Public Property Code As Integer
        <DataMember> Public Property IP As String
        <DataMember> Public Property Message As String
        <DataMember> Public Property StackTrace As String
        <DataMember> Public Property RequestUrl As String
        <DataMember> Public Property Source As String
        <DataMember> Public Property WebBrowser As String
        <DataMember> Public Property VisitLogCode As Guid
        <DataMember> Public Property ErrorLine As String
        <DataMember> Public Property MethodName As String
        <DataMember> Public Property ModuleName As String
        <DataMember> Public Property ErrorLogTypeVCode As ErrorLogTypeEnum

    End Class

End Namespace