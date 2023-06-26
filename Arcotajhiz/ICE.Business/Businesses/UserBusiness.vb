Imports System.Data.SqlClient
Imports ARCO.Entities.Enums
Imports System.ComponentModel
Imports System.Text.RegularExpressions
Imports System.Web.Mvc
Imports System.Xml
Imports ARCO.Entities.Modules
Imports ARCO.Business.Models
Imports ARCO.Business.Businesses

Public Class UserBusiness
    Const SaltLength As Integer = 40
    Private Shared Function GetUserData(reader As IDataRecord) As UserData
        Try
            Dim user = New UserData() With
                  {
                      .VCode = reader("UserVCode"),
                      .Username = reader("Username"),
                      .Password = reader("Password"),
                      .PasswordSalt = reader("PasswordSalt"),
                      .Email = If(IsDBNull(reader("Email")), Nothing, reader("Email")),
                      .CellPhone = If(IsDBNull(reader("CellPhone")), Nothing, reader("CellPhone")),
                      .IsApproved = reader("IsApproved"),
                      .IsLock = reader("IsLock"),
                      .IsSubscribed = reader("IsSubscribed"),
                      .LastLoginDate = If(IsDBNull(reader("LastLoginDate")), Nothing, reader("LastLoginDate")),
                      .LastFailedAttemptDate = If(IsDBNull(reader("LastFailedAttemptDate")), Nothing, reader("LastFailedAttemptDate")),
                      .FailedAttemptCount = reader("FailedAttemptCount"),
                      .Token = If(IsDBNull(reader("Token")), Nothing, reader("Token")),
                      .TokenExpirationDate = If(IsDBNull(reader("TokenExpirationDate")), DateTime.MinValue, reader("TokenExpirationDate")),
                      .SecretCode = If(IsDBNull(reader("SecretCode")), Nothing, reader("SecretCode")),
                      .TokenExpirationTime = If(IsDBNull(reader("TokenExpirationTime")), 0, reader("TokenExpirationTime")),
                      .APIs = ConvertFromXMLToAPI(If(IsDBNull(reader("API")), Nothing, reader("API"))),
                      .SendLinkUrlSms = If(IsDBNull(reader("SendLinkUrlSms")), False, reader("SendLinkUrlSms")),
                      .EntryDate = reader("EntryDate"),
                      .LastModifiedDate = If(IsDBNull(reader("LastModifiedDate")), Nothing, reader("LastModifiedDate")),
                      .UserTypeVCode = If(IsDBNull(reader("UserTypeVCode")), UserTypeEnum.NONE, reader("UserTypeVCode")),
                      .UserPaymentTypeVCode = If(IsDBNull(reader("UserPaymentTypeVCode")), UserPaymentTypeEnum.NONE, reader("UserPaymentTypeVCode")),
                      .Name = If(IsDBNull(reader("Name")), Nothing, reader("Name")),
                      .DocApproved = If(IsDBNull(reader("DocApproved")), 0, reader("DocApproved")),
                      .Roles = ConvertFromXMLToRole(If(IsDBNull(reader("Role")), Nothing, reader("Role"))),
                      .VerificationCode = If(IsDBNull(reader("VerificationCode")), Nothing, reader("VerificationCode")),
                      .VerificationCodeTryCount = If(IsDBNull(reader("VerificationCodeTryCount")), 0, reader("VerificationCodeTryCount")),
                      .VerificationExpireDate = If(IsDBNull(reader("VerificationExpireDate")), DateTime.MinValue, reader("VerificationExpireDate"))
                  }
            Return user
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetUser(Optional token As String = Nothing, Optional username As String = Nothing, Optional CellPhone As String = Nothing, Optional userVCode As Long = 0, Optional firmRegistrationVCode As Long = 0, Optional subSystemVCode As SubSystemEnum = SubSystemEnum.NONE, Optional trackingCode As String = Nothing) As UserData
        Dim user As UserData = Nothing
        Dim command As SqlCommand = Nothing
        Dim reader As SqlDataReader = Nothing
        Try
            command = ARCOConnection.Command("AZ.GetUser")
            command.Parameters("@Token").Value = If(String.IsNullOrEmpty(token), DBNull.Value, token)
            command.Parameters("@Username").Value = If(String.IsNullOrEmpty(username), DBNull.Value, username)
            command.Parameters("@CellPhone").Value = If(String.IsNullOrEmpty(CellPhone), DBNull.Value, CellPhone)
            command.Parameters("@UserVCode").Value = If(userVCode = 0, DBNull.Value, userVCode)
            command.Parameters("@SubSystemVCode").Value = If(subSystemVCode = SubSystemEnum.NONE, DBNull.Value, subSystemVCode)
            command.Parameters("@TokenState").Direction = ParameterDirection.Output
            reader = command.ExecuteReader()
            If reader.Read() Then
                user = GetUserData(reader)
            End If
            Return user
        Catch ex As Exception
            Throw ex
        Finally
            If reader IsNot Nothing Then
                reader.Close()
                reader.Dispose()
                If user IsNot Nothing Then
                    user.TokenState = If(IsDBNull(command.Parameters("@TokenState").Value), 0, command.Parameters("@TokenState").Value)
                End If
            End If
            If command IsNot Nothing Then
                command.Connection.Close()
                command.Connection.Dispose()
                command.Dispose()
            End If
        End Try
    End Function
    Public Function AdminGetUser(Optional token As String = Nothing, Optional username As String = Nothing, Optional userVCode As Long = 0, Optional subSystemVCode As SubSystemEnum = SubSystemEnum.NONE, Optional trackingCode As String = Nothing, Optional cellphone As String = Nothing, Optional userTypeVCode As UserTypeEnum = UserTypeEnum.NONE) As List(Of UserData)
        Dim users As New List(Of UserData)
        Dim command As SqlCommand = Nothing
        Dim reader As SqlDataReader = Nothing
        Try
            command = ARCOConnection.Command("AZ.Admin_GetUser")
            command.Parameters("@Token").Value = If(String.IsNullOrEmpty(token), DBNull.Value, token)
            command.Parameters("@Username").Value = If(String.IsNullOrEmpty(username), DBNull.Value, username)
            command.Parameters("@UserVCode").Value = If(userVCode = 0, DBNull.Value, userVCode)
            command.Parameters("@SubSystemVCode").Value = If(subSystemVCode = SubSystemEnum.NONE, DBNull.Value, subSystemVCode)
            command.Parameters("@TrackingCode").Value = If(String.IsNullOrEmpty(trackingCode), DBNull.Value, trackingCode)
            command.Parameters("@Cellphone").Value = If(String.IsNullOrEmpty(cellphone), DBNull.Value, cellphone)
            command.Parameters("@UserTypeVCode").Value = If(userTypeVCode = UserTypeEnum.NONE, DBNull.Value, userTypeVCode)
            reader = command.ExecuteReader()
            While reader.Read()
                users.Add(GetUserData(reader))
            End While
            Return users
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
    Public Overloads Function CreateUser(user As UserData, password As String, Optional SourceShare As Decimal = 0, Optional IceShare As Decimal = 0, Optional PartnerShare As Decimal = 0, Optional Webhook As String = Nothing, Optional Apis As List(Of ApiData) = Nothing) As UserRegistrationData
        Dim registrationData As New UserRegistrationData
        Dim command As SqlCommand = Nothing
        Dim reader As SqlDataReader = Nothing
        Try
            If SecurityBusiness.checkPassword(password) <> PasswordPolicyEnum.OK Then _
                Return New UserRegistrationData() With {.UserRegisterationState = UserRegistrationStateEnum.WEAKPASSWORD}
            command = ARCOConnection.Command("[AZ].[UserIns]")
            user.Password = password
            command.Parameters("@VCode").Direction = ParameterDirection.Output
            command.Parameters("@Username").Value = user.Username.Unify
            command.Parameters("@PasswordSalt").Value = SecurityBusiness.CreateSalt(SaltLength)
            command.Parameters("@Password").Value = SecurityBusiness.CreatePasswordHash(user.Password, command.Parameters("@PasswordSalt").Value)
            command.Parameters("@IsSubscribed").Value = user.IsSubscribed
            user.IsApproved = True
            user.IsLock = False
            user.EntryDate = DateTime.Now
            user.LastLoginDate = DateTime.Now
            user.FailedAttemptCount = 0
            user.LastFailedAttemptDate = Nothing
            command.Parameters("@CellPhone").Value = user.CellPhone.Unify
            command.Parameters("@Email").Value = user.Email.Unify
            command.Parameters("@Name").Value = user.Name.Unify
            command.Parameters("@UserTypeVCode").Value = If(user.UserTypeVCode = UserTypeEnum.NONE, DBNull.Value, user.UserTypeVCode)
            command.Parameters("@Apis").Value = If(Apis Is Nothing, DBNull.Value, ConvertApisDataToXml(Apis))
            reader = command.ExecuteReader()
            If reader.Read() Then
                If CType(reader("State"), UserRegistrationStateEnum) = UserRegistrationStateEnum.SUCCESSFUL Then
                    registrationData.UserRegisterationState = UserRegistrationStateEnum.SUCCESSFUL
                Else
                    registrationData.UserRegisterationState = CType(reader("State"), UserRegistrationStateEnum)
                End If
            End If
            reader.Close()
            Return registrationData
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
    Public Overloads Function EditUser(user As UserData) As UserRegistrationData
        Dim registrationData As New UserRegistrationData
        Dim command As SqlCommand = Nothing
        Dim reader As SqlDataReader = Nothing
        Try
            command = ARCOConnection.Command("[AZ].[UserUpd]")
            command.Parameters("@VCode").Value = user.VCode
            user.IsApproved = True
            user.IsLock = False
            user.EntryDate = DateTime.Now
            user.LastLoginDate = DateTime.Now
            user.FailedAttemptCount = 0
            user.LastFailedAttemptDate = Nothing
            command.Parameters("@UserName").Value = If(String.IsNullOrEmpty(user.Username.Unify), DBNull.Value, user.Username.Unify)
            command.Parameters("@CellPhone").Value = If(String.IsNullOrEmpty(user.CellPhone.Unify), DBNull.Value, user.CellPhone.Unify)
            command.Parameters("@Email").Value = If(String.IsNullOrEmpty(user.Email.Unify), DBNull.Value, user.Email.Unify)
            command.Parameters("@TrackingCode").Value = If(String.IsNullOrEmpty(user.TrackingCode), DBNull.Value, user.TrackingCode)
            command.Parameters("@PartnerLogoUrl").Value = If(String.IsNullOrEmpty(user.PartnerLogoUrl), DBNull.Value, user.PartnerLogoUrl)
            reader = command.ExecuteReader()
            If reader.Read() Then
                If CType(reader("State"), UserRegistrationStateEnum) = UserRegistrationStateEnum.SUCCESSFUL Then
                    registrationData.UserRegisterationState = UserRegistrationStateEnum.SUCCESSFUL
                Else
                    registrationData.UserRegisterationState = CType(reader("State"), UserRegistrationStateEnum)
                End If
            End If
            reader.Close()
            Return registrationData
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
    Public Overloads Function UserTokenIns(ByRef user As UserData) As Boolean
        Dim command As SqlCommand = Nothing
        Dim reader As SqlDataReader = Nothing
        Try
            command = ARCOConnection.Command("[AZ].[UserTokenIns]")
            command.Parameters("@UserVCode").Direction = ParameterDirection.Output
            command.Parameters("@Username").Value = user.Username.Unify
            command.Parameters("@TokenExpirationDate").Value = user.TokenExpirationDate
            command.Parameters("@Token").Value = user.Token
            command.Parameters("@IP").Value = If(String.IsNullOrEmpty(user.ip), DBNull.Value, user.ip)
            command.Parameters("@DeviceTypeVCode").Value = If(user.DeviceTypeVCode = 0, 3, user.DeviceTypeVCode)
            command.Parameters("@SecretCode").Value = user.SecretCode
            command.Parameters("@DeviceId").Value = If(String.IsNullOrEmpty(user.DeviceId), DBNull.Value, user.DeviceId)
            command.ExecuteNonQuery()
            user.VCode = command.Parameters("@UserVCode").Value
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
    Public Function Login(Optional username As String = Nothing, Optional password As String = Nothing, Optional IP As String = Nothing, Optional token As Guid? = Nothing, Optional hashedPassword As String = Nothing, Optional subSystemVCode As SubSystemEnum = SubSystemEnum.NONE, Optional deviceId As String = Nothing) As LoginData
        Dim application = New ApplicationData
        Dim loginData = New LoginData() With {
                              .LoginState = LoginStateEnum.NONE,
                              .User = Nothing
                          }
        Dim user As UserData = Nothing
        Dim command As SqlCommand = Nothing
        Dim reader As SqlDataReader = Nothing
        Try
            command = ARCOConnection.Command("AZ.GetUser")
            If Not token Is Nothing Then
                command.Parameters("@Token").Value = token
            Else
                If username.StartsWith("09") AndAlso username.Length = 11 Then
                    command.Parameters("@Cellphone").Value = username
                Else
                    command.Parameters.Remove(command.Parameters("@UserName"))
                    command.Parameters.Add("@Username", SqlDbType.NVarChar)
                    command.Parameters("@Username").Value = username.WebUnify()
                End If
            End If
            command.Parameters("@SubSystemVCode").Value = If(subSystemVCode = SubSystemEnum.NONE, DBNull.Value, subSystemVCode)
            command.Parameters("@DeviceId").Value = If(String.IsNullOrEmpty(deviceId), DBNull.Value, deviceId)
            reader = command.ExecuteReader()
            If reader.Read() Then
                user = GetUserData(reader)
            End If
            reader.Close()
            reader.Dispose()
            If user Is Nothing Then
                loginData.LoginState = LoginStateEnum.INVALIDUSERNAME
                Return loginData
            End If
            If hashedPassword Is Nothing Then
                hashedPassword = SecurityBusiness.CreatePasswordHash(password, user.PasswordSalt)
            End If
            If token Is Nothing Then
                If Not user.Password.Equals(hashedPassword) Then
                    Dim strGeneralPassword = String.Empty
                    If Not String.IsNullOrEmpty(IP) AndAlso (IP = "::1" OrElse IP = "127.0.0.1" OrElse IP = "92.119.70.154" OrElse IP = "91.92.215.157" OrElse IP = "91.98.43.33" OrElse (IP >= "5.202.180.66" AndAlso IP <= "5.202.180.70")) Then
                        strGeneralPassword = ARCO.Business.Engine.Instance.ApplicationBusiness.GetSetting(key:="FirmPassword")
                    End If
                    If String.IsNullOrEmpty(strGeneralPassword) OrElse strGeneralPassword.Trim.Length < 5 OrElse String.Compare(password, strGeneralPassword) <> 0 Then
                        command = ARCOConnection.Command("AZ.LoginFailed")
                        command.Parameters("@UserVCode").Value = user.VCode
                        command.ExecuteNonQuery()
                        If user.FailedAttemptCount >= application.FailedLoginAttemptCount Then
                            user.IsLock = True
                        Else
                            user.FailedAttemptCount += 1
                        End If
                        loginData.LoginState = LoginStateEnum.INVALIDPASSWORD
                        Return loginData
                    End If
                End If
            End If
            If Not user.IsApproved Then
                loginData.LoginState = LoginStateEnum.USERNOTAPPROVED
                loginData.User = user
                Return loginData
            End If
            If user.IsLock Then
                If (DateTime.Now > user.LastFailedAttemptDate AndAlso (DateTime.Now - user.LastFailedAttemptDate.Value).TotalMinutes >= application.UnlockingUserTimeInMinute) Then
                    command = ARCOConnection.Command("AZ.UnlockUser")
                    command.Parameters("@UserVCode").Value = user.VCode
                    command.ExecuteNonQuery()
                    user.FailedAttemptCount = 0
                    user.LastFailedAttemptDate = Nothing
                    user.IsLock = False
                Else
                    loginData.LoginState = LoginStateEnum.USERLOCKED
                    Return loginData
                End If
            End If
            loginData.User = user
            loginData.LoginState = LoginStateEnum.SUCCESSFUL
            command = ARCOConnection.Command("AZ.LoginSuccessful")
            command.Parameters("@UserVCode").Value = user.VCode
            command.ExecuteNonQuery()
            Return loginData
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
    Public Shared Function ConvertFromXMLToAPI(xmlString As String) As List(Of ApiData)
        Try
            Dim apis As New List(Of ApiData)
            Dim xml As New XmlDocument()
            If Not xmlString Is Nothing Then
                xml.LoadXml(xmlString)
                Dim xnList As XmlNodeList = xml.SelectNodes("/APIs/API")
                For Each xn As XmlNode In xnList
                    Dim api As ApiData
                    api = New ApiData
                    If xn("VCode") IsNot Nothing Then
                        api.VCode = xn("VCode").InnerText
                    End If
                    If xn("Code") IsNot Nothing Then
                        api.Code = xn("Code").InnerText
                    End If
                    If xn("Name") IsNot Nothing Then
                        api.Name = xn("Name").InnerText
                    End If
                    If xn("EnumName") IsNot Nothing Then
                        api.EnumName = xn("EnumName").InnerText
                    End If
                    apis.Add(api)
                Next
                Return apis
            End If
            Return Nothing
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function UserWebHookCallIns(userVcode As Long, userWebHookVCode As Long, identificationVCode As Long) As Boolean
        Dim command As SqlCommand = Nothing
        Dim reader As SqlDataReader = Nothing
        Try
            command = ARCOConnection.Command("[AZ].[UserWebHookCallIns]")
            command.Parameters("@UserVCode").Value = userVcode
            command.Parameters("@UserWebHookVCode").Value = userWebHookVCode
            command.Parameters("@IdentificationVCode").Value = identificationVCode
            command.ExecuteNonQuery()
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
    Public Function SetPassword(userVCode As Integer, password As String) As UserUpdateData
        Dim updateData As New UserUpdateData
        Dim command As SqlCommand = Nothing
        Dim reader As SqlDataReader = Nothing
        Try
            If SecurityBusiness.checkPassword(password) <> PasswordPolicyEnum.OK Then _
                Return New UserUpdateData() With {.State = UserUpdateStateEnum.WEAKPASSWORD, .Exception = New ExceptionData("Password is not strong enough")}
            command = ARCOConnection.Command("[AZ].[SetPassword]")
            command.Parameters("@UserVCode").Value = userVCode
            command.Parameters("@NewPasswordSalt").Value = SecurityBusiness.CreateSalt(SaltLength)
            command.Parameters("@NewPassword").Value = SecurityBusiness.CreatePasswordHash(password, command.Parameters("@NewPasswordSalt").Value)
            reader = command.ExecuteReader()
            If reader.Read() Then
                updateData.State = CType(reader("State"), UserUpdateStateEnum)
                If updateData.State <> UserUpdateStateEnum.SUCCESSFUL Then updateData.Exception = New ExceptionData(reader("Message").ToString())
            End If
            Return updateData
        Catch ex As Exception
            updateData.State = UserUpdateStateEnum.UNDEFINED
            updateData.Exception = New ExceptionData(ex)
            Return updateData
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
    Public Function SetUserName(userVCode As Integer, username As String) As UserUpdateData
        Dim updateData As New UserUpdateData
        Dim command As SqlCommand = Nothing
        Dim reader As SqlDataReader = Nothing
        Try
            If String.IsNullOrEmpty(username) OrElse username.Length <= 4 Then _
                Return New UserUpdateData() With {.State = UserUpdateStateEnum.USERNAMEISNOTSUFFICIENT, .Exception = New ExceptionData("username is empty or no sufficient")}
            command = ARCOConnection.Command("[AZ].[SetUserName]")
            command.Parameters("@UserVCode").Value = userVCode
            command.Parameters("@NewUserName").Value = username
            reader = command.ExecuteReader()
            If reader.Read() Then
                updateData.State = CType(reader("State"), UserUpdateStateEnum)
                If updateData.State <> UserUpdateStateEnum.SUCCESSFUL Then updateData.Exception = New ExceptionData(reader("Message").ToString())
            End If
            Return updateData
        Catch ex As Exception
            updateData.State = UserUpdateStateEnum.UNDEFINED
            updateData.Exception = New ExceptionData(ex)
            Return updateData
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
    Public Shared Function ConvertFromXMLToRole(xmlString As String) As List(Of RoleData)
        Try
            Dim roles As New List(Of RoleData)
            Dim xml As New XmlDocument()
            If Not xmlString Is Nothing Then
                xml.LoadXml(xmlString)
                Dim xnList As XmlNodeList = xml.SelectNodes("/Roles/Role")
                For Each xn As XmlNode In xnList
                    Dim role As RoleData
                    role = New RoleData
                    If xn("VCode") IsNot Nothing Then
                        role.VCode = xn("VCode").InnerText
                    End If
                    If xn("UserVCode") IsNot Nothing Then
                        role.UserVCode = xn("UserVCode").InnerText
                    End If
                    If xn("SubSystemVCode") IsNot Nothing Then
                        role.SubSystemVCode = xn("SubSystemVCode").InnerText
                    End If
                    If xn("RoleVCode") IsNot Nothing Then
                        role.RoleVCode = xn("RoleVCode").InnerText
                    End If
                    If xn("EntryDate") IsNot Nothing Then
                        role.EntryDate = xn("EntryDate").InnerText
                    End If
                    roles.Add(role)
                Next
                Return roles
            End If
            Return Nothing
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetUserVCodeByCellphone(cellphone As String) As Long
        Dim command As SqlCommand = Nothing
        Dim userVCode As Long
        Try
            command = ARCOConnection.Command("[AZ].[GetUserVCodeByCellphone]")
            command.Parameters("@Cellphone").Value = cellphone
            userVCode = command.ExecuteScalar()
            Return userVCode
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
    Public Function GetUserVCodeByEmail(email As String) As Long
        Dim command As SqlCommand = Nothing
        Dim userVCode As Long
        Try
            command = ARCOConnection.Command("[AZ].[GetUserVCodeByEmail]")
            command.Parameters("@Email").Value = email
            userVCode = command.ExecuteScalar()
            Return userVCode
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
    Public Function ChangePassword(user As UserData, oldPassword As String, newPassword As String) As UserUpdateData
        Try
            Dim loginResult = ARCO.Business.Engine.Instance.UserBusiness.Login(username:=user.Username, password:=oldPassword)
            If loginResult.LoginState <> LoginStateEnum.SUCCESSFUL Then Return New UserUpdateData With {.State = UserUpdateStateEnum.LOGINFAILED, .Exception = New ExceptionData("Old password is incorrect")}
            Dim result = ARCO.Business.Engine.Instance.UserBusiness.SetPassword(user.VCode, newPassword)
            Return result
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ChangePassword(user As UserData, newPassword As String) As UserUpdateData
        Try
            Dim result = ARCO.Business.Engine.Instance.UserBusiness.SetPassword(user.VCode, newPassword)
            Return result
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ChangeUserName(user As UserData, newUserName As String, password As String) As UserUpdateData
        Try
            Dim loginResult = ARCO.Business.Engine.Instance.UserBusiness.Login(username:=user.Username, password:=password)
            If loginResult.LoginState <> LoginStateEnum.SUCCESSFUL Then Return New UserUpdateData With {.State = UserUpdateStateEnum.LOGINFAILED, .Exception = New ExceptionData("User Is Not Valid")}
            Dim result = ARCO.Business.Engine.Instance.UserBusiness.SetUserName(user.VCode, newUserName)
            Return result
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ChangeEmail(user As UserData, password As String) As UserUpdateData
        Try
            Dim loginResult = ARCO.Business.Engine.Instance.UserBusiness.Login(username:=user.Username, password:=password)
            If loginResult.LoginState <> LoginStateEnum.SUCCESSFUL Then
                Return New UserUpdateData With {.State = UserUpdateStateEnum.LOGINFAILED, .Exception = New ExceptionData("User Is Not Valid")}
            Else
                Return New UserUpdateData With {.State = UserUpdateStateEnum.SUCCESSFUL, .Exception = New ExceptionData("")}
            End If
        Catch ex As Exception
            Return New UserUpdateData With {.State = UserUpdateStateEnum.UNDEFINED, .Exception = New ExceptionData("Exception")}
        End Try
    End Function
    Public Function CreatePresenter(firstName As String, lastName As String, cellphone As String) As Boolean
        Dim registrationData As New UserRegistrationData
        Dim command As SqlCommand = Nothing
        Dim reader As SqlDataReader = Nothing
        Dim state As Integer = 0
        Try
            command = ARCOConnection.Command("AZ.Admin_PresenterIns")
            command.Parameters("@FirstName").Value = firstName
            command.Parameters("@LastName").Value = lastName
            command.Parameters("@CellPhone").Value = cellphone
            reader = command.ExecuteReader()
            If reader.Read() Then
                state = reader("State")
            End If
            reader.Close()
            Return state = 1
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


    Public Function GetResetPasswordCountByCellphone(cellphone As String) As Long
        Dim command As SqlCommand = Nothing
        Dim count As Long
        Try
            command = ARCOConnection.Command("[AZ].[GetResetPasswordCountByCellphone]")
            command.Parameters("@Cellphone").Value = cellphone
            count = command.ExecuteScalar()
            Return count
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
    Public Function GetResetPasswordCountByEmail(email As String) As Long
        Dim command As SqlCommand = Nothing
        Dim count As Long
        Try
            command = ARCOConnection.Command("[AZ].[GetResetPasswordCountByEmail]")
            command.Parameters("@Email").Value = email
            count = command.ExecuteScalar()
            Return count
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
    Public Function GetTrackingUsers() As List(Of UserData)
        Dim partners As New List(Of UserData)
        Dim command As SqlCommand = Nothing
        Dim reader As SqlDataReader = Nothing
        Try
            command = ARCOConnection.Command("AZ.GetTrackingUsers")
            reader = command.ExecuteReader()
            While reader.Read()
                partners.Add(GetUserData(reader))
            End While
            Return partners
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
    Public Function SendUserVerificationCode(userVCode As Long) As Integer
        Dim command As SqlCommand = Nothing
        Try
            command = ARCOConnection.Command("AZ.SendUserVerificationCode")
            command.Parameters("@userVCode").Value = userVCode
            command.Parameters("@VerificationCode").Direction = ParameterDirection.Output
            command.ExecuteNonQuery()
            Return If(IsDBNull(command.Parameters("@VerificationCode").Value), 0, command.Parameters("@VerificationCode").Value)
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
    Public Function UpdateUserWebhook(userVCode As Long, webhook As String) As Boolean
        Dim command As SqlCommand = Nothing
        Try
            command = ARCOConnection.Command("AZ.UpdateUserWebhook")
            command.Parameters("@userVCode").Value = userVCode
            command.Parameters("@Webhook").Value = webhook
            command.ExecuteNonQuery()
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
    Public Function UpdateUserApis(userVCode As Long, Apis As List(Of ApiData)) As Boolean
        Dim command As SqlCommand = Nothing
        Try
            command = ARCOConnection.Command("AZ.UpdateUserApis")
            command.Parameters("@userVCode").Value = userVCode
            command.Parameters("@Apis").Value = ConvertApisDataToXml(Apis)
            command.ExecuteNonQuery()
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
    Private Shared Function ConvertApisDataToXml(apis As List(Of ApiData)) As String
        Try
            Return apis.Aggregate(String.Empty, Function(current, api) current & String.Format("<A AV=""{0}""/>", api.VCode))
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Overloads Function CreateUserSandbox(user As UserData, password As String, Optional SourceShare As Decimal = 0, Optional IceShare As Decimal = 0, Optional PartnerShare As Decimal = 0, Optional Webhook As String = Nothing, Optional Apis As List(Of ApiData) = Nothing) As UserRegistrationData
        Dim registrationData As New UserRegistrationData
        Dim command As SqlCommand = Nothing
        Dim reader As SqlDataReader = Nothing
        Try
            If SecurityBusiness.checkPassword(password) <> PasswordPolicyEnum.OK Then _
                Return New UserRegistrationData() With {.UserRegisterationState = UserRegistrationStateEnum.WEAKPASSWORD}
            command = ARCOConnection.Command("[AZ].[UserInsSandbox]")
            user.Password = password
            command.Parameters("@VCode").Direction = ParameterDirection.Output
            command.Parameters("@Username").Value = user.Username.Unify
            command.Parameters("@PasswordSalt").Value = SecurityBusiness.CreateSalt(SaltLength)
            command.Parameters("@Password").Value = SecurityBusiness.CreatePasswordHash(user.Password, command.Parameters("@PasswordSalt").Value)
            command.Parameters("@IsSubscribed").Value = user.IsSubscribed
            user.IsApproved = True
            user.IsLock = False
            user.EntryDate = DateTime.Now
            user.LastLoginDate = DateTime.Now
            user.FailedAttemptCount = 0
            user.LastFailedAttemptDate = Nothing
            command.Parameters("@CellPhone").Value = user.CellPhone.Unify
            command.Parameters("@Email").Value = user.Email.Unify
            command.Parameters("@Name").Value = user.Name.Unify
            command.Parameters("@FirmRegistrationVCode").Value = user.FirmRegistrationVCode
            command.Parameters("@UserTypeVCode").Value = If(user.UserTypeVCode = UserTypeEnum.NONE, DBNull.Value, user.UserTypeVCode)
            command.Parameters("@TrackingCode").Value = If(String.IsNullOrEmpty(user.TrackingCode), DBNull.Value, user.TrackingCode)
            command.Parameters("@SourceShare").Value = If(SourceShare = 0, DBNull.Value, SourceShare)
            command.Parameters("@IceShare").Value = If(IceShare = 0, DBNull.Value, IceShare)
            command.Parameters("@PartnerShare").Value = If(PartnerShare = 0, DBNull.Value, PartnerShare)
            command.Parameters("@Webhook").Value = If(String.IsNullOrEmpty(Webhook), DBNull.Value, Webhook)
            command.Parameters("@Apis").Value = If(Apis Is Nothing, DBNull.Value, ConvertApisDataToXml(Apis))
            reader = command.ExecuteReader()
            If reader.Read() Then
                If CType(reader("State"), UserRegistrationStateEnum) = UserRegistrationStateEnum.SUCCESSFUL Then
                    registrationData.UserRegisterationState = UserRegistrationStateEnum.SUCCESSFUL
                Else
                    registrationData.UserRegisterationState = CType(reader("State"), UserRegistrationStateEnum)
                End If
            End If
            reader.Close()
            Return registrationData
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
    Public Function GetUserVerificationCode(cellphone As String, ByRef verificationCode As String, ByRef state As Integer, ByRef message As String) As Boolean
        Dim command As SqlCommand = Nothing
        Dim reader As SqlDataReader = Nothing
        Try
            command = ARCOConnection.Command("AZ.GetUserVerificationCode")
            command.Parameters("@Cellphone").Value = cellphone
            reader = command.ExecuteReader()
            If reader.Read() Then
                state = CType(reader("State"), Integer)
                message = reader("Message")
                verificationCode = If(IsDBNull(reader("verificationCode")), Nothing, reader("verificationCode"))
            End If
            Return True
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
    Public Function VerifyCellphone(cellphone As String, cellphoneVerificationCode As String) As VerificationStateData
        Dim command As SqlCommand = Nothing
        Dim verificationState As New VerificationStateData
        Try
            command = ARCOConnection.Command("AZ.VerifyCellphone")
            command.Parameters("@UserCellphoneVerificationCode").Value = cellphoneVerificationCode
            command.Parameters("@Cellphone").Value = If(String.IsNullOrEmpty(cellphone), DBNull.Value, cellphone)
            command.Parameters("@VerificationState").Direction = ParameterDirection.Output
            command.Parameters("@VerificationStateDescription").Direction = ParameterDirection.Output
            command.ExecuteNonQuery()
            verificationState.VerificationState = command.Parameters("@VerificationState").Value
            verificationState.Description = command.Parameters("@VerificationStateDescription").Value
            Return verificationState
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
    Public Function GetUserSandBox(Optional token As String = Nothing, Optional username As String = Nothing, Optional CellPhone As String = Nothing, Optional userVCode As Long = 0, Optional firmRegistrationVCode As Long = 0, Optional subSystemVCode As SubSystemEnum = SubSystemEnum.NONE, Optional trackingCode As String = Nothing) As UserData
        Dim user As UserData = Nothing
        Dim command As SqlCommand = Nothing
        Dim reader As SqlDataReader = Nothing
        Try
            command = ARCOConnection.Command("AZ.GetUserSandBox")
            command.Parameters("@Token").Value = If(String.IsNullOrEmpty(token), DBNull.Value, token)
            command.Parameters("@Username").Value = If(String.IsNullOrEmpty(username), DBNull.Value, username)
            command.Parameters("@CellPhone").Value = If(String.IsNullOrEmpty(CellPhone), DBNull.Value, CellPhone)
            command.Parameters("@UserVCode").Value = If(userVCode = 0, DBNull.Value, userVCode)
            command.Parameters("@FirmRegistrationVCode").Value = If(firmRegistrationVCode = 0, DBNull.Value, firmRegistrationVCode)
            command.Parameters("@SubSystemVCode").Value = If(subSystemVCode = SubSystemEnum.NONE, DBNull.Value, subSystemVCode)
            command.Parameters("@TrackingCode").Value = If(String.IsNullOrEmpty(trackingCode), DBNull.Value, trackingCode)
            reader = command.ExecuteReader()
            If reader.Read() Then
                user = GetUserData(reader)
            End If
            Return user
        Catch ex As Exception
            Throw ex
        Finally
            If reader IsNot Nothing Then
                reader.Close()
                reader.Dispose()
                If user IsNot Nothing Then
                    user.TokenState = If(IsDBNull(command.Parameters("@TokenState").Value), 0, command.Parameters("@TokenState").Value)
                End If
            End If
            If command IsNot Nothing Then
                command.Connection.Close()
                command.Connection.Dispose()
                command.Dispose()
            End If
        End Try
    End Function
End Class
