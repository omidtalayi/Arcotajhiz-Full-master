Imports System.Data.SqlClient
Imports ARCO.Entities.Modules

Public Class ComplaintBusiness
    Public Function AddComplaints(ByRef vcode As Long, title As String, firstName As String, lastName As String, nationalCode As String, ByRef email As String, cellphone As String, description As String) As Boolean
        Dim command As SqlCommand = Nothing
        Try
            command = ARCOConnection.Command("AZ.ComplaintIns")
            command.Parameters("@VCode").Direction = ParameterDirection.Output
            command.Parameters("@Title").Value = title.Unify
            command.Parameters("@FirstName").Value = firstName
            command.Parameters("@LastName").Value = lastName
            command.Parameters("@NationalCode").Value = nationalCode
            command.Parameters("@Email").Value = email
            command.Parameters("@CellPhone").Value = cellphone
            command.Parameters("@Description").Value = description.Unify
            command.ExecuteNonQuery()
            vcode = command.Parameters("@VCode").Value
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
