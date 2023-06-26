CREATE PROCEDURE [AZ].[Background_ChangeIdentificationState]	
AS
BEGIN
	UPDATE AZ.Identification SET IdentificationStateVCode = 14,LastModifiedDate = GETDATE() 
	WHERE VCode IN (SELECT VCode FROM AZ.Identification WHERE IdentificationStateVCode IN (1) AND ReportExpirationDate < GETDATE())

	DECLARE @UserVCode BIGINT,@IdentificationVCode BIGINT
  
	DECLARE cur CURSOR FOR   
	SELECT I.VCode,I.UserVCode FROM AZ.Identification I WHERE 
		EXISTS(SELECT 1 FROM [$(ICEUserManagement)].AZ.[User] U WHERE UserTypeVCode = 2 AND I.UserVCode = U.VCode) 
		AND (CAST(EntryDate AS DATE) = CAST(GETDATE() AS DATE) OR CAST(EntryDate AS DATE) = CAST(DATEADD(DAY,-1,GETDATE()) AS DATE))
		AND I.IdentificationStateVCode IN (11,17)
		AND UserPaymentTypeVCode = 2
		AND DATEDIFF(HOUR,EntryDate,GETDATE()) > 1
  
	OPEN cur  
  
	FETCH NEXT FROM cur INTO @IdentificationVCode,@UserVCode
  
	WHILE @@FETCH_STATUS = 0  
	BEGIN  
		EXEC [AZ].[ReturnIdentificationCredit] @IdentificationVCode = @IdentificationVCode,@UserVCode = @UserVCode

		FETCH NEXT FROM cur INTO @IdentificationVCode,@UserVCode 
	END   
	CLOSE cur;  
	DEALLOCATE cur;
END
GO

