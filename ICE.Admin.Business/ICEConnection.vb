Imports System.Configuration
Imports System.Data.SqlClient

Friend Class ICEConnection
    Friend Shared ReadOnly Property Connection As SqlConnection
        Get
            SqlConnection.ClearAllPools()
            Dim _Connection As New SqlConnection(ConfigurationManager.ConnectionStrings("ICEConnection").ConnectionString)
            _Connection.Open()
            Return _Connection
        End Get
    End Property
    Friend Shared ReadOnly Property Command(StoredProcedureName As String) As SqlCommand
        Get
            SqlConnection.ClearAllPools()
            Dim _Command As New SqlCommand(StoredProcedureName, New SqlConnection(ConfigurationManager.ConnectionStrings("ICEConnection").ConnectionString)) _
                With {.CommandType = CommandType.StoredProcedure}
            _Command.Connection.Open()
            _Command.CommandTimeout = 30
            SqlCommandBuilder.DeriveParameters(_Command)
            Return _Command
        End Get
    End Property
End Class
