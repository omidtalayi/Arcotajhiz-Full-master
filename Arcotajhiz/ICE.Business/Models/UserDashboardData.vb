Imports System.Runtime.Serialization
Imports ARCO.Entities.Enums

Namespace Models

    <DataContract>
    Public Class UserDashboardData
        <DataMember> Public Property Credit As Decimal
        <DataMember> Public Property ConfirmedCount As Integer
        <DataMember> Public Property WaitingPersonConfirmationCount As Integer
        <DataMember> Public Property RejectedCount As Integer
        <DataMember> Public Property UserLastSevenDaysSeens As List(Of UserLastSevenDaysSeenData)
        <DataMember> Public Property UserLastSevenHours As List(Of UserLastSevenHoursData)
    End Class

End Namespace
