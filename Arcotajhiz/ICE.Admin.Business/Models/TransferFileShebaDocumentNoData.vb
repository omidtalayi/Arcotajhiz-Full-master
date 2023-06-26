Imports System.Runtime.Serialization
Imports ARCO.Entities.Enums
Imports ICE.Business.Models

Namespace Models
    <DataContract>
    Public Class TransferFileShebaDocumentNoData
        <DataMember> Public Property DocumentNo As Integer
        Public ReadOnly Property Description() As String
            Get
                If DocumentNo = 0 Then
                    Return "همه"
                Else
                    Return "صورت پرداخت شماره " & DocumentNo.ToString()
                End If
            End Get
        End Property
    End Class
End Namespace
