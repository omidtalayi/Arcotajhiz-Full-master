Imports System.Runtime.Serialization

Namespace Models

    <DataContract>
    Public Class UserLastSevenHoursData
        <DataMember> Public Property Hour As Integer
        <DataMember> Public Property ConfirmedCnt As Integer
        <DataMember> Public Property RejectedCnt As Integer
        <DataMember> Public Property WaitingPersonConfirmationCnt As Integer
        <DataMember> Public ReadOnly Property FormattedHour As String
            Get
                Return Hour.ToString("00") + ":00"
            End Get
        End Property
    End Class

End Namespace


