Imports System.Runtime.Serialization

Namespace Models
    Public Class JaamRequestStatusData
        Public Property state As Boolean
        Public Property data As List(Of JaamRequestDataInner)
    End Class
    Public Class JaamRequestDataInner
        Public Property RequestNo As String
        Public Property ActionDate As String
        Public Property NationalId As String
        Public Property Fi1 As Integer
        Public Property Fi2 As Integer
        Public Property RequestStatus As String
    End Class

End Namespace
