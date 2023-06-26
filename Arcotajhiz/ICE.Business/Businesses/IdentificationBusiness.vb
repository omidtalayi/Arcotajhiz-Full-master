Imports System.Data.SqlClient
Imports ARCO.Business.Models
Imports ARCO.Entities.Enums
Imports System.Xml

Public Class IdentificationBusiness
    'Private Shared Function GetReportTypeData(reader As IDataRecord) As ReportTypeData
    '    Try
    '        Dim reportType = New ReportTypeData With
    '                  {
    '                      .VCode = reader("VCode"),
    '                      .Code = If(IsDBNull(reader("Code")), 0, reader("Code")),
    '                      .Name = If(IsDBNull(reader("Name")), Nothing, reader("Name")),
    '                      .EnumName = If(IsDBNull(reader("EnumName")), Nothing, reader("EnumName"))
    '                  }
    '        Return reportType
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Function
    'Private Shared Function GetBatchResultXmlData(reader As IDataRecord) As BatchResultXmlData
    '    Try
    '        Dim batchResultXml = New BatchResultXmlData With
    '                  {
    '                      .ResponseXml = If(IsDBNull(reader("ResponseXml")), Nothing, reader("ResponseXml")),
    '                      .ResponseXmlScore = If(IsDBNull(reader("ResponseXmlScore")), Nothing, reader("ResponseXmlScore")),
    '                      .ResponseXmlEmpty = If(IsDBNull(reader("ResponseXmlEmpty")), Nothing, reader("ResponseXmlEmpty")),
    '                      .ResponseJson = If(IsDBNull(reader("ResponseJson")), Nothing, reader("ResponseJson"))
    '                  }
    '        Return batchResultXml
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Function
    'Private Shared Function GetUserLastSevenDaysSeenData(reader As IDataRecord) As UserLastSevenDaysSeenData
    '    Try
    '        Dim userLastSevenDaysSeen = New UserLastSevenDaysSeenData With
    '                  {
    '                      .ReportDate = If(IsDBNull(reader("ReportDate")), Nothing, reader("ReportDate")),
    '                      .Cnt = If(IsDBNull(reader("Cnt")), 0, reader("Cnt"))
    '                  }
    '        Return userLastSevenDaysSeen
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Function
    'Private Shared Function GetUserLastSevenHoursData(reader As IDataRecord) As UserLastSevenHoursData
    '    Try
    '        Dim UserLastSevenHours = New UserLastSevenHoursData With
    '                  {
    '                      .Hour = If(IsDBNull(reader("Hour")), 0, reader("Hour")),
    '                      .ConfirmedCnt = If(IsDBNull(reader("ConfirmedCnt")), 0, reader("ConfirmedCnt")),
    '                      .RejectedCnt = If(IsDBNull(reader("RejectedCnt")), 0, reader("RejectedCnt")),
    '                      .WaitingPersonConfirmationCnt = If(IsDBNull(reader("WaitingPersonConfirmationCnt")), 0, reader("WaitingPersonConfirmationCnt"))
    '                  }
    '        Return UserLastSevenHours
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Function
    'Private Shared Function GetReportLinkAndExpirationDateData(reader As IDataRecord) As ReportLinkAndExpirationDateData
    '    Try
    '        Dim reportLinkAndExpirationDate = New ReportLinkAndExpirationDateData With
    '                  {
    '                      .ReportExpirationDate = If(IsDBNull(reader("ReportExpirationDate")), DateTime.MinValue, reader("ReportExpirationDate")),
    '                      .ReportLink = If(IsDBNull(reader("ReportLink")), Nothing, reader("ReportLink")),
    '                      .IdentificationVCode = If(IsDBNull(reader("IdentificationVCode")), 0, reader("IdentificationVCode")),
    '                      .IdentificationState = If(IsDBNull(reader("IdentificationStateVCode")), IdentificationStateEnum.NONE, reader("IdentificationStateVCode"))
    '                  }
    '        Return reportLinkAndExpirationDate
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Function
    'Private Shared Function GetIdentificationData(reader As IDataRecord) As IdentificationData
    '    Try
    '        Dim identification = New IdentificationData With
    '              {
    '                  .VCode = reader("VCode"),
    '                  .Row = reader("R"),
    '                  .Cellphone = If(IsDBNull(reader("Cellphone")), Nothing, reader("Cellphone")),
    '                  .NationalCode = If(IsDBNull(reader("NationalCode")), Nothing, reader("NationalCode")),
    '                  .VerificationCode = If(IsDBNull(reader("VerificationCode")), 0, reader("VerificationCode")),
    '                  .VerificationLink = If(IsDBNull(reader("VerificationLink")), Nothing, reader("VerificationLink")),
    '                  .IsVerified = If(IsDBNull(reader("IsVerified")), 0, reader("IsVerified")),
    '                  .HasShahkarIdentified = If(IsDBNull(reader("HasShahkarIdentified")), 0, reader("HasShahkarIdentified")),
    '                  .ExpirationDate = If(IsDBNull(reader("ExpirationDate")), Nothing, reader("ExpirationDate")),
    '                  .IdentificationState = ConvertFromXMLToIdentificationState(If(IsDBNull(reader("IdentificationState")), Nothing, reader("IdentificationState"))),
    '                  .IdentificationType = ConvertFromXMLToIdentificationType(If(IsDBNull(reader("IdentificationType")), Nothing, reader("IdentificationType"))),
    '                  .ReportExpirationDate = If(IsDBNull(reader("ReportExpirationDate")), Nothing, reader("ReportExpirationDate")),
    '                  .ReportLink = If(IsDBNull(reader("ReportLink")), Nothing, reader("ReportLink")),
    '                  .IdentificationSendToOthers = ConvertFromXMLToIdentificationSendToOthers(If(IsDBNull(reader("IdentificationSendToOthers")), Nothing, reader("IdentificationSendToOthers"))),
    '                  .AvailabilityCheck = If(IsDBNull(reader("AvailabilityCheck")), False, reader("AvailabilityCheck")),
    '                  .HasCheckedKyc = If(IsDBNull(reader("HasCheckedKyc")), False, reader("HasCheckedKyc")),
    '                  .TrackingCode = If(IsDBNull(reader("TrackingCode")), 0, reader("TrackingCode")),
    '                  .EntryDate = If(IsDBNull(reader("EntryDate")), Nothing, reader("EntryDate")),
    '                  .LastModifiedDate = If(IsDBNull(reader("LastModifiedDate")), Nothing, reader("LastModifiedDate")),
    '                  .UserVCode = If(IsDBNull(reader("UserVCode")), 0, reader("UserVCode")),
    '                  .UserPaymentTypeVCode = If(IsDBNull(reader("UserPaymentTypeVCode")), UserPaymentTypeEnum.NONE, reader("UserPaymentTypeVCode")),
    '                  .FromFirmPanel = If(IsDBNull(reader("FromFirmPanel")), False, reader("FromFirmPanel")),
    '                  .UserPaymentTypeName = If(IsDBNull(reader("UserPaymentTypeName")), Nothing, reader("UserPaymentTypeName")),
    '                  .TraceNo = If(IsDBNull(reader("TraceNo")), Nothing, reader("TraceNo")),
    '                  .PaymentAmount = If(IsDBNull(reader("PaymentAmount")), 0, reader("PaymentAmount")),
    '                  .PaymentDate = If(IsDBNull(reader("PaymentDate")), Nothing, reader("PaymentDate")),
    '                  .SecondCellphone = If(IsDBNull(reader("SecondCellphone")), Nothing, reader("SecondCellphone")),
    '                  .SecondCellphoneIsVerified = If(IsDBNull(reader("SecondCellphoneIsVerified")), False, reader("SecondCellphoneIsVerified")),
    '                  .SecondCellphoneHasCheckedKyc = If(IsDBNull(reader("SecondCellphoneHasCheckedKyc")), False, reader("SecondCellphoneHasCheckedKyc")),
    '                  .ReceiverCellphone = If(IsDBNull(reader("ReceiverCellphone")), Nothing, reader("ReceiverCellphone")),
    '                  .ReceiverCellphoneIsVerified = If(IsDBNull(reader("ReceiverCellphoneIsVerified")), False, reader("ReceiverCellphoneIsVerified")),
    '                  .ReceiverCellphoneVerificationCode = If(IsDBNull(reader("ReceiverCellphoneVerificationCode")), 0, reader("ReceiverCellphoneVerificationCode")),
    '                  .ReceiverCellphoneExpirationDate = If(IsDBNull(reader("ReceiverCellphoneExpirationDate")), Nothing, reader("ReceiverCellphoneExpirationDate")),
    '                  .SendSmsTryCount = If(IsDBNull(reader("SendSmsTryCount")), 0, reader("SendSmsTryCount")),
    '                  .SiteHasBeenRepaired = If(IsDBNull(reader("SiteHasBeenRepaired")), False, reader("SiteHasBeenRepaired")),
    '                  .ReportSuccessfullySent = If(IsDBNull(reader("ReportSuccessfullySent")), False, reader("ReportSuccessfullySent")),
    '                  .CompanyNationalID = If(IsDBNull(reader("CompanyNationalID")), 0, reader("CompanyNationalID")),
    '                  .IsLegalPerson = If(IsDBNull(reader("IsLegalPerson")), False, reader("IsLegalPerson")),
    '                  .RedirectUrlICS24 = If(IsDBNull(reader("RedirectUrlICS24")), Nothing, reader("RedirectUrlICS24")),
    '                  .IdICS24 = If(IsDBNull(reader("IdICS24")), Nothing, reader("IdICS24")),
    '                  .SendSmsPeygiri = If(IsDBNull(reader("SendSmsPeygiri")), Nothing, reader("SendSmsPeygiri")),
    '                  .IsPendingICS24Service = If(IsDBNull(reader("IsPendingICS24Service")), False, reader("IsPendingICS24Service")),
    '                  .IsRepairMessageSent = If(IsDBNull(reader("IsRepairMessageSent")), False, reader("IsRepairMessageSent")),
    '                  .appIcs24HashCode = If(IsDBNull(reader("appIcs24HashCode")), Nothing, reader("appIcs24HashCode")),
    '                  .ResponseJson = If(IsDBNull(reader("ResponseJson")), Nothing, reader("ResponseJson")),
    '                  .TrackingId = If(IsDBNull(reader("TrackingId")), Nothing, reader("TrackingId")),
    '                  .ReportIsCorrupted = If(IsDBNull(reader("ReportIsCorrupted")), False, reader("ReportIsCorrupted")),
    '                  .FromApp = If(IsDBNull(reader("FromApp")), False, reader("FromApp")),
    '                  .IdentificationComplaintStateVCode = If(IsDBNull(reader("IdentificationComplaintStateVCode")), IdentificationComplaintStateEnum.NONE, reader("IdentificationComplaintStateVCode")),
    '                  .GUID = If(IsDBNull(reader("GUID")), Nothing, reader("GUID"))
    '              }
    '        Return identification
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Function
    'Private Shared Function GetIdentificationStateData(reader As IDataRecord) As IdentificationStateData
    '    Try
    '        Dim identificationState = New IdentificationStateData With
    '                  {
    '                      .VCode = reader("VCode"),
    '                      .Code = If(IsDBNull(reader("Code")), 0, reader("Code")),
    '                      .Name = If(IsDBNull(reader("Name")), Nothing, reader("Name")),
    '                      .EnumName = If(IsDBNull(reader("EnumName")), Nothing, reader("EnumName"))
    '                  }
    '        Return identificationState
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Function
    'Private Shared Function IdentificationSendToOthersData(reader As IDataRecord) As IdentificationSendToOthersData
    '    Try
    '        Dim identificationSendToOthers = New IdentificationSendToOthersData With
    '            {
    '                .VCode = reader("VCode"),
    '                .IdentificationVCode = If(IsDBNull(reader("IdentificationVCode")), 0, reader("IdentificationVCode")),
    '                .IdentificationSendToOthersTypeVCode = If(IsDBNull(reader("IdentificationSendToOthersTypeVCode")), IdentificationSendToOthersTypeEnum.NONE, reader("IdentificationSendToOthersTypeVCode")),
    '                .Receiver = If(IsDBNull(reader("Receiver")), Nothing, reader("Receiver")),
    '                .EntryDate = If(IsDBNull(reader("EntryDate")), DateTime.MinValue, reader("EntryDate"))
    '            }
    '        Return identificationSendToOthers
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Function
    'Private Shared Function GetAllIdentificationStateData(reader As IDataRecord) As AllIdentificationStateData
    '    Try
    '        Dim allIdentificationState = New AllIdentificationStateData With
    '                  {
    '                      .IdentificationStateVCode = If(IsDBNull(reader("IdentificationStateVCode")), IdentificationStateEnum.NONE, reader("IdentificationStateVCode")),
    '                      .Cnt = If(IsDBNull(reader("cnt")), 0, reader("cnt"))
    '                  }
    '        Return allIdentificationState
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Function
    'Private Shared Function GetIdentificationPaidData(reader As IDataRecord) As IdentificationPaidData
    '    Try
    '        Dim identificationPaid = New IdentificationPaidData With
    '                  {
    '                      .VCode = reader("VCode"),
    '                      .IdentificationStateVCode = If(IsDBNull(reader("IdentificationStateVCode")), IdentificationStateEnum.NONE, reader("IdentificationStateVCode")),
    '                      .Cellphone = If(IsDBNull(reader("Cellphone")), Nothing, reader("Cellphone")),
    '                      .NationalCode = If(IsDBNull(reader("NationalCode")), Nothing, reader("NationalCode")),
    '                      .EntryDate = If(IsDBNull(reader("EntryDate")), Nothing, reader("EntryDate"))
    '                  }
    '        Return identificationPaid
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Function
    'Public Function GetIdentification(Optional vcode As Long = 0, Optional trackingCode As Long = 0, Optional idICS24 As String = Nothing, Optional appIcs24HashCode As String = Nothing) As IdentificationData
    '    Dim identification As IdentificationData = Nothing
    '    Dim command As SqlCommand = Nothing
    '    Dim reader As SqlDataReader = Nothing
    '    Try
    '        command = ARCOConnection.Command("AZ.GetIdentification")
    '        command.Parameters("@VCode").Value = If(vcode = 0, DBNull.Value, vcode)
    '        command.Parameters("@TrackingCode").Value = If(trackingCode = 0, DBNull.Value, trackingCode)
    '        command.Parameters("@IdICS24").Value = If(String.IsNullOrEmpty(idICS24), DBNull.Value, idICS24)
    '        command.Parameters("@AppIcs24HashCode").Value = If(String.IsNullOrEmpty(appIcs24HashCode), DBNull.Value, appIcs24HashCode)
    '        reader = command.ExecuteReader()
    '        If reader.Read() Then
    '            identification = GetIdentificationData(reader)
    '        End If
    '        Return identification
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If reader IsNot Nothing Then
    '            reader.Close()
    '            reader.Dispose()
    '        End If
    '        If command IsNot Nothing Then
    '            command.Connection.Close()
    '            command.Connection.Dispose()
    '            command.Dispose()
    '        End If
    '    End Try
    'End Function
    'Public Function GetIdentificationSimple(vcode As Long) As IdentificationData
    '    Dim identification As IdentificationData = Nothing
    '    Dim command As SqlCommand = Nothing
    '    Dim reader As SqlDataReader = Nothing
    '    Try
    '        command = ARCOConnection.Command("AZ.GetIdentificationSimple")
    '        command.Parameters("@VCode").Value = If(vcode = 0, DBNull.Value, vcode)
    '        reader = command.ExecuteReader()
    '        If reader.Read() Then
    '            identification = GetIdentificationData(reader)
    '        End If
    '        Return identification
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If reader IsNot Nothing Then
    '            reader.Close()
    '            reader.Dispose()
    '        End If
    '        If command IsNot Nothing Then
    '            command.Connection.Close()
    '            command.Connection.Dispose()
    '            command.Dispose()
    '        End If
    '    End Try
    'End Function
    'Public Function GetIdentifications(Optional cellphone As String = Nothing, Optional nationalCode As String = Nothing, Optional userVCode As Integer = 0, Optional identificationStates As List(Of IdentificationStateData) = Nothing, Optional pageNo As Integer = 0, Optional pageSize As Integer = 0, Optional fromDate As String = Nothing, Optional toDate As String = Nothing, Optional withoutPartners As Boolean = False, Optional withouExpired As Boolean = False, Optional hasComplaint As Boolean = False, Optional IdentificationTypeVCode As IdentificationTypeEnum = IdentificationTypeEnum.NONE, Optional complaintStates As List(Of IdentificationComplaintStateEnum) = Nothing, Optional companyNationalId As String = Nothing, Optional trackingCode As Long = 0) As List(Of IdentificationData)
    '    Dim identifications As New List(Of IdentificationData)
    '    Dim command As SqlCommand = Nothing
    '    Dim reader As SqlDataReader = Nothing
    '    Try
    '        command = ARCOConnection.Command("AZ.GetIdentifications")
    '        command.Parameters("@Cellphone").Value = If(String.IsNullOrEmpty(cellphone), DBNull.Value, cellphone)
    '        command.Parameters("@NationalCode").Value = If(String.IsNullOrEmpty(nationalCode), DBNull.Value, nationalCode)
    '        command.Parameters("@UserVCode").Value = If(userVCode = 0, DBNull.Value, userVCode)
    '        command.Parameters("@IdentificationStates").Value = If(identificationStates Is Nothing OrElse identificationStates.Count = 0, DBNull.Value, ConvertIdentificationStatesDataToXml(identificationStates))
    '        command.Parameters("@PageNo").Value = If(pageNo = 0, DBNull.Value, pageNo)
    '        command.Parameters("@PageSize").Value = If(pageSize = 0, DBNull.Value, pageSize)
    '        command.Parameters("@FromJDate").Value = If(fromDate Is Nothing, DBNull.Value, fromDate)
    '        command.Parameters("@ToJDate").Value = If(toDate Is Nothing, DBNull.Value, toDate)
    '        command.Parameters("@WithoutPartners").Value = withoutPartners
    '        command.Parameters("@WithouExpired").Value = withouExpired
    '        command.Parameters("@HasComplaint").Value = hasComplaint
    '        command.Parameters("@IdentificationTypeVCode").Value = If(IdentificationTypeVCode = IdentificationTypeEnum.NONE, DBNull.Value, CType(IdentificationTypeVCode, Integer))
    '        command.Parameters("@IdentificationComplaintStateVCodes").Value = If(complaintStates Is Nothing, DBNull.Value, ConvertIdentificationComplaintStatesDataToXml(complaintStates))
    '        command.Parameters("@CompanyNationalId").Value = If(String.IsNullOrEmpty(companyNationalId), DBNull.Value, companyNationalId)
    '        command.Parameters("@TrackingCode").Value = If(trackingCode = 0, DBNull.Value, trackingCode)
    '        reader = command.ExecuteReader()
    '        While reader.Read()
    '            identifications.Add(GetIdentificationData(reader))
    '        End While
    '        Return identifications
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If reader IsNot Nothing Then
    '            reader.Close()
    '            reader.Dispose()
    '        End If
    '        If command IsNot Nothing Then
    '            command.Connection.Close()
    '            command.Connection.Dispose()
    '            command.Dispose()
    '        End If
    '    End Try
    'End Function
    'Public Function AddIdentification(ByRef vcode As Long, cellphone As String, nationalCode As String, ByRef verificationCode As Integer, Optional userVCode As Integer = 0, Optional ByRef LinkToken As String = Nothing, Optional userPaymentTypeVCode As UserPaymentTypeEnum = UserPaymentTypeEnum.PAY_BY_CUSTOMER, Optional fromFirmPanel As Boolean = False, Optional receiverCellphone As String = Nothing, ByRef Optional receiverCellphoneVerificationCode As Integer = 0, Optional companyNationalID As String = Nothing, Optional identificationTypeVCode As IdentificationTypeEnum = IdentificationTypeEnum.INDIVIDUAL, Optional isFromSendSmsPeygiri As Boolean = False, Optional hasChequeReport As Boolean = False, Optional TrackingId As String = Nothing, Optional InvitationCode As String = Nothing, Optional fromApp As Boolean = False) As IdentificationInsStateEnum
    '    Dim command As SqlCommand = Nothing
    '    Dim cryptography As New CryptographyBusiness()
    '    Dim Link As String = "/IndividualVerification?id="
    '    Dim linkCommand As SqlCommand = Nothing
    '    Dim identificationInsState As IdentificationInsStateEnum = IdentificationInsStateEnum.SUCCESSFUL
    '    Try
    '        command = ARCOConnection.Command("AZ.IdentificationIns")
    '        command.Parameters("@VCode").Direction = ParameterDirection.Output
    '        command.Parameters("@Cellphone").Value = cellphone
    '        command.Parameters("@NationalCode").Value = nationalCode
    '        command.Parameters("@UserVCode").Value = userVCode
    '        command.Parameters("@UserPaymentTypeVCode").Value = CType(userPaymentTypeVCode, Integer)
    '        command.Parameters("@FromFirmPanel").Value = fromFirmPanel
    '        command.Parameters("@VerificationCode").Direction = ParameterDirection.Output
    '        command.Parameters("@IdentificationInsStateVCode").Direction = ParameterDirection.Output
    '        command.Parameters("@ReceiverCellphone").Value = If(String.IsNullOrEmpty(receiverCellphone), DBNull.Value, receiverCellphone)
    '        command.Parameters("@ReceiverCellphoneVerificationCode").Direction = ParameterDirection.Output
    '        command.Parameters("@CompanyNationalID").Value = If(String.IsNullOrEmpty(companyNationalID), DBNull.Value, companyNationalID)
    '        command.Parameters("@IdentificationTypeVCode").Value = CType(identificationTypeVCode, Integer)
    '        command.Parameters("@IsFromSendSmsPeygiri").Value = isFromSendSmsPeygiri
    '        command.Parameters("@HasChequeReport").Value = hasChequeReport
    '        command.Parameters("@TrackingId").Value = If(String.IsNullOrEmpty(TrackingId), DBNull.Value, TrackingId)
    '        command.Parameters("@InvitationCode").Value = If(String.IsNullOrEmpty(InvitationCode), DBNull.Value, InvitationCode)
    '        command.Parameters("@FromApp").Value = fromApp
    '        command.ExecuteNonQuery()
    '        vcode = If(IsDBNull(command.Parameters("@VCode").Value), 0, command.Parameters("@VCode").Value)
    '        verificationCode = command.Parameters("@VerificationCode").Value
    '        receiverCellphoneVerificationCode = If(IsDBNull(command.Parameters("@ReceiverCellphoneVerificationCode").Value), 0, command.Parameters("@ReceiverCellphoneVerificationCode").Value)
    '        identificationInsState = command.Parameters("@IdentificationInsStateVCode").Value
    '        LinkToken = StringToURL(cryptography.Encrypt(sPlainText:=vcode, keyType:=EncryptKeyEnum.Key1))
    '        linkCommand = ARCOConnection.Command("AZ.SetVerificationLink")
    '        linkCommand.Parameters("@IdentificationVCode").Value = vcode
    '        linkCommand.Parameters("@VerificationLink").Value = Link & LinkToken
    '        linkCommand.ExecuteNonQuery()
    '        Return identificationInsState
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If command IsNot Nothing Then
    '            command.Connection.Close()
    '            command.Connection.Dispose()
    '            command.Dispose()
    '        End If
    '    End Try
    'End Function
    'Public Function AddIdentificationApi(ByRef vcode As Long, cellphone As String, nationalCode As String, ByRef verificationCode As Integer, ip As String, saleAmount As Decimal, saleRefID As String, Optional userVCode As Integer = 0, Optional ByRef LinkToken As String = Nothing, Optional ByRef State As Boolean = False) As Boolean
    '    Dim command As SqlCommand = Nothing
    '    Dim cryptography As New CryptographyBusiness()
    '    Dim Link As String = "/IndividualVerification?id="
    '    Dim linkCommand As SqlCommand = Nothing
    '    Try
    '        command = ARCOConnection.Command("AZ.IdentificationInsApi")
    '        command.Parameters("@VCode").Direction = ParameterDirection.Output
    '        command.Parameters("@State").Direction = ParameterDirection.Output
    '        command.Parameters("@Cellphone").Value = cellphone
    '        command.Parameters("@NationalCode").Value = nationalCode
    '        command.Parameters("@UserVCode").Value = userVCode
    '        command.Parameters("@VerificationCode").Direction = ParameterDirection.Output
    '        command.Parameters("@IP").Value = ip
    '        command.Parameters("@SaleAmount").Value = If(saleAmount = 0, DBNull.Value, saleAmount)
    '        command.Parameters("@SaleRefID").Value = If(String.IsNullOrEmpty(saleRefID), DBNull.Value, saleRefID)
    '        command.ExecuteNonQuery()
    '        vcode = If(IsDBNull(command.Parameters("@VCode").Value), 0, command.Parameters("@VCode").Value)
    '        verificationCode = If(IsDBNull(command.Parameters("@VerificationCode").Value), 0, command.Parameters("@VerificationCode").Value)
    '        State = command.Parameters("@State").Value
    '        LinkToken = StringToURL(cryptography.Encrypt(sPlainText:=vcode, keyType:=EncryptKeyEnum.Key1))
    '        linkCommand = ARCOConnection.Command("AZ.SetVerificationLink")
    '        linkCommand.Parameters("@IdentificationVCode").Value = vcode
    '        linkCommand.Parameters("@VerificationLink").Value = Link + LinkToken + "&printPage=false"
    '        linkCommand.ExecuteNonQuery()
    '        Return True
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If command IsNot Nothing Then
    '            command.Connection.Close()
    '            command.Connection.Dispose()
    '            command.Dispose()
    '        End If
    '    End Try
    'End Function
    'Public Function VerifyCellPhone(identificationVCode As Long, cellphone As String, verificationCode As String) As VerificationStateData
    '    Dim command As SqlCommand = Nothing
    '    Dim verificationState As New VerificationStateData
    '    Try
    '        command = ARCOConnection.Command("AZ.VerifyCellPhone")
    '        command.Parameters("@IdentificationVCode").Value = If(identificationVCode = 0, DBNull.Value, identificationVCode)
    '        command.Parameters("@VerificationCode").Value = verificationCode
    '        command.Parameters("@Cellphone").Value = If(String.IsNullOrEmpty(cellphone), DBNull.Value, cellphone)
    '        command.Parameters("@VerificationState").Direction = ParameterDirection.Output
    '        command.Parameters("@VerificationStateDescription").Direction = ParameterDirection.Output
    '        command.ExecuteNonQuery()
    '        verificationState.VerificationState = command.Parameters("@VerificationState").Value
    '        verificationState.Description = command.Parameters("@VerificationStateDescription").Value
    '        Return verificationState
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If command IsNot Nothing Then
    '            command.Connection.Close()
    '            command.Connection.Dispose()
    '            command.Dispose()
    '        End If
    '    End Try
    'End Function
    'Public Function VerifyShahkar(identificationVCode As Long, nationalCode As String, cellphone As String, hasShahkarIdentified As Boolean) As ShahkarStateData
    '    Dim command As SqlCommand = Nothing
    '    Dim shahkarState As New ShahkarStateData
    '    Try
    '        command = ARCOConnection.Command("AZ.VerifyShahkar")
    '        command.Parameters("@IdentificationVCode").Value = identificationVCode
    '        command.Parameters("@NationalCode").Value = nationalCode
    '        command.Parameters("@Cellphone").Value = cellphone
    '        command.Parameters("@HasShahkarIdentified").Value = hasShahkarIdentified
    '        command.Parameters("@ShahkarState").Direction = ParameterDirection.Output
    '        command.Parameters("@ShahkarStateDescription").Direction = ParameterDirection.Output
    '        command.ExecuteNonQuery()
    '        shahkarState.ShahkarState = command.Parameters("@ShahkarState").Value
    '        shahkarState.Description = command.Parameters("@ShahkarStateDescription").Value
    '        Return shahkarState
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If command IsNot Nothing Then
    '            command.Connection.Close()
    '            command.Connection.Dispose()
    '            command.Dispose()
    '        End If
    '    End Try
    'End Function
    'Public Function SetVerificationLink(identificationVCode As Long, verificationLink As String) As Boolean
    '    Dim command As SqlCommand = Nothing
    '    Dim shahkarState As New ShahkarStateData
    '    Try
    '        command = ARCOConnection.Command("AZ.SetVerificationLink")
    '        command.Parameters("@IdentificationVCode").Value = identificationVCode
    '        command.Parameters("@VerificationLink").Value = verificationLink
    '        command.ExecuteNonQuery()
    '        Return True
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If command IsNot Nothing Then
    '            command.Connection.Close()
    '            command.Connection.Dispose()
    '            command.Dispose()
    '        End If
    '    End Try
    'End Function
    'Public Function VerifyIndividual(identificationVCode As Long, nationalCode As String, Optional hasRejected As Boolean = False) As VerificationStateData
    '    Dim command As SqlCommand = Nothing
    '    Dim verificationState As New VerificationStateData
    '    Try
    '        command = ARCOConnection.Command("AZ.VerifyIndividual")
    '        command.Parameters("@IdentificationVCode").Value = identificationVCode
    '        command.Parameters("@NationalCode").Value = nationalCode
    '        command.Parameters("@HasRejected").Value = If(hasRejected = False, DBNull.Value, hasRejected)
    '        command.Parameters("@VerificationState").Direction = ParameterDirection.Output
    '        command.Parameters("@VerificationStateDescription").Direction = ParameterDirection.Output
    '        command.ExecuteNonQuery()
    '        verificationState.VerificationState = command.Parameters("@VerificationState").Value
    '        verificationState.Description = command.Parameters("@VerificationStateDescription").Value
    '        Return verificationState
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If command IsNot Nothing Then
    '            command.Connection.Close()
    '            command.Connection.Dispose()
    '            command.Dispose()
    '        End If
    '    End Try
    'End Function
    'Public Shared Function StringToURL(text As String) As String
    '    Return System.Web.HttpUtility.UrlEncode(text)
    '    'Return text.Replace("/", "(S)").Replace("&", "(A)").Replace("+", "(P)").Replace("=", "(E)")
    'End Function
    'Public Shared Function ConvertFromXMLToIdentificationType(xmlString As String) As IdentificationTypeData
    '    Try
    '        Dim identificationType As IdentificationTypeData
    '        Dim xml As New XmlDocument()
    '        If Not xmlString Is Nothing Then
    '            xml.LoadXml(xmlString)
    '            Dim xnList As XmlNodeList = xml.SelectNodes("/IdentificationTypes/IdentificationType")
    '            For Each xn As XmlNode In xnList
    '                identificationType = New IdentificationTypeData
    '                If xn("VCode") IsNot Nothing Then
    '                    identificationType.VCode = xn("VCode").InnerText
    '                End If
    '                If xn("Code") IsNot Nothing Then
    '                    identificationType.Code = xn("Code").InnerText
    '                End If
    '                If xn("Name") IsNot Nothing Then
    '                    identificationType.Name = xn("Name").InnerText
    '                End If
    '                If xn("EnumName") IsNot Nothing Then
    '                    identificationType.EnumName = xn("EnumName").InnerText
    '                End If
    '                Return identificationType
    '            Next
    '        End If
    '        Return Nothing
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Function
    'Public Shared Function ConvertFromXMLToIdentificationState(xmlString As String) As IdentificationStateData
    '    Try
    '        Dim identificationState As IdentificationStateData
    '        Dim xml As New XmlDocument()
    '        If Not xmlString Is Nothing Then
    '            xml.LoadXml(xmlString)
    '            Dim xnList As XmlNodeList = xml.SelectNodes("/IdentificationStates/IdentificationState")
    '            For Each xn As XmlNode In xnList
    '                identificationState = New IdentificationStateData
    '                If xn("VCode") IsNot Nothing Then
    '                    identificationState.VCode = xn("VCode").InnerText
    '                End If
    '                If xn("Code") IsNot Nothing Then
    '                    identificationState.Code = xn("Code").InnerText
    '                End If
    '                If xn("Name") IsNot Nothing Then
    '                    identificationState.Name = xn("Name").InnerText
    '                End If
    '                If xn("EnumName") IsNot Nothing Then
    '                    identificationState.EnumName = xn("EnumName").InnerText
    '                End If
    '                Return identificationState
    '            Next
    '        End If
    '        Return Nothing
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Function
    'Public Function GetUserWaitingPersonConfirmation(userVCode As Integer) As Integer
    '    Dim command As SqlCommand = Nothing
    '    Dim verificationState As New VerificationStateData
    '    Try
    '        command = ARCOConnection.Command("AZ.GetUserWaitingPersonConfirmation")
    '        command.Parameters("@UserVCode").Value = userVCode
    '        Return command.ExecuteScalar()
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If command IsNot Nothing Then
    '            command.Connection.Close()
    '            command.Connection.Dispose()
    '            command.Dispose()
    '        End If
    '    End Try
    'End Function
    'Public Function GetUserConfirmed(userVCode As Integer) As Integer
    '    Dim command As SqlCommand = Nothing
    '    Dim verificationState As New VerificationStateData
    '    Try
    '        command = ARCOConnection.Command("AZ.GetUserConfirmed")
    '        command.Parameters("@UserVCode").Value = userVCode
    '        Return command.ExecuteScalar()
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If command IsNot Nothing Then
    '            command.Connection.Close()
    '            command.Connection.Dispose()
    '            command.Dispose()
    '        End If
    '    End Try
    'End Function
    'Public Function GetUserRejected(userVCode As Integer) As Integer
    '    Dim command As SqlCommand = Nothing
    '    Dim verificationState As New VerificationStateData
    '    Try
    '        command = ARCOConnection.Command("AZ.GetUserRejected")
    '        command.Parameters("@UserVCode").Value = userVCode
    '        Return command.ExecuteScalar()
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If command IsNot Nothing Then
    '            command.Connection.Close()
    '            command.Connection.Dispose()
    '            command.Dispose()
    '        End If
    '    End Try
    'End Function
    'Private Shared Function ConvertReportTypesDataToXml(reportTypes As List(Of ReportTypeData)) As String
    '    Try
    '        Return reportTypes.Aggregate(String.Empty, Function(current, reportType) current & String.Format("<IRT RT=""{0}""/>", reportType.VCode))
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Function
    'Public Function IdentificationReportTypeIns(identificationVCode As Long, reportTypes As List(Of ReportTypeData)) As Boolean
    '    Dim command As SqlCommand = Nothing
    '    Dim verificationState As New VerificationStateData
    '    Try
    '        command = ARCOConnection.Command("AZ.IdentificationReportTypeIns")
    '        command.Parameters("@IdentificationVCode").Value = identificationVCode
    '        command.Parameters("@ReportTypes").Value = ConvertReportTypesDataToXml(reportTypes)
    '        command.ExecuteNonQuery()
    '        Return True
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If command IsNot Nothing Then
    '            command.Connection.Close()
    '            command.Connection.Dispose()
    '            command.Dispose()
    '        End If
    '    End Try
    'End Function
    'Public Function SetIdentificationAvailabilityCheck(identificationVCode As Long, availabilityCheck As Boolean) As Boolean
    '    Dim command As SqlCommand = Nothing
    '    Dim verificationState As New VerificationStateData
    '    Try
    '        command = ARCOConnection.Command("AZ.SetIdentificationAvailabilityCheck")
    '        command.Parameters("@IdentificationVCode").Value = identificationVCode
    '        command.Parameters("@AvailabilityCheck").Value = availabilityCheck
    '        command.ExecuteNonQuery()
    '        Return True
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If command IsNot Nothing Then
    '            command.Connection.Close()
    '            command.Connection.Dispose()
    '            command.Dispose()
    '        End If
    '    End Try
    'End Function
    'Public Function GetIdentificationReportTypes(Optional identificationVCode As Long = 0, Optional trackingCode As Long = 0) As List(Of ReportTypeData)
    '    Dim reportTypes As New List(Of ReportTypeData)
    '    Dim command As SqlCommand = Nothing
    '    Dim reader As SqlDataReader = Nothing
    '    Try
    '        command = ARCOConnection.Command("AZ.GetIdentificationReportTypes")
    '        command.Parameters("@IdentificationVCode").Value = identificationVCode
    '        command.Parameters("@TrackingCode").Value = trackingCode
    '        reader = command.ExecuteReader()
    '        While reader.Read()
    '            reportTypes.Add(GetReportTypeData(reader))
    '        End While
    '        Return reportTypes
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If reader IsNot Nothing Then
    '            reader.Close()
    '            reader.Dispose()
    '        End If
    '        If command IsNot Nothing Then
    '            command.Connection.Close()
    '            command.Connection.Dispose()
    '            command.Dispose()
    '        End If
    '    End Try
    'End Function
    'Public Function GetIdentificationState(identificationStateVCode As Long) As IdentificationStateData
    '    Dim identificationState As IdentificationStateData = Nothing
    '    Dim command As SqlCommand = Nothing
    '    Dim reader As SqlDataReader = Nothing
    '    Try
    '        command = ARCOConnection.Command("AZ.GetIdentificationState")
    '        command.Parameters("@IdentificationStateVCode").Value = identificationStateVCode
    '        reader = command.ExecuteReader()
    '        If reader.Read() Then
    '            identificationState = GetIdentificationStateData(reader)
    '        End If
    '        Return identificationState
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If reader IsNot Nothing Then
    '            reader.Close()
    '            reader.Dispose()
    '        End If
    '        If command IsNot Nothing Then
    '            command.Connection.Close()
    '            command.Connection.Dispose()
    '            command.Dispose()
    '        End If
    '    End Try
    'End Function
    'Public Function SetUserSeenCreditReport(identificationVCode As Long, userVCode As Integer) As Boolean
    '    Dim command As SqlCommand = Nothing
    '    Try
    '        command = ARCOConnection.Command("AZ.SetUserSeenCreditReport")
    '        command.Parameters("@IdentificationVCode").Value = identificationVCode
    '        command.Parameters("@UserVCode").Value = userVCode
    '        command.ExecuteNonQuery()
    '        Return True
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If command IsNot Nothing Then
    '            command.Connection.Close()
    '            command.Connection.Dispose()
    '            command.Dispose()
    '        End If
    '    End Try
    'End Function
    'Public Function GetBatchResultXml(identificationVCode As Long, Optional IsNewCoreConnection As Boolean = False) As BatchResultXmlData
    '    Dim batchResultXml As BatchResultXmlData = Nothing
    '    Dim command As SqlCommand = Nothing
    '    Dim reader As SqlDataReader = Nothing
    '    Try
    '        command = ICECoreConnection.Command(StoredProcedureName:="AZ.GetBatchResult", IsNewCoreConnection:=IsNewCoreConnection)
    '        command.Parameters("@IdentificationVCode").Value = identificationVCode
    '        command.CommandTimeout = 3600
    '        reader = command.ExecuteReader()
    '        If reader.Read() Then
    '            batchResultXml = GetBatchResultXmlData(reader)
    '        End If
    '        Return batchResultXml
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If reader IsNot Nothing Then
    '            reader.Close()
    '            reader.Dispose()
    '        End If
    '        If command IsNot Nothing Then
    '            command.Connection.Close()
    '            command.Connection.Dispose()
    '            command.Dispose()
    '        End If
    '    End Try
    'End Function
    'Public Function GetReportLinkAndExpirationDate(cellphone As String, nationalCode As String, UserVCode As Long, Optional receiverCellphone As String = Nothing, Optional companyNationalID As String = Nothing) As ReportLinkAndExpirationDateData
    '    Dim reportLinkAndExpirationDate As ReportLinkAndExpirationDateData = Nothing
    '    Dim command As SqlCommand = Nothing
    '    Dim reader As SqlDataReader = Nothing
    '    Try
    '        command = ARCOConnection.Command("AZ.GetReportLinkAndExpirationDate")
    '        command.Parameters("@Cellphone").Value = cellphone
    '        command.Parameters("@NationalCode").Value = nationalCode
    '        command.Parameters("@UserVCode").Value = UserVCode
    '        command.Parameters("@ReceiverCellphone").Value = If(String.IsNullOrEmpty(receiverCellphone), DBNull.Value, receiverCellphone)
    '        command.Parameters("@CompanyNationalID").Value = If(String.IsNullOrEmpty(companyNationalID), DBNull.Value, companyNationalID)
    '        reader = command.ExecuteReader()
    '        If reader.Read() Then
    '            reportLinkAndExpirationDate = GetReportLinkAndExpirationDateData(reader)
    '        End If
    '        Return reportLinkAndExpirationDate
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If reader IsNot Nothing Then
    '            reader.Close()
    '            reader.Dispose()
    '        End If
    '        If command IsNot Nothing Then
    '            command.Connection.Close()
    '            command.Connection.Dispose()
    '            command.Dispose()
    '        End If
    '    End Try
    'End Function
    'Public Function GetIdentificationLinkAndExpirationDateFirmPanel(cellphone As String, nationalCode As String, UserVCode As Long, Optional CompanyNationalID As String = Nothing) As ReportLinkAndExpirationDateData
    '    Dim reportLinkAndExpirationDate As ReportLinkAndExpirationDateData = Nothing
    '    Dim command As SqlCommand = Nothing
    '    Dim reader As SqlDataReader = Nothing
    '    Try
    '        command = ARCOConnection.Command("AZ.GetIdentificationLinkAndExpirationDateFirmPanel")
    '        command.Parameters("@Cellphone").Value = cellphone
    '        command.Parameters("@NationalCode").Value = nationalCode
    '        command.Parameters("@UserVCode").Value = UserVCode
    '        command.Parameters("@CompanyNationalID").Value = If(String.IsNullOrEmpty(CompanyNationalID), DBNull.Value, CompanyNationalID)
    '        reader = command.ExecuteReader()
    '        If reader.Read() Then
    '            reportLinkAndExpirationDate = GetReportLinkAndExpirationDateData(reader)
    '        End If
    '        Return reportLinkAndExpirationDate
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If reader IsNot Nothing Then
    '            reader.Close()
    '            reader.Dispose()
    '        End If
    '        If command IsNot Nothing Then
    '            command.Connection.Close()
    '            command.Connection.Dispose()
    '            command.Dispose()
    '        End If
    '    End Try
    'End Function
    'Public Function GetTrackingCode(identificationVCode As Long) As Long
    '    Dim command As SqlCommand = Nothing
    '    Try
    '        command = ARCOConnection.Command("AZ.GetTrackingCode")
    '        command.Parameters("@IdentificationVCode").Value = identificationVCode
    '        Return command.ExecuteScalar()
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If command IsNot Nothing Then
    '            command.Connection.Close()
    '            command.Connection.Dispose()
    '            command.Dispose()
    '        End If
    '    End Try
    'End Function
    'Public Shared Function IdentificationSendToOthersIns(identificationVCode As Long, identificationSendToOthers As List(Of IdentificationSendToOthersData)) As Boolean
    '    Dim command As SqlCommand = Nothing
    '    Try
    '        command = ARCOConnection.Command("AZ.IdentificationSendToOthersIns")
    '        command.Parameters("@IdentificationVCode").Value = identificationVCode
    '        command.Parameters("@IdentificationSendToOthers").Value = ConvertIdentificationSendToOthersDataToXml(identificationSendToOthers)
    '        command.ExecuteNonQuery()
    '        Return True
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If command IsNot Nothing Then
    '            command.Connection.Close()
    '            command.Connection.Dispose()
    '            command.Dispose()
    '        End If
    '    End Try
    'End Function
    'Public Shared Function SetIdentificationVerified(identificationVCode As Long) As Boolean
    '    Dim command As SqlCommand = Nothing
    '    Try
    '        command = ARCOConnection.Command("AZ.SetIdentificationVerified")
    '        command.Parameters("@IdentificationVCode").Value = identificationVCode
    '        command.ExecuteNonQuery()
    '        Return True
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If command IsNot Nothing Then
    '            command.Connection.Close()
    '            command.Connection.Dispose()
    '            command.Dispose()
    '        End If
    '    End Try
    'End Function
    'Private Shared Function ConvertIdentificationSendToOthersDataToXml(identificationSendToOthers As List(Of IdentificationSendToOthersData)) As String
    '    Return identificationSendToOthers.Aggregate(String.Empty, Function(current, identificationSendToOther) current & String.Format("<I ISTOT=""{0}"" R=""{1}""/>", CType(identificationSendToOther.IdentificationSendToOthersTypeVCode, Integer), identificationSendToOther.Receiver))
    'End Function
    'Public Shared Function ConvertFromXMLToIdentificationSendToOthers(xmlString As String) As List(Of IdentificationSendToOthersData)
    '    Try
    '        Dim identificationSendToOthers As New List(Of IdentificationSendToOthersData)
    '        Dim identificationSendToOther As IdentificationSendToOthersData
    '        Dim xml As New XmlDocument()
    '        If Not xmlString Is Nothing Then
    '            xml.LoadXml(xmlString)
    '            Dim xnList As XmlNodeList = xml.SelectNodes("/IdentificationSendToOthers/IdentificationSendToOther")
    '            For Each xn As XmlNode In xnList
    '                identificationSendToOther = New IdentificationSendToOthersData
    '                If xn("VCode") IsNot Nothing Then
    '                    identificationSendToOther.VCode = xn("VCode").InnerText
    '                End If
    '                If xn("IdentificationVCode") IsNot Nothing Then
    '                    identificationSendToOther.IdentificationVCode = xn("IdentificationVCode").InnerText
    '                End If
    '                If xn("IdentificationSendToOthersTypeVCode") IsNot Nothing Then
    '                    identificationSendToOther.IdentificationSendToOthersTypeVCode = xn("IdentificationSendToOthersTypeVCode").InnerText
    '                End If
    '                If xn("Receiver") IsNot Nothing Then
    '                    identificationSendToOther.Receiver = xn("Receiver").InnerText
    '                End If
    '                If xn("EntryDate") IsNot Nothing Then
    '                    identificationSendToOther.EntryDate = xn("EntryDate").InnerText
    '                End If
    '                identificationSendToOthers.Add(identificationSendToOther)
    '            Next
    '        End If
    '        Return identificationSendToOthers
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Function
    'Public Shared Function ResendVerificationCode(identificationVCode As Long) As Integer
    '    Dim command As SqlCommand = Nothing
    '    Try
    '        command = ARCOConnection.Command("AZ.ResendVerificationCode")
    '        command.Parameters("@IdentificationVCode").Value = identificationVCode
    '        command.Parameters("@VerificationCode").Direction = ParameterDirection.Output
    '        command.ExecuteNonQuery()
    '        Return If(IsDBNull(command.Parameters("@VerificationCode").Value), 0, command.Parameters("@VerificationCode").Value)
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If command IsNot Nothing Then
    '            command.Connection.Close()
    '            command.Connection.Dispose()
    '            command.Dispose()
    '        End If
    '    End Try
    'End Function
    'Public Shared Function ResendVerificationCodeSecondCellphone(identificationVCode As Long, secondCellphone As String) As Integer
    '    Dim command As SqlCommand = Nothing
    '    Try
    '        command = ARCOConnection.Command("AZ.ResendVerificationCodeSecondCellphone")
    '        command.Parameters("@IdentificationVCode").Value = identificationVCode
    '        command.Parameters("@SecondCellphone").Value = secondCellphone
    '        command.Parameters("@SecondCellphoneVerificationCode").Direction = ParameterDirection.Output
    '        command.ExecuteNonQuery()
    '        Return If(IsDBNull(command.Parameters("@SecondCellphoneVerificationCode").Value), 0, command.Parameters("@SecondCellphoneVerificationCode").Value)
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If command IsNot Nothing Then
    '            command.Connection.Close()
    '            command.Connection.Dispose()
    '            command.Dispose()
    '        End If
    '    End Try
    'End Function
    'Public Shared Function GetReportDateWithTrackingCode(trackingCode As Long) As DateTime
    '    Dim command As SqlCommand = Nothing
    '    Try
    '        command = ARCOConnection.Command("AZ.GetReportDateWithTrackingCode")
    '        command.Parameters("@TrackingCode").Value = trackingCode
    '        Return command.ExecuteScalar()
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If command IsNot Nothing Then
    '            command.Connection.Close()
    '            command.Connection.Dispose()
    '            command.Dispose()
    '        End If
    '    End Try
    'End Function
    'Public Function SetIdentificationReadyForSee(identificationVCode As Long) As Boolean
    '    Dim command As SqlCommand = Nothing
    '    Try
    '        command = ARCOConnection.Command("AZ.SetIdentificationReadyForSee")
    '        command.Parameters("@IdentificationVCode").Value = identificationVCode
    '        command.ExecuteNonQuery()
    '        Return True
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If command IsNot Nothing Then
    '            command.Connection.Close()
    '            command.Connection.Dispose()
    '            command.Dispose()
    '        End If
    '    End Try
    'End Function
    'Private Shared Function ConvertIdentificationStatesDataToXml(identificationStates As List(Of IdentificationStateData)) As String
    '    Return identificationStates.Aggregate(String.Empty, Function(current, identificationState) current & String.Format("<IS ISV=""{0}""/>", identificationState.VCode))
    'End Function
    'Public Function GetAllIdentificationStates(userVCode As Integer) As List(Of AllIdentificationStateData)
    '    Dim allIdentificationStates As New List(Of AllIdentificationStateData)
    '    Dim command As SqlCommand = Nothing
    '    Dim reader As SqlDataReader = Nothing
    '    Try
    '        command = ARCOConnection.Command("AZ.GetAllIdentificationStates")
    '        command.Parameters("@UserVCode").Value = If(userVCode = 0, DBNull.Value, userVCode)
    '        reader = command.ExecuteReader()
    '        While reader.Read()
    '            allIdentificationStates.Add(GetAllIdentificationStateData(reader))
    '        End While
    '        Return allIdentificationStates
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If reader IsNot Nothing Then
    '            reader.Close()
    '            reader.Dispose()
    '        End If
    '        If command IsNot Nothing Then
    '            command.Connection.Close()
    '            command.Connection.Dispose()
    '            command.Dispose()
    '        End If
    '    End Try
    'End Function
    'Public Function GetIdentificationsTracking(userVCode As Long, Optional fromDate As DateTime = Nothing, Optional toDate As DateTime = Nothing, Optional pageNo As Integer = 0, Optional pageSize As Integer = 0, Optional ByRef rowCount As Integer = 0) As List(Of IdentificationData)
    '    Dim identifications As New List(Of IdentificationData)
    '    Dim command As SqlCommand = Nothing
    '    Dim reader As SqlDataReader = Nothing
    '    Try
    '        command = ARCOConnection.Command("AZ.GetIdentificationsTracking")
    '        command.Parameters("@UserVCode").Value = If(userVCode = 0, DBNull.Value, userVCode)
    '        command.Parameters("@FromDate").Value = If(fromDate = DateTime.MinValue, DBNull.Value, fromDate)
    '        command.Parameters("@ToDate").Value = If(toDate = DateTime.MinValue, DBNull.Value, toDate)
    '        command.Parameters("@PageNo").Value = If(pageNo = 0, DBNull.Value, pageNo)
    '        command.Parameters("@PageSize").Value = If(pageSize = 0, DBNull.Value, pageSize)
    '        reader = command.ExecuteReader()
    '        While reader.Read()
    '            identifications.Add(GetIdentificationData(reader))
    '            rowCount = reader("RowCount")
    '        End While
    '        Return identifications
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If reader IsNot Nothing Then
    '            reader.Close()
    '            reader.Dispose()
    '        End If
    '        If command IsNot Nothing Then
    '            command.Connection.Close()
    '            command.Connection.Dispose()
    '            command.Dispose()
    '        End If
    '    End Try
    'End Function
    'Public Function VerifySecondCellPhone(identificationVCode As Long, secondCellphone As String, secondCellphoneVerificationCode As String) As VerificationStateData
    '    Dim command As SqlCommand = Nothing
    '    Dim verificationState As New VerificationStateData
    '    Try
    '        command = ARCOConnection.Command("AZ.VerifySecondCellPhone")
    '        command.Parameters("@IdentificationVCode").Value = If(identificationVCode = 0, DBNull.Value, identificationVCode)
    '        command.Parameters("@SecondCellphoneVerificationCode").Value = secondCellphoneVerificationCode
    '        command.Parameters("@SecondCellphone").Value = If(String.IsNullOrEmpty(secondCellphone), DBNull.Value, secondCellphone)
    '        command.Parameters("@VerificationState").Direction = ParameterDirection.Output
    '        command.Parameters("@VerificationStateDescription").Direction = ParameterDirection.Output
    '        command.ExecuteNonQuery()
    '        verificationState.VerificationState = command.Parameters("@VerificationState").Value
    '        verificationState.Description = command.Parameters("@VerificationStateDescription").Value
    '        Return verificationState
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If command IsNot Nothing Then
    '            command.Connection.Close()
    '            command.Connection.Dispose()
    '            command.Dispose()
    '        End If
    '    End Try
    'End Function
    'Public Function IdentificationUserPaymentTypeIns(identificationVCode As Long, userPaymentTypeVCode As UserPaymentTypeEnum) As IdentificationUserPaymentTypeInsStateData
    '    Dim command As SqlCommand = Nothing
    '    Dim IdentificationUserPaymentTypeInsState As New IdentificationUserPaymentTypeInsStateData
    '    Try
    '        command = ARCOConnection.Command("AZ.IdentificationUserPaymentTypeIns")
    '        command.Parameters("@IdentificationVCode").Value = If(identificationVCode = 0, DBNull.Value, identificationVCode)
    '        command.Parameters("@UserPaymentTypeVCode").Value = userPaymentTypeVCode
    '        command.Parameters("@IdentificationUserPaymentTypeInsState").Direction = ParameterDirection.Output
    '        command.Parameters("@IdentificationUserPaymentTypeInsStateDescription").Direction = ParameterDirection.Output
    '        command.ExecuteNonQuery()
    '        IdentificationUserPaymentTypeInsState.IdentificationUserPaymentTypeInsState = command.Parameters("@IdentificationUserPaymentTypeInsState").Value
    '        IdentificationUserPaymentTypeInsState.Description = command.Parameters("@IdentificationUserPaymentTypeInsStateDescription").Value
    '        Return IdentificationUserPaymentTypeInsState
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If command IsNot Nothing Then
    '            command.Connection.Close()
    '            command.Connection.Dispose()
    '            command.Dispose()
    '        End If
    '    End Try
    'End Function
    'Public Function VerifyShahkarSecondCellphone(identificationVCode As Long, nationalCode As String, secondCellphone As String, hasShahkarIdentified As Boolean) As ShahkarStateData
    '    Dim command As SqlCommand = Nothing
    '    Dim shahkarState As New ShahkarStateData
    '    Try
    '        command = ARCOConnection.Command("AZ.VerifyShahkarSecondCellphone")
    '        command.Parameters("@IdentificationVCode").Value = identificationVCode
    '        command.Parameters("@NationalCode").Value = nationalCode
    '        command.Parameters("@SecondCellphone").Value = secondCellphone
    '        command.Parameters("@HasShahkarIdentified").Value = hasShahkarIdentified
    '        command.Parameters("@ShahkarState").Direction = ParameterDirection.Output
    '        command.Parameters("@ShahkarStateDescription").Direction = ParameterDirection.Output
    '        command.ExecuteNonQuery()
    '        shahkarState.ShahkarState = command.Parameters("@ShahkarState").Value
    '        shahkarState.Description = command.Parameters("@ShahkarStateDescription").Value
    '        Return shahkarState
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If command IsNot Nothing Then
    '            command.Connection.Close()
    '            command.Connection.Dispose()
    '            command.Dispose()
    '        End If
    '    End Try
    'End Function
    'Public Shared Function ResendVerificationCodeReceiverCellphone(identificationVCode As Long, receiverCellphone As String) As Integer
    '    Dim command As SqlCommand = Nothing
    '    Try
    '        command = ARCOConnection.Command("AZ.ResendVerificationCodeReceiverCellphone")
    '        command.Parameters("@IdentificationVCode").Value = identificationVCode
    '        command.Parameters("@ReceiverCellphone").Value = receiverCellphone
    '        command.Parameters("@ReceiverCellphoneVerificationCode").Direction = ParameterDirection.Output
    '        command.ExecuteNonQuery()
    '        Return If(IsDBNull(command.Parameters("@ReceiverCellphoneVerificationCode").Value), 0, command.Parameters("@ReceiverCellphoneVerificationCode").Value)
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If command IsNot Nothing Then
    '            command.Connection.Close()
    '            command.Connection.Dispose()
    '            command.Dispose()
    '        End If
    '    End Try
    'End Function
    'Public Function VerifyReceiverCellPhone(identificationVCode As Long, receiverCellphone As String, receiverCellphoneVerificationCode As String) As VerificationStateData
    '    Dim command As SqlCommand = Nothing
    '    Dim verificationState As New VerificationStateData
    '    Try
    '        command = ARCOConnection.Command("AZ.VerifyReceiverCellPhone")
    '        command.Parameters("@IdentificationVCode").Value = If(identificationVCode = 0, DBNull.Value, identificationVCode)
    '        command.Parameters("@ReceiverCellphoneVerificationCode").Value = receiverCellphoneVerificationCode
    '        command.Parameters("@ReceiverCellphone").Value = If(String.IsNullOrEmpty(receiverCellphone), DBNull.Value, receiverCellphone)
    '        command.Parameters("@VerificationState").Direction = ParameterDirection.Output
    '        command.Parameters("@VerificationStateDescription").Direction = ParameterDirection.Output
    '        command.ExecuteNonQuery()
    '        verificationState.VerificationState = command.Parameters("@VerificationState").Value
    '        verificationState.Description = command.Parameters("@VerificationStateDescription").Value
    '        Return verificationState
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If command IsNot Nothing Then
    '            command.Connection.Close()
    '            command.Connection.Dispose()
    '            command.Dispose()
    '        End If
    '    End Try
    'End Function
    'Public Shared Function SetIdentificationNotPaid(identificationVCode As Long) As Boolean
    '    Dim command As SqlCommand = Nothing
    '    Try
    '        command = ARCOConnection.Command("AZ.SetIdentificationNotPaid")
    '        command.Parameters("@IdentificationVCode").Value = identificationVCode
    '        command.ExecuteNonQuery()
    '        Return True
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If command IsNot Nothing Then
    '            command.Connection.Close()
    '            command.Connection.Dispose()
    '            command.Dispose()
    '        End If
    '    End Try
    'End Function
    'Public Function AddSendSmsTryCount(identificationVCode As Long) As Boolean
    '    Dim command As SqlCommand = Nothing
    '    Try
    '        command = ARCOConnection.Command("AZ.AddSendSmsTryCount")
    '        command.Parameters("@IdentificationVCode").Value = identificationVCode
    '        command.ExecuteNonQuery()
    '        Return True
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If command IsNot Nothing Then
    '            command.Connection.Close()
    '            command.Connection.Dispose()
    '            command.Dispose()
    '        End If
    '    End Try
    'End Function
    'Public Function UpdateReportExpirationDate(VCode As Integer, Optional Hour As Integer = 24) As Boolean
    '    Dim command As SqlCommand = Nothing
    '    Try
    '        command = ARCOConnection.Command("AZ.ReportExpirationDateUpd")
    '        command.Parameters("@VCode").Value = VCode
    '        command.Parameters("@Hour").Value = Hour
    '        command.ExecuteNonQuery()
    '        Return True
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If command IsNot Nothing Then
    '            command.Connection.Close()
    '            command.Connection.Dispose()
    '            command.Dispose()
    '        End If
    '    End Try
    'End Function
    'Public Function CancelIdentifiction(identificationVCode As Long, userVCode As Long) As Boolean
    '    Dim command As SqlCommand = Nothing
    '    Try
    '        command = ARCOConnection.Command("AZ.CancelIdentifiction")
    '        command.Parameters("@IdentificationVCode").Value = identificationVCode
    '        command.Parameters("@UserVCode").Value = userVCode
    '        command.ExecuteNonQuery()
    '        Return True
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If command IsNot Nothing Then
    '            command.Connection.Close()
    '            command.Connection.Dispose()
    '            command.Dispose()
    '        End If
    '    End Try
    'End Function
    'Public Function SetIdentificationICS24(identificationVCode As Long, redirectUrlICS24 As String, idICS24 As String) As Boolean
    '    Dim command As SqlCommand = Nothing
    '    Try
    '        command = ARCOConnection.Command("AZ.SetIdentificationICS24")
    '        command.Parameters("@IdentificationVCode").Value = identificationVCode
    '        command.Parameters("@RedirectUrlICS24").Value = redirectUrlICS24
    '        command.Parameters("@IdICS24").Value = idICS24
    '        command.ExecuteNonQuery()
    '        Return True
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If command IsNot Nothing Then
    '            command.Connection.Close()
    '            command.Connection.Dispose()
    '            command.Dispose()
    '        End If
    '    End Try
    'End Function
    'Public Function SetIdentificationAppICS24HashCode(identificationVCode As Long, appIcs24HashCode As String) As Boolean
    '    Dim command As SqlCommand = Nothing
    '    Try
    '        command = ARCOConnection.Command("AZ.SetIdentificationAppICS24HashCode")
    '        command.Parameters("@IdentificationVCode").Value = identificationVCode
    '        command.Parameters("@AppIcs24HashCode").Value = appIcs24HashCode
    '        command.ExecuteNonQuery()
    '        Return True
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If command IsNot Nothing Then
    '            command.Connection.Close()
    '            command.Connection.Dispose()
    '            command.Dispose()
    '        End If
    '    End Try
    'End Function
    'Public Function GetIdentificationsICS24() As List(Of IdentificationData)
    '    Dim identifications As New List(Of IdentificationData)
    '    Dim command As SqlCommand = Nothing
    '    Dim reader As SqlDataReader = Nothing
    '    Try
    '        command = ARCOConnection.Command("AZ.GetIdentificationsICS24")
    '        reader = command.ExecuteReader()
    '        While reader.Read()
    '            identifications.Add(GetIdentificationData(reader))
    '        End While
    '        Return identifications
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If reader IsNot Nothing Then
    '            reader.Close()
    '            reader.Dispose()
    '        End If
    '        If command IsNot Nothing Then
    '            command.Connection.Close()
    '            command.Connection.Dispose()
    '            command.Dispose()
    '        End If
    '    End Try
    'End Function
    'Public Function GetIdentificationsPartnerSubmit() As List(Of IdentificationData)
    '    Dim identifications As New List(Of IdentificationData)
    '    Dim command As SqlCommand = Nothing
    '    Dim reader As SqlDataReader = Nothing
    '    Try
    '        command = ARCOConnection.Command("AZ.GetIdentificationsPartnerSubmit")
    '        reader = command.ExecuteReader()
    '        While reader.Read()
    '            identifications.Add(GetIdentificationData(reader))
    '        End While
    '        Return identifications
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If reader IsNot Nothing Then
    '            reader.Close()
    '            reader.Dispose()
    '        End If
    '        If command IsNot Nothing Then
    '            command.Connection.Close()
    '            command.Connection.Dispose()
    '            command.Dispose()
    '        End If
    '    End Try
    'End Function
    'Public Function IdentificationStateUpdICS24(identificationVCode As Long, identificationStateVCode As Integer, shahkarState As Boolean, isLegalaPersonState As Boolean) As Boolean
    '    Dim command As SqlCommand = Nothing
    '    Try
    '        command = ARCOConnection.Command("AZ.IdentificationStateUpdICS24")
    '        command.Parameters("@IdentificationVCode").Value = identificationVCode
    '        command.Parameters("@IdentificationStateVCode").Value = identificationStateVCode
    '        command.Parameters("@ShahkarState").Value = shahkarState
    '        command.Parameters("@IsLegalPersonState").Value = isLegalaPersonState
    '        command.ExecuteNonQuery()
    '        Return True
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If command IsNot Nothing Then
    '            command.Connection.Close()
    '            command.Connection.Dispose()
    '            command.Dispose()
    '        End If
    '    End Try
    'End Function
    'Public Function IdentificationSetStatePayed(identificationVCode As Long) As Boolean
    '    Dim command As SqlCommand = Nothing
    '    Try
    '        command = ARCOConnection.Command("AZ.IdentificationSetStatePayed")
    '        command.Parameters("@IdentificationVCode").Value = identificationVCode
    '        command.ExecuteNonQuery()
    '        Return True
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If command IsNot Nothing Then
    '            command.Connection.Close()
    '            command.Connection.Dispose()
    '            command.Dispose()
    '        End If
    '    End Try
    'End Function
    'Public Function GetMostRepairedIdentifications() As List(Of IdentificationData)
    '    Dim identifications As New List(Of IdentificationData)
    '    Dim command As SqlCommand = Nothing
    '    Dim reader As SqlDataReader = Nothing
    '    Try
    '        command = ARCOConnection.Command("AZ.GetMostRepairedIdentifications")
    '        reader = command.ExecuteReader()
    '        While reader.Read()
    '            identifications.Add(GetIdentificationData(reader))
    '        End While
    '        Return identifications
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If reader IsNot Nothing Then
    '            reader.Close()
    '            reader.Dispose()
    '        End If
    '        If command IsNot Nothing Then
    '            command.Connection.Close()
    '            command.Connection.Dispose()
    '            command.Dispose()
    '        End If
    '    End Try
    'End Function
    'Public Function GetIdentificationSentRepairMessage() As List(Of IdentificationData)
    '    Dim identifications As New List(Of IdentificationData)
    '    Dim command As SqlCommand = Nothing
    '    Dim reader As SqlDataReader = Nothing
    '    Try
    '        command = ARCOConnection.Command("AZ.GetIdentificationSentRepairMessage")
    '        reader = command.ExecuteReader()
    '        While reader.Read()
    '            identifications.Add(GetIdentificationData(reader))
    '        End While
    '        Return identifications
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If reader IsNot Nothing Then
    '            reader.Close()
    '            reader.Dispose()
    '        End If
    '        If command IsNot Nothing Then
    '            command.Connection.Close()
    '            command.Connection.Dispose()
    '            command.Dispose()
    '        End If
    '    End Try
    'End Function
    'Public Function GetMostSendSMSPeygiriIdentification() As List(Of IdentificationData)
    '    Dim identifications As New List(Of IdentificationData)
    '    Dim command As SqlCommand = Nothing
    '    Dim reader As SqlDataReader = Nothing
    '    Try
    '        command = ARCOConnection.Command("AZ.GetMostSendSMSPeygiriIdentification")
    '        reader = command.ExecuteReader()
    '        While reader.Read()
    '            identifications.Add(GetIdentificationData(reader))
    '        End While
    '        Return identifications
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If reader IsNot Nothing Then
    '            reader.Close()
    '            reader.Dispose()
    '        End If
    '        If command IsNot Nothing Then
    '            command.Connection.Close()
    '            command.Connection.Dispose()
    '            command.Dispose()
    '        End If
    '    End Try
    'End Function
    'Public Function IdentificationReportLinkUpd(identificationVCode As Long, reportLink As String) As Boolean
    '    Dim command As SqlCommand = Nothing
    '    Try
    '        command = ARCOConnection.Command("AZ.IdentificationReportLinkUpd")
    '        command.Parameters("@IdentificationVCode").Value = identificationVCode
    '        command.Parameters("@IdentificationStateVCode").Value = reportLink
    '        command.ExecuteNonQuery()
    '        Return True
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If command IsNot Nothing Then
    '            command.Connection.Close()
    '            command.Connection.Dispose()
    '            command.Dispose()
    '        End If
    '    End Try
    'End Function
    'Public Function BatchCompleteIns(batch As NewBatchData, batchResult As NewBatchResultData, Optional IsNewCoreConnection As Boolean = False) As Boolean
    '    Dim command As SqlCommand = Nothing
    '    Try
    '        command = ICECoreConnection.Command(StoredProcedureName:="AZ.BatchCompleteIns", IsNewCoreConnection:=IsNewCoreConnection)
    '        command.Parameters("@UserVCode").Value = batch.UserVCode
    '        command.Parameters("@Cellphone").Value = batch.Cellphone
    '        command.Parameters("@NationalCode").Value = batch.NationalCode
    '        command.Parameters("@CompanyNationalID").Value = If(batch.CompanyNationalID = "0", DBNull.Value, batch.CompanyNationalID)
    '        command.Parameters("@IdentificationVCode").Value = batch.IdentificationVCode
    '        command.Parameters("@IdentificationTypeVCode").Value = batch.IdentificationTypeVCode
    '        command.Parameters("@ReportXml").Value = If(batchResult.ResponseXml Is Nothing, DBNull.Value, batchResult.ResponseXml)
    '        command.Parameters("@ScoreXml").Value = If(batchResult.ResponseXmlScore Is Nothing, DBNull.Value, batchResult.ResponseXmlScore)
    '        command.Parameters("@AvailabilityXml").Value = If(batchResult.ResponseXmlEmpty Is Nothing, DBNull.Value, batchResult.ResponseXmlEmpty)
    '        command.Parameters("@ResponseJson").Value = If(batchResult.ResponseJson Is Nothing, DBNull.Value, batchResult.ResponseJson)
    '        command.Parameters("@HasShahkarIdentified").Value = batchResult.HasShahkarIdentified
    '        command.Parameters("@IsLegalPerson").Value = batchResult.IsLegalPerson
    '        command.ExecuteNonQuery()
    '        'batchResult.VCode = command.Parameters("@VCode").Value
    '        Return True
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If command IsNot Nothing Then
    '            command.Connection.Close()
    '            command.Connection.Dispose()
    '            command.Dispose()
    '        End If
    '    End Try
    'End Function
    'Public Function SetIdentificationIsLegalPerson(identificationVCode As Long, isLegalPerson As Boolean) As Boolean
    '    Dim command As SqlCommand = Nothing
    '    Try
    '        command = ARCOConnection.Command("AZ.SetIdentificationIsLegalPerson")
    '        command.Parameters("@IdentificationVCode").Value = identificationVCode
    '        command.Parameters("@IsLegalPerson").Value = If(isLegalPerson, 1, 0)
    '        command.ExecuteNonQuery()
    '        Return True
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If command IsNot Nothing Then
    '            command.Connection.Close()
    '            command.Connection.Dispose()
    '            command.Dispose()
    '        End If
    '    End Try
    'End Function
    'Public Function SetIdentificationHasShahkarIdentified(identificationVCode As Long, hasShahkarIdentified As Boolean) As Boolean
    '    Dim command As SqlCommand = Nothing
    '    Try
    '        command = ARCOConnection.Command("AZ.SetIdentificationHasShahkarIdentified")
    '        command.Parameters("@IdentificationVCode").Value = identificationVCode
    '        command.Parameters("@HasShahkarIdentified").Value = If(hasShahkarIdentified, 1, 0)
    '        command.ExecuteNonQuery()
    '        Return True
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If command IsNot Nothing Then
    '            command.Connection.Close()
    '            command.Connection.Dispose()
    '            command.Dispose()
    '        End If
    '    End Try
    'End Function
    'Public Function SetIdentificationICS24Confirmed(identificationVCode As Long) As Boolean
    '    Dim command As SqlCommand = Nothing
    '    Try
    '        command = ARCOConnection.Command("AZ.SetIdentificationICS24Confirmed")
    '        command.Parameters("@IdentificationVCode").Value = identificationVCode
    '        command.ExecuteNonQuery()
    '        Return True
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If command IsNot Nothing Then
    '            command.Connection.Close()
    '            command.Connection.Dispose()
    '            command.Dispose()
    '        End If
    '    End Try
    'End Function
    'Public Function SetIdentificationOtpLock(identificationVCode As Long) As Boolean
    '    Dim command As SqlCommand = Nothing
    '    Try
    '        command = ARCOConnection.Command("AZ.SetIdentificationOtpLock")
    '        command.Parameters("@IdentificationVCode").Value = identificationVCode
    '        command.ExecuteNonQuery()
    '        Return True
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If command IsNot Nothing Then
    '            command.Connection.Close()
    '            command.Connection.Dispose()
    '            command.Dispose()
    '        End If
    '    End Try
    'End Function
    'Private Shared Function ConvertIdentificationComplaintStatesDataToXml(complaintStates As List(Of IdentificationComplaintStateEnum)) As String
    '    Try
    '        Return complaintStates.Aggregate(String.Empty, Function(current, complaintState) current & String.Format("<ICSV S=""{0}""/>", CType(complaintState, Integer)))
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Function
    'Public Function GetUserLastSevenDaysSeen(userVCode As Long) As List(Of UserLastSevenDaysSeenData)
    '    Dim userLastSevenDaysSeens As New List(Of UserLastSevenDaysSeenData)
    '    Dim command As SqlCommand = Nothing
    '    Dim reader As SqlDataReader = Nothing
    '    Try
    '        command = ARCOConnection.Command("AZ.GetUserLastSevenDaysSeen")
    '        command.Parameters("@UserVCode").Value = userVCode
    '        reader = command.ExecuteReader()
    '        While reader.Read()
    '            userLastSevenDaysSeens.Add(GetUserLastSevenDaysSeenData(reader))
    '        End While
    '        Return userLastSevenDaysSeens
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If reader IsNot Nothing Then
    '            reader.Close()
    '            reader.Dispose()
    '        End If
    '        If command IsNot Nothing Then
    '            command.Connection.Close()
    '            command.Connection.Dispose()
    '            command.Dispose()
    '        End If
    '    End Try
    'End Function
    'Public Function GetUserLastSevenHours(userVCode As Long) As List(Of UserLastSevenHoursData)
    '    Dim userLastSevenHours As New List(Of UserLastSevenHoursData)
    '    Dim command As SqlCommand = Nothing
    '    Dim reader As SqlDataReader = Nothing
    '    Try
    '        command = ARCOConnection.Command("AZ.GetUserLastSevenHours")
    '        command.Parameters("@UserVCode").Value = userVCode
    '        reader = command.ExecuteReader()
    '        While reader.Read()
    '            userLastSevenHours.Add(GetUserLastSevenHoursData(reader))
    '        End While
    '        Return userLastSevenHours
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If reader IsNot Nothing Then
    '            reader.Close()
    '            reader.Dispose()
    '        End If
    '        If command IsNot Nothing Then
    '            command.Connection.Close()
    '            command.Connection.Dispose()
    '            command.Dispose()
    '        End If
    '    End Try
    'End Function
    'Public Function GetUserDashboard(userVCode As Long) As UserDashboardData
    '    Dim userDashboard As New UserDashboardData
    '    Try
    '        userDashboard.ConfirmedCount = GetUserConfirmed(userVCode:=userVCode)
    '        userDashboard.RejectedCount = GetUserRejected(userVCode:=userVCode)
    '        userDashboard.WaitingPersonConfirmationCount = GetUserWaitingPersonConfirmation(userVCode:=userVCode)
    '        userDashboard.UserLastSevenDaysSeens = GetUserLastSevenDaysSeen(userVCode:=userVCode)
    '        userDashboard.UserLastSevenHours = GetUserLastSevenHours(userVCode:=userVCode)
    '        Return userDashboard
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '    End Try
    'End Function
    'Public Function UpdateExpirationDate(VCode As Long, expirationDate As DateTime) As Boolean
    '    Dim command As SqlCommand = Nothing
    '    Try
    '        command = ARCOConnection.Command("AZ.ExpirationDateUpd")
    '        command.Parameters("@VCode").Value = VCode
    '        command.Parameters("@ExpirationDate").Value = expirationDate
    '        command.ExecuteNonQuery()
    '        Return True
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If command IsNot Nothing Then
    '            command.Connection.Close()
    '            command.Connection.Dispose()
    '            command.Dispose()
    '        End If
    '    End Try
    'End Function
    'Public Function IdentificationRateUpd(identificationVCode As Long, rate As Decimal) As Boolean
    '    Dim command As SqlCommand = Nothing
    '    Try
    '        command = ARCOConnection.Command("AZ.IdentificationRateUpd")
    '        command.Parameters("@IdentificationVCode").Value = identificationVCode
    '        command.Parameters("@Rate").Value = rate
    '        command.ExecuteNonQuery()
    '        Return True
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If command IsNot Nothing Then
    '            command.Connection.Close()
    '            command.Connection.Dispose()
    '            command.Dispose()
    '        End If
    '    End Try
    'End Function
    'Public Function GetIdentificationReportIsCorrupted() As List(Of IdentificationData)
    '    Dim identifications As New List(Of IdentificationData)
    '    Dim command As SqlCommand = Nothing
    '    Dim reader As SqlDataReader = Nothing
    '    Try
    '        command = ARCOConnection.Command("AZ.GetIdentificationReportIsCorrupted")
    '        reader = command.ExecuteReader()
    '        While reader.Read()
    '            identifications.Add(GetIdentificationData(reader))
    '        End While
    '        Return identifications
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If reader IsNot Nothing Then
    '            reader.Close()
    '            reader.Dispose()
    '        End If
    '        If command IsNot Nothing Then
    '            command.Connection.Close()
    '            command.Connection.Dispose()
    '            command.Dispose()
    '        End If
    '    End Try
    'End Function
End Class
