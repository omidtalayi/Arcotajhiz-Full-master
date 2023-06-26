CREATE PROCEDURE [AZ].[VisitLogIns] @Code UNIQUEIDENTIFIER, @Url VARCHAR(300),@IP VARCHAR(30),@Browser VARCHAR(100),@ReferrerUrl VARCHAR(300),
@UserVCode INT,@GUID UNIQUEIDENTIFIER, @LoadDate DATETIME, @DropDate DATETIME,@DeviceTypeVCode INT,@IsAjaxRequest BIT,@PageTitle VARCHAR(256),
@PostData VARCHAR(MAX) = NULL,@RegisterDate DATETIME = NULL, @IP_API_COM VARCHAR(MAX) = NULL,@RequestCookies VARCHAR(MAX) = NULL
AS 
BEGIN
	DECLARE @WebsiteType TINYINT
	SET @WebsiteType = (SELECT CASE WHEN CHARINDEX('www.icescoring.com', @Url) > 0 THEN 1 WHEN CHARINDEX('icescoring.com', @Url) > 0 THEN 2 ELSE 0 END)
	/*	قرار شد شبها یوزرهای دیوایس به روز رسانی شود
IF @UserVCode IS NULL
		SET @UserVCode = (SELECT UserVCode FROM AZ.VisitLog WITH(NOLOCK) WHERE VCode = (SELECT MAX(vcode) vcode FROM AZ.VisitLog WHERE (IP = @IP OR GUID = @GUID) AND WebsiteType = CASE WHEN CHARINDEX('www.ibiar.com', @Url) > 0 THEN 1 WHEN CHARINDEX('store.ibiar.com', @Url) > 0 THEN 2 ELSE 0 END AND NOT UserVCode IS NULL) AND DATEDIFF(hh, RegisterDate, GETDATE()) < 24)
		*/
    INSERT INTO AZ.VisitLog(Code, Url,[IP],Browser,ReferrerUrl,UserVCode,[GUID],LoadDate,DropDate,DeviceTypeVCode,IsAjaxRequest,PageTitle,RegisterDate,WebsiteType,PostData,IP_API_COM, RequestCookies)
    VALUES (@Code, @Url,@IP,@Browser,@ReferrerUrl,@UserVCode,@GUID,@LoadDate,@DropDate,@DeviceTypeVCode,@IsAjaxRequest,@PageTitle,ISNULL(@RegisterDate,GETDATE()),@WebsiteType,ISNULL(@PostData, ''),ISNULL(@IP_API_COM, ''),ISNULL(@RequestCookies,''))
END
GO

