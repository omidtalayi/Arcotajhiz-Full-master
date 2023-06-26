Imports System.Data.SqlClient
Imports ICE.Admin.Business.Models

Public Class SettingBusiness
    Private Shared Function GetApplicationSettingData(reader As IDataRecord) As ApplicationSettingData
        Try
            Dim oPagesData = New ApplicationSettingData With
                {
                    .VCode = If(IsDBNull(reader("VCode")), 0, reader("VCode")),
                    .Key = If(IsDBNull(reader("key")), Nothing, reader("key")),
                    .Value = If(IsDBNull(reader("Value")), Nothing, reader("Value")),
                    .Description = If(IsDBNull(reader("Description")), Nothing, reader("Description")),
                    .EntryDate = If(IsDBNull(reader("EntryDate")), Nothing, reader("EntryDate"))
                }
            Return oPagesData
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetApplicationSetting() As List(Of ApplicationSettingData)
        Dim oApplicationSettingData As New List(Of ApplicationSettingData)
        Dim command As SqlCommand = Nothing
        Dim reader As SqlDataReader = Nothing
        Try
            command = ICEConnection.Command("AZ.Admin_GetApplicationSetting")
            reader = command.ExecuteReader()
            While reader.Read()
                oApplicationSettingData.Add(GetApplicationSettingData(reader))
            End While
            Return oApplicationSettingData
        Catch ex As Exception
            Throw ex
        Finally
            If reader IsNot Nothing Then
                reader.Close()
                reader.Dispose()
            End If
            If command IsNot Nothing Then
                command.Connection.Close()
                command.Connection.Dispose()
                command.Dispose()
            End If
        End Try
    End Function
    Public Function ApplicationSettingUpd(VCode As Integer, Value As String) As Boolean
        Dim command As SqlCommand = Nothing
        Try
            command = ICEConnection.Command("AZ.Admin_ApplicationSettingUpd")
            command.Parameters("@VCode").Value = VCode
            command.Parameters("@Value").Value = Value
            command.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            Throw ex
            Return False
        Finally
            If command IsNot Nothing Then
                command.Connection.Close()
                command.Connection.Dispose()
                command.Dispose()
            End If
        End Try
    End Function
End Class
