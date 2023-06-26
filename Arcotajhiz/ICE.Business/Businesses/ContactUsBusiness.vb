Imports System.Data.SqlClient
Imports ARCO.Entities.Modules

Public Class ContactUsBusiness
    Public Function AddContactUs(ByRef vcode As Long, title As String, fullname As String, ByRef email As String, cellphone As String, description As String) As Boolean
        Dim command As SqlCommand = Nothing
        Try
            command = ARCOConnection.Command("AZ.ContactUsIns")
            command.Parameters("@VCode").Direction = ParameterDirection.Output
            command.Parameters("@Title").Value = title.Unify
            command.Parameters("@FullName").Value = fullname
            command.Parameters("@Email").Value = email
            command.Parameters("@CellPhone").Value = If(String.IsNullOrEmpty(cellphone), DBNull.Value, cellphone)
            command.Parameters("@Description").Value = description.Unify
            command.ExecuteNonQuery()
            'vcode = command.Parameters("@VCode").Value
            Return True
        Catch ex As Exception
            Throw ex
        Finally
            If command IsNot Nothing Then
                command.Connection.Close()
                command.Connection.Dispose()
                command.Dispose()
            End If
        End Try
    End Function
End Class
