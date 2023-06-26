Imports System.Data.SqlClient
Imports ARCO.Entities.Enums

Public Class NotificationBusiness
    Public Shared Function NotificationLogIns(ByRef vcode As Long, notificationTypeVCode As NotificationTypeEnum, message As String, userVCode As Integer, Optional cellphone As String = Nothing, Optional email As String = Nothing, Optional smsProviderID As Long? = Nothing) As Boolean
        Dim command As SqlCommand = Nothing
        Try
            command = ICELogConnection.Command("AZ.NotificationLogIns")
            command.Parameters("@VCode").Direction = ParameterDirection.Output
            command.Parameters("@UserVCode").Value = If(userVCode <> 0, userVCode, DBNull.Value)
            command.Parameters("@NotificationTypeVCode").Value = notificationTypeVCode
            command.Parameters("@Message").Value = If(Not String.IsNullOrEmpty(message), message, DBNull.Value)
            command.Parameters("@Cellphone").Value = If(Not String.IsNullOrEmpty(cellphone), cellphone, DBNull.Value)
            command.Parameters("@Email").Value = If(Not String.IsNullOrEmpty(email), email, DBNull.Value)
            command.Parameters("@SMSProviderID").Value = If(smsProviderID.HasValue, smsProviderID, DBNull.Value)
            command.ExecuteNonQuery()
            vcode = command.Parameters("@VCode").Value
            Return True
        Catch ex As Exception
            Throw
        Finally
            If command IsNot Nothing Then
                command.Connection.Close()
                command.Connection.Dispose()
                command.Dispose()
            End If
        End Try
    End Function
End Class
