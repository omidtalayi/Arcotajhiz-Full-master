Imports System.Data.SqlClient
Imports System.Xml
Imports ARCO.Business.Models
Imports ARCO.Entities.Enums

Public Class ContentBusinesses
    Private Shared Function GetPagesData(reader As IDataRecord) As PagesData
        Try
            Dim oPagesData = New PagesData With
                {
                    .Row = reader("Row"),
                    .VCode = If(IsDBNull(reader("VCode")), 0, reader("VCode")),
                    .Name = If(IsDBNull(reader("Name")), Nothing, reader("Name")),
                    .PagesType = If(IsDBNull(reader("PagesTypeVCode")), Nothing, reader("PagesTypeVCode")),
                    .Title = If(IsDBNull(reader("Title")), 0, reader("Title")),
                    .Description = If(IsDBNull(reader("Description")), Nothing, reader("Description")),
                    .Body = If(IsDBNull(reader("Body")), Nothing, reader("Body")),
                    .Link = If(IsDBNull(reader("Link")), Nothing, reader("Link")),
                    .Keywords = If(IsDBNull(reader("Keywords")), Nothing, reader("Keywords")),
                    .ImageLink = If(IsDBNull(reader("ImageLink")), Nothing, reader("ImageLink")),
                    .EntryDate = If(IsDBNull(reader("EntryDate")), Nothing, reader("EntryDate")),
                    .LastModifiedDate = If(IsDBNull(reader("LastModifiedDate")), Nothing, reader("LastModifiedDate")),
                    .TopicVCode = If(IsDBNull(reader("TopicVCode")), 0, reader("TopicVCode")),
                    .PagesImages = ConvertFromXMLToPagesImages(If(IsDBNull(reader("PagesImages")), Nothing, reader("PagesImages")))
                }
            Return oPagesData
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Private Shared Function GetTopicData(reader As IDataRecord) As TopicData
        Try
            Dim oTopicData = New TopicData With
                {
                    .VCode = If(IsDBNull(reader("VCode")), 0, reader("VCode")),
                    .Name = If(IsDBNull(reader("Name")), Nothing, reader("Name"))
                }
            Return oTopicData
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetPages(Optional pageNo As Integer = 0, Optional pageSize As Integer = 0, Optional ByRef rowCount As Integer = 0) As List(Of PagesData)
        Dim oPagesData As New List(Of PagesData)
        Dim command As SqlCommand = Nothing
        Dim reader As SqlDataReader = Nothing
        Try
            command = ICEConnection.Command("AZ.Admin_GetPages")
            If pageSize > -1 AndAlso pageNo > -1 Then
                command.Parameters("@PageNo").Value = If(pageNo = 0, DBNull.Value, pageNo)
                command.Parameters("@PageSize").Value = If(pageSize = 0, DBNull.Value, pageSize)
            End If
            reader = command.ExecuteReader()
            While reader.Read()
                oPagesData.Add(GetPagesData(reader))
                rowCount = reader("RowCount")
            End While
            Return oPagesData
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
    Public Function CreateContentPage(Name As String, Title As String, Body As String, Link As String, ImageLink As String, IsActivate As Boolean, Optional keywords As String = Nothing, Optional Description As String = "", Optional Images As List(Of PagesImageData) = Nothing, Optional pageType As PageTypeEnum = PageTypeEnum.BLOG, Optional PageTopicVCode As Integer = 4) As Boolean
        Dim command As SqlCommand = Nothing
        Dim reader As SqlDataReader = Nothing
        Try
            command = ICEConnection.Command("AZ.Admin_PagesIns")
            'command.ExecuteReader()
            command.Parameters("@Name").Value = Name
            command.Parameters("@Body").Value = Body
            command.Parameters("@Title").Value = Title
            command.Parameters("@PageTypeVCode").Value = pageType
            'command.Parameters("@ImagePath").Value = If(String.IsNullOrEmpty(ImagePath), DBNull.Value, ImagePath)
            command.Parameters("@Description").Value = If(String.IsNullOrEmpty(Description), DBNull.Value, Description)
            command.Parameters("@Keywords").Value = If(String.IsNullOrEmpty(keywords), DBNull.Value, keywords)
            command.Parameters("@Link").Value = Link
            command.Parameters("@ImageLink").Value = ImageLink
            command.Parameters("@IsActivate").Value = If(String.IsNullOrEmpty(IsActivate), DBNull.Value, IsActivate)
            command.Parameters("@Images").Value = If(Images Is Nothing, DBNull.Value, ConvertPagesImageDataToXml(Images))
            command.Parameters("@TopicVCode").Value = PageTopicVCode
            command.ExecuteNonQuery()
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
    Public Function UpdatePage(VCode As Integer, Name As String, Body As String, Title As String, Link As String, ImageLink As String, Optional isActive As Boolean = True, Optional keywords As String = Nothing, Optional Description As String = "", Optional pageType As PageTypeEnum = PageTypeEnum.BLOG, Optional PageTopicVCode As Integer = 4) As Boolean
        Dim command As SqlCommand = Nothing
        Dim reader As SqlDataReader = Nothing
        Try
            command = ICEConnection.Command("AZ.Admin_PagesUpd")
            'command.ExecuteReader()
            command.Parameters("@VCode").Value = VCode
            command.Parameters("@Name").Value = Name
            command.Parameters("@Title").Value = Title
            command.Parameters("@PageTypeVCode").Value = pageType
            command.Parameters("@Description").Value = If(String.IsNullOrEmpty(Description), DBNull.Value, Description)
            command.Parameters("@Keywords").Value = If(String.IsNullOrEmpty(keywords), DBNull.Value, keywords)
            command.Parameters("@Body").Value = Body
            command.Parameters("@Link").Value = Link
            command.Parameters("@ImageLink").Value = ImageLink
            command.Parameters("@IsActivate").Value = isActive
            command.Parameters("@TopicVCode").Value = PageTopicVCode
            command.ExecuteNonQuery()
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
    Public Function DeletePage(Optional VCode As Integer = 0) As Boolean
        Dim command As SqlCommand = Nothing
        Dim reader As SqlDataReader = Nothing
        Try
            command = ICEConnection.Command("AZ.Admin_PagesDel")
            'command.ExecuteReader()
            command.Parameters("@VCode").Value = VCode
            command.ExecuteNonQuery()
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

    Public Function GetIndividualContentPage(Optional VCode As Integer = 0) As PagesData
        Dim oPageContentData As PagesData = Nothing
        Dim command As SqlCommand = Nothing
        Dim reader As SqlDataReader = Nothing
        Try
            command = ICEConnection.Command("AZ.Admin_GetIndividualContentPage")
            command.Parameters("@VCode").Value = If(String.IsNullOrEmpty(VCode), DBNull.Value, VCode)
            reader = command.ExecuteReader()
            If reader.Read() Then
                oPageContentData = GetPagesData(reader)
            End If
            Return oPageContentData
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
    Private Shared Function ConvertPagesImageDataToXml(pagesImages As List(Of PagesImageData)) As String
        Return pagesImages.Aggregate(String.Empty, Function(current, pagesImage) current & String.Format("<PI N=""{0}""/>", pagesImage.Name))
    End Function
    Public Function PagesImageDel(vcode As Integer) As Boolean
        Dim command As SqlCommand = Nothing
        Try
            command = ICEConnection.Command("AZ.PagesImageDel")
            command.Parameters("@VCode").Value = vcode
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
    Public Function PagesImageIns(name As String, pagesVCode As Integer) As Boolean
        Dim command As SqlCommand = Nothing
        Try
            command = ICEConnection.Command("AZ.PagesImageIns")
            command.Parameters("@VCode").Direction = ParameterDirection.Output
            command.Parameters("@Name").Value = name
            command.Parameters("@PagesVCode").Value = pagesVCode
            command.ExecuteNonQuery()
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
    Public Function PagesImageUpd(vcode As Integer, name As String, Optional pagesVCode As Integer = 0) As Boolean
        Dim command As SqlCommand = Nothing
        Try
            command = ICEConnection.Command("AZ.PagesImageUpd")
            command.Parameters("@VCode").Value = vcode
            command.Parameters("@Name").Value = name
            command.Parameters("@PagesVCode").Value = pagesVCode
            command.ExecuteNonQuery()
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
    Public Function GetTopics() As List(Of TopicData)
        Dim oTopicData As New List(Of TopicData)
        Dim command As SqlCommand = Nothing
        Dim reader As SqlDataReader = Nothing
        Try
            command = ICEConnection.Command("AZ.Admin_GetTopic")
            reader = command.ExecuteReader()
            While reader.Read()
                oTopicData.Add(GetTopicData(reader))
            End While
            Return oTopicData
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
End Class
