Imports System.Data.SqlClient
Imports System.Xml
Imports ARCO.Business.Models
Imports ARCO.Entities.Enums
Imports ARCO.Entities.Models
Imports ARCO.Entities.Modules

Public Class ContentBusiness
    Private Shared Function GetPagesEntity(reader As IDataRecord) As PagesEntity
        Try
            '.Rate = If(IsDBNull(reader("Rate")), 0, reader("Rate")),
            '.RateCount = If(IsDBNull(reader("RateCount")), 0, reader("RateCount")),
            Dim pages = New PagesEntity()
            pages.Id = If(IsDBNull(reader("Id")), 0, reader("Id"))
            pages.Name = If(IsDBNull(reader("Name")), Nothing, reader("Name"))
            pages.PagesType = If(IsDBNull(reader("PagesTypeId")), Nothing, reader("PagesTypeId"))
            pages.Title = If(IsDBNull(reader("Title")), 0, reader("Title")).ToString.WebUnify()
            pages.Description = If(IsDBNull(reader("Description")), Nothing, reader("Description"))
            pages.Body = If(IsDBNull(reader("Body")), Nothing, reader("Body").ToString.WebUnify())
            pages.Image = If(IsDBNull(reader("Image")), Nothing, reader("Image"))
            pages.Keywords = If(IsDBNull(reader("Keywords")), Nothing, reader("Keywords"))
            pages.PagesComments = ConvertFromXMLToPagesComments(If(IsDBNull(reader("PagesComments")), Nothing, reader("PagesComments")))
            pages.EntryDate = If(IsDBNull(reader("EntryDate")), Nothing, reader("EntryDate"))
            Return pages
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Private Shared Function GetTotalPagesRateData(reader As IDataRecord) As TotalPagesRateData
        Try
            Dim totalPagesRate = New TotalPagesRateData With
                {
                    .Cnt = If(IsDBNull(reader("Cnt")), 0, reader("Cnt")),
                    .Rate = If(IsDBNull(reader("Rate")), 0, reader("Rate"))
                }
            Return totalPagesRate
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Private Shared Function GetFAQData(reader As IDataRecord) As FAQData
        Try
            Dim pages = New FAQData With
                {
                    .VCode = If(IsDBNull(reader("VCode")), 0, reader("VCode")),
                    .Answer = If(IsDBNull(reader("Answer")), Nothing, reader("Answer")),
                    .Question = If(IsDBNull(reader("Question")), Nothing, reader("Question")),
                    .Link = If(IsDBNull(reader("Link")), 0, reader("Link")).ToString.WebUnify(),
                    .ImageLink = If(IsDBNull(reader("ImageLink")), Nothing, reader("ImageLink")),
                    .TopicVCode = If(IsDBNull(reader("TopicVCode")), Nothing, reader("TopicVCode")),
                    .EntryDate = If(IsDBNull(reader("EntryDate")), Nothing, reader("EntryDate")),
                    .LastModifiedDate = If(IsDBNull(reader("LastModifiedDate")), Nothing, reader("LastModifiedDate")),
                    .IsDeleted = If(IsDBNull(reader("IsDeleted")), False, CType(reader("IsDeleted"), Boolean))
                }
            Return pages
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Private Shared Function GetPagesCommentEntity(reader As IDataRecord) As PagesCommentEntity
        Try
            Dim pagesComment = New PagesCommentEntity With
                {
                    .VCode = If(IsDBNull(reader("VCode")), 0, reader("VCode")),
                    .ParentVCode = If(IsDBNull(reader("ParentVCode")), 0, reader("ParentVCode")),
                    .PagesVCode = If(IsDBNull(reader("PagesVCode")), 0, reader("PagesVCode")),
                    .Name = If(IsDBNull(reader("Name")), Nothing, reader("Name")).ToString.WebUnify(),
                    .Message = If(IsDBNull(reader("Message")), Nothing, reader("Message")).ToString.WebUnify(),
                    .Email = If(IsDBNull(reader("Email")), Nothing, reader("Email")),
                    .ApprovalStateVCode = If(IsDBNull(reader("ApprovalStateVCode")), ApprovalStateEnum.NONE, reader("ApprovalStateVCode")),
                    .Website = If(IsDBNull(reader("Website")), Nothing, reader("Website")),
                    .Cellphone = If(IsDBNull(reader("Cellphone")), Nothing, reader("Cellphone")),
                    .EntryDate = If(IsDBNull(reader("EntryDate")), Nothing, reader("EntryDate")),
                    .LastModifiedDate = If(IsDBNull(reader("LastModifiedDate")), Nothing, reader("LastModifiedDate"))
                }
            Return pagesComment
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetPage(Optional id As Guid? = Nothing, Optional link As String = Nothing, Optional pageTypeVCode As PageTypeEnum = PageTypeEnum.BLOG, Optional lastCommentedPage As Boolean = False) As PagesEntity
        Dim page As PagesEntity = Nothing
        Dim command As SqlCommand = Nothing
        Dim reader As SqlDataReader = Nothing
        Try
            command = ARCOConnection.Command("AZ.GetPage")
            command.Parameters("@Id").Value = id
            command.Parameters("@Link").Value = If(String.IsNullOrEmpty(link), DBNull.Value, link)
            command.Parameters("@PageTypeVCode").Value = If(String.IsNullOrEmpty(link), DBNull.Value, pageTypeVCode)
            command.Parameters("@LastCommentedPage").Value = lastCommentedPage
            reader = command.ExecuteReader()
            If reader.Read() Then
                page = GetPagesEntity(reader)
            End If
            Return page
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
    Public Function GetPages(Optional pageNo As Integer = 0, Optional pageSize As Integer = 0, Optional ByRef rowCount As Integer = 0, Optional pageType As Integer = 1) As List(Of PagesEntity)
        Dim pages As New List(Of PagesEntity)
        Dim command As SqlCommand = Nothing
        Dim reader As SqlDataReader = Nothing
        Try
            command = ARCOConnection.Command("AZ.GetPages")
            If pageSize > -1 AndAlso pageNo > -1 Then
                command.Parameters("@PageNo").Value = If(pageNo = 0, DBNull.Value, pageNo)
                command.Parameters("@PageSize").Value = If(pageSize = 0, DBNull.Value, pageSize)
                command.Parameters("@PageTypeId").Value = If(pageType = 0, DBNull.Value, pageType)
            End If
            reader = command.ExecuteReader()
            While reader.Read()
                pages.Add(GetPagesEntity(reader))
                rowCount = reader("RowCount")
            End While
            Return pages
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
    Public Function PagesIns(Optional name As String = Nothing, Optional pagesTypeId As Integer = 1, Optional title As String = Nothing, Optional image As String = Nothing, Optional description As String = Nothing, Optional body As String = Nothing, Optional keywords As String = Nothing) As PagesEntity
        Dim item As New PagesEntity
        Dim command As SqlCommand = Nothing
        Dim reader As SqlDataReader = Nothing
        Try
            command = ARCOConnection.Command("AZ.PagesIns")
            command.Parameters("@Name").Value = If(String.IsNullOrEmpty(name), DBNull.Value, name)
            command.Parameters("@pagesTypeId").Value = pagesTypeId
            command.Parameters("@Title").Value = If(String.IsNullOrEmpty(title), DBNull.Value, title)
            command.Parameters("@Image").Value = If(String.IsNullOrEmpty(image), DBNull.Value, image)
            command.Parameters("@Description").Value = If(String.IsNullOrEmpty(description), DBNull.Value, description)
            command.Parameters("@Body").Value = If(String.IsNullOrEmpty(body), DBNull.Value, body)
            command.Parameters("@Keywords").Value = If(String.IsNullOrEmpty(keywords), DBNull.Value, keywords)
            reader = command.ExecuteReader()
            If reader.HasRows Then
                reader.Read()
                item = GetPagesEntity(reader)
            End If
            Return item
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
    Public Function GetFAQs(Optional pageNo As Integer = 0, Optional pageSize As Integer = 0, Optional ByRef rowCount As Integer = 0) As List(Of FAQData)
        Dim faqs As New List(Of FAQData)
        Dim command As SqlCommand = Nothing
        Dim reader As SqlDataReader = Nothing
        Try
            command = ARCOConnection.Command("AZ.GetFAQs")
            If pageSize > -1 AndAlso pageNo > -1 Then
                command.Parameters("@PageNo").Value = If(pageNo = 0, DBNull.Value, pageNo)
                command.Parameters("@PageSize").Value = If(pageSize = 0, DBNull.Value, pageSize)
            End If
            reader = command.ExecuteReader()
            While reader.Read()
                faqs.Add(GetFAQData(reader))
                rowCount = reader("RowCount")
            End While
            Return faqs
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
    Public Function GetFrontPageCount(Optional pageType As PageTypeEnum = PageTypeEnum.NONE) As Integer
        Dim frontPageCount As Integer
        Dim command As SqlCommand = Nothing
        Dim reader As SqlDataReader = Nothing
        Try
            command = ARCOConnection.Command("AZ.GetFrontPageCount")
            command.Parameters("@PageTypeVCode").Value = If(pageType = PageTypeEnum.NONE, DBNull.Value, pageType)
            reader = command.ExecuteReader()
            If reader.Read() Then
                frontPageCount = reader("frontPageCount")
            End If
            Return frontPageCount
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
    Public Shared Function ConvertFromXMLToPagesImages(xmlString As String) As List(Of PagesImageData)
        Try
            Dim PagesImages As New List(Of PagesImageData)
            Dim PagesImage As PagesImageData
            Dim xml As New XmlDocument()
            If Not xmlString Is Nothing Then
                xml.LoadXml(xmlString)
                Dim xnList As XmlNodeList = xml.SelectNodes("/PagesImages/PagesImage")
                For Each xn As XmlNode In xnList
                    PagesImage = New PagesImageData
                    If xn("VCode") IsNot Nothing Then
                        PagesImage.ID = xn("VCode").InnerText
                    End If
                    If xn("Name") IsNot Nothing Then
                        PagesImage.Name = xn("Name").InnerText
                    End If
                    If xn("EntryDate") IsNot Nothing Then
                        PagesImage.EntryDate = xn("EntryDate").InnerText
                    End If
                    If xn("LastModifiedDate") IsNot Nothing Then
                        PagesImage.LastModifiedDate = xn("LastModifiedDate").InnerText
                    End If
                    If xn("EntryUserVCode") IsNot Nothing Then
                        PagesImage.EntryUserVCode = xn("EntryUserVCode").InnerText
                    End If
                    If xn("LastModifiedUserVCode") IsNot Nothing Then
                        PagesImage.LastModifiedUserVCode = xn("LastModifiedUserVCode").InnerText
                    End If
                    PagesImages.Add(PagesImage)
                Next
            End If
            Return PagesImages
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function PagesCommentIns(ByRef vcode As Long, pagesVCode As Long, name As String, message As String, Optional email As String = Nothing, Optional website As String = Nothing, Optional identificationVCode As Long = 0, Optional parentVCode As Long = 0) As Boolean
        Dim command As SqlCommand = Nothing
        Try
            command = ARCOConnection.Command("AZ.PagesCommentIns")
            command.Parameters("@VCode").Direction = ParameterDirection.Output
            command.Parameters("@PagesVCode").Value = pagesVCode
            command.Parameters("@ParentVCode").Value = If(parentVCode = 0, DBNull.Value, parentVCode)
            command.Parameters("@Name").Value = name
            command.Parameters("@Message").Value = message
            command.Parameters("@Email").Value = If(String.IsNullOrEmpty(email), DBNull.Value, email)
            command.Parameters("@Website").Value = If(String.IsNullOrEmpty(website), DBNull.Value, website)
            command.Parameters("@IdentificationVCode").Value = If(identificationVCode = 0, DBNull.Value, identificationVCode)
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
    Public Shared Function ConvertFromXMLToPagesComments(xmlString As String) As List(Of PagesCommentEntity)
        Try
            Dim PagesComments As New List(Of PagesCommentEntity)
            Dim PagesComment As PagesCommentEntity
            Dim xml As New XmlDocument()
            If Not xmlString Is Nothing Then
                xml.LoadXml(xmlString)
                Dim xnList As XmlNodeList = xml.SelectNodes("/PagesComments/PagesComment")
                For Each xn As XmlNode In xnList
                    PagesComment = New PagesCommentEntity
                    If xn("VCode") IsNot Nothing Then
                        PagesComment.VCode = xn("VCode").InnerText
                    End If
                    If xn("PagesVCode") IsNot Nothing Then
                        PagesComment.PagesVCode = xn("PagesVCode").InnerText
                    End If
                    If xn("ApprovalStateVCode") IsNot Nothing Then
                        PagesComment.ApprovalStateVCode = xn("ApprovalStateVCode").InnerText
                    End If
                    If xn("Name") IsNot Nothing Then
                        PagesComment.Name = xn("Name").InnerText
                    End If
                    If xn("Message") IsNot Nothing Then
                        PagesComment.Message = xn("Message").InnerText
                    End If
                    If xn("Email") IsNot Nothing Then
                        PagesComment.Email = xn("Email").InnerText
                    End If
                    If xn("Website") IsNot Nothing Then
                        PagesComment.Website = xn("Website").InnerText
                    End If
                    If xn("ApprovalStateVCode") IsNot Nothing Then
                        PagesComment.ApprovalStateVCode = xn("ApprovalStateVCode").InnerText
                    End If
                    If xn("ParentVCode") IsNot Nothing Then
                        PagesComment.ParentVCode = xn("ParentVCode").InnerText
                    End If
                    If xn("EntryDate") IsNot Nothing Then
                        PagesComment.EntryDate = xn("EntryDate").InnerText
                    End If
                    If xn("LastModifiedDate") IsNot Nothing Then
                        PagesComment.LastModifiedDate = xn("LastModifiedDate").InnerText
                    End If
                    PagesComments.Add(PagesComment)
                Next
            End If
            Return PagesComments
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function PagesRateIns(ByRef vcode As Long, pagesVCode As Long, rate As Decimal, sessionId As String, ByRef averageRate As Decimal, Optional ip As String = Nothing, Optional ByRef message As String = Nothing) As Boolean
        Dim command As SqlCommand = Nothing
        Try
            command = ARCOConnection.Command("AZ.PagesRateIns")
            command.Parameters("@VCode").Direction = ParameterDirection.Output
            command.Parameters("@PagesVCode").Value = pagesVCode
            command.Parameters("@Rate").Value = rate
            command.Parameters("@SessionId").Value = sessionId
            command.Parameters("@IP").Value = If(String.IsNullOrEmpty(ip), DBNull.Value, ip)
            command.Parameters("@AverageRate").Direction = ParameterDirection.Output
            command.Parameters("@Message").Direction = ParameterDirection.Output
            command.ExecuteNonQuery()
            vcode = If(IsDBNull(command.Parameters("@VCode").Value), 0, command.Parameters("@VCode").Value)
            averageRate = If(IsDBNull(command.Parameters("@AverageRate").Value), 0, command.Parameters("@AverageRate").Value)
            message = If(IsDBNull(command.Parameters("@Message").Value), Nothing, command.Parameters("@Message").Value)

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
    Public Function GetTotalPageRates() As TotalPagesRateData
        Dim totalPagesRate As TotalPagesRateData = Nothing
        Dim command As SqlCommand = Nothing
        Dim reader As SqlDataReader = Nothing
        Try
            command = ARCOConnection.Command("AZ.GetTotalPageRates")
            reader = command.ExecuteReader()
            If reader.Read() Then
                totalPagesRate = GetTotalPagesRateData(reader)
            End If
            Return totalPagesRate
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
    Public Function GetPagesComment(vcode As Long) As PagesCommentEntity
        Dim pagesComment As PagesCommentEntity = Nothing
        Dim command As SqlCommand = Nothing
        Dim reader As SqlDataReader = Nothing
        Try
            command = ARCOConnection.Command("AZ.GetPagesComment")
            command.Parameters("@VCode").Value = vcode
            reader = command.ExecuteReader()
            If reader.Read() Then
                pagesComment = GetPagesCommentEntity(reader)
            End If
            Return pagesComment
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
    Public Function PagesCommentApprovalStateUpd(vcode As Long, approvalStateVCode As ApprovalStateEnum) As Boolean
        Dim command As SqlCommand = Nothing
        Try
            command = ARCOConnection.Command("AZ.PagesCommentApprovalStateUpd")
            command.Parameters("@VCode").Value = vcode
            command.Parameters("@ApprovalStateVCode").Value = approvalStateVCode
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
    Public Function GetPagesComments(Optional approvalStateVCode As ApprovalStateEnum = ApprovalStateEnum.NONE, Optional pageVCode As Long = 0) As List(Of PagesCommentEntity)
        Dim pagesComments As New List(Of PagesCommentEntity)
        Dim command As SqlCommand = Nothing
        Dim reader As SqlDataReader = Nothing
        Try
            command = ARCOConnection.Command("AZ.GetPagesComments")
            command.Parameters("@ApprovalStateVCode").Value = If(approvalStateVCode = ApprovalStateEnum.NONE, DBNull.Value, approvalStateVCode)
            command.Parameters("@PageVCode").Value = If(pageVCode = 0, DBNull.Value, pageVCode)
            reader = command.ExecuteReader()
            While reader.Read()
                pagesComments.Add(GetPagesCommentEntity(reader))
            End While
            Return pagesComments
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
    Public Function DeletePage(id As Guid?) As PagesEntity
        Dim item As New PagesEntity
        Dim command As SqlCommand = Nothing
        Dim reader As SqlDataReader = Nothing
        Try
            command = ARCOConnection.Command("AZ.DeletePage")
            command.Parameters("@id").Value = If(id = Nothing, DBNull.Value, id)
            reader = command.ExecuteReader()
            If reader.HasRows Then
                reader.Read()
                item = GetPagesEntity(reader)
            End If
            Return item
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
End Class
