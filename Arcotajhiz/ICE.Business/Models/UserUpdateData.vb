Imports System.Runtime.Serialization
Imports ARCO.Entities.Enums

Namespace Models

    <DataContract>
    Public Class UserUpdateData
        Inherits BaseData(Of UserUpdateStateEnum)
    End Class

End Namespace

