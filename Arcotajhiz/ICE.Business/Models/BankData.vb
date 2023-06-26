Imports System.Runtime.Serialization
Imports ARCO.Entities.Enums

Namespace Models

    <DataContract>
    Public Class BankData
        <DataMember> Public Property VCode As Integer
        <DataMember> Public Property Code As Integer
        <DataMember> Public Property Name As String
        <DataMember> Public Property EnumName As String
        <DataMember> Public Property Image As String
        <DataMember> Public ReadOnly Property ImagePath() As String
            Get
                Return "/Content/Img/BankLogo/" + Image
            End Get
        End Property
    End Class
End Namespace

