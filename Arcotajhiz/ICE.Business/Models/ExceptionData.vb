Imports System.Runtime.Serialization

Namespace Models

    <DataContract>
    Public Class ExceptionData

        Public Sub New(message As String)
            Me.Message = message
            InnerException = Nothing
        End Sub

        Public Sub New(ex As Exception)
            Message = ex.Message
            If ex.InnerException IsNot Nothing Then
                InnerException = New ExceptionData(ex.InnerException)
            End If
        End Sub

        <DataMember> Public Property Message As String
        <DataMember> Public Property InnerException As ExceptionData
        <DataMember> Public ReadOnly Property AllMessages As String
            Get
                Dim result As String = String.Empty
                If InnerException IsNot Nothing Then result &= " <- " & InnerException.AllMessages
                Return Message & result
            End Get
        End Property

    End Class

End Namespace
