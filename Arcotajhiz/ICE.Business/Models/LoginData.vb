Imports System.Runtime.Serialization
Imports ICE.Entities
Imports ARCO.Entities.Enums
Imports System.Collections.Generic

Namespace Models

    <DataContract>
    Public Class LoginData
        <DataMember> Public Property LoginState As LoginStateEnum
        <DataMember> Public Property User As UserData
        <DataMember> Public Property Exception As ExceptionData
    End Class

End Namespace