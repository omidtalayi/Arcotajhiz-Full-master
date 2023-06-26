Imports System.Runtime.Serialization

Namespace Models

    <DataContract>
    Public MustInherit Class BaseData(Of TState)

        <DataMember> Public Property State As TState
        <DataMember> Public Property Exception As ExceptionData

        <DataMember>
        Public ReadOnly Property HasError() As Boolean
            Get
                Return Exception IsNot Nothing
            End Get
        End Property

    End Class

End Namespace
