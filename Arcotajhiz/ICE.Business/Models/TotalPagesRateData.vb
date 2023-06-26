Imports System.Runtime.Serialization
Imports ARCO.Entities.Enums

Namespace Models

    <DataContract>
    Public Class TotalPagesRateData
        <DataMember> Public Property Cnt As Integer
        <DataMember> Public Property Rate As Decimal
    End Class

End Namespace


