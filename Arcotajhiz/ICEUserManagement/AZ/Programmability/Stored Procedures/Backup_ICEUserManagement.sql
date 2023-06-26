CREATE PROCEDURE [AZ].[Backup_ICEUserManagement]
AS
BEGIN
	DECLARE @var NVARCHAR(max) ='D:\Backups\ICEUserManagement_' + REPLACE(RTRIM(CONVERT(CHAR,GETDATE())), ':',',')+'.bak';
	BACKUP DATABASE ICEUserManagement TO DISK = @var WITH COMPRESSION

	DECLARE @var98 NVARCHAR(max) ='\\192.168.3.1\BackUps\Transactions\ICEUserManagement_' + REPLACE(RTRIM(CONVERT(CHAR,GETDATE())), ':',',')+'.bak';
    BACKUP DATABASE ICEUserManagement TO DISK = @var98 WITH COMPRESSION
END