Imports System.Globalization
Imports System.Runtime.Serialization
Imports ARCO.Entities.Enums

Namespace Models
    <DataContract>
    Public Class RptPayFromCreditUsersData
        <DataMember> Public Property Username As String
        <DataMember> Public Property UserVCode As Long
    End Class
End Namespace


