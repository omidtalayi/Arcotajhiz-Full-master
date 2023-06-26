Imports ARCO.Entities.Enums

Public Class ApplicationBusiness
    Public Function GetSetting(key As String) As String
        Dim result As String
        Using command = ARCOConnection.Command("AZ.GetSetting")
            command.Parameters("@Key").Value = key
            result = command.ExecuteScalar()
        End Using
        Return result
    End Function
    Public Sub SetSetting(subSystemVCode As SubSystemEnum, key As String, newValue As String)
        Using command = ARCOConnection.Command("AZ.SetSetting")
            command.Parameters("@SubSystemVCode").Value = subSystemVCode
            command.Parameters("@Key").Value = key
            command.Parameters("@NewValue").Value = newValue
            command.ExecuteNonQuery()
        End Using
    End Sub

    Private Shared Function ConvertKiesToXml(kies As List(Of String)) As String
        Return kies.Aggregate(String.Empty, Function(current, key) current & String.Format("<K V=""{0}""/>", key))
    End Function

    Public Function GetSettings(Optional kies As List(Of String) = Nothing) As Hashtable
        Dim result As New Hashtable
        Using command = ARCOConnection.Command("AZ.GetSettings")
            command.Parameters("@Kies").Value = If(kies Is Nothing, DBNull.Value, ConvertKiesToXml(kies))
            Dim reader = command.ExecuteReader()
            While reader.Read()
                result.Add(reader("Key"), reader("Value"))
            End While
            reader.Close()
            reader.Dispose()
        End Using
        Return result
    End Function
    Public Function AddMonth(Jdate As String, Optional month As Integer = 1) As String
        Dim nextJdate As String
        Try
            Using command = ARCOConnection.Command("AZ.PROC_JAddMonth")
                command.Parameters("@JDate").Value = Jdate
                command.Parameters("@Month").Value = month
                nextJdate = command.ExecuteScalar()
            End Using
            Return CType(nextJdate, Integer).ToString("####/##/##")
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetDatabaseServerTime() As DateTime
        Dim databaseServerTime As DateTime
        Try
            Using command = ARCOConnection.Command("AZ.GetDatabaseServerTime")
                databaseServerTime = command.ExecuteScalar()
            End Using
            Return databaseServerTime
        Catch ex As Exception
            Throw ex
        End Try
    End Function

End Class
