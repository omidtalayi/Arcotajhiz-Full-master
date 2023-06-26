CREATE PROCEDURE [AZ].[Admin_GetTransferFileSheba](
	@JDate CHAR(8)
)
AS
BEGIN
	BEGIN TRAN
    BEGIN TRY
		DECLARE @DocumentNo INT,
				@TransferFileShebaVCode BIGINT,
				@TransferFileShebaDetailVCode BIGINT

		IF EXISTS(SELECT 1 FROM AZ.TransferFileSheba WHERE JDate = @JDate)
		BEGIN
			SELECT	TransferFileShebaVCode,
					'تسویه شماره ' + CAST(((ROW_NUMBER() OVER(ORDER BY ShebaNumber)  - 1) % 5) + 1 AS NVARCHAR(10)) ClearingNumber,
					DocumentNo,
					CompanyAccount,
					ShebaNumber,
					PersonageName,
					JDate,
					[Description],
					SUM(Amount) Amount,
					ROW_NUMBER() OVER(ORDER BY ShebaNumber) R
			FROM
			(
				SELECT	TFS.VCode TransferFileShebaVCode,
						'صورت پرداخت شماره ' + CAST(TFS.DocumentNo AS NVARCHAR(10)) DocumentNo,
						TFSD.R,
						TFSD.CompanyAccount,
						SUBSTRING(TFSD.ShebaNumber,1,4) + ' ' + SUBSTRING(TFSD.ShebaNumber,5,4) + ' ' + SUBSTRING(TFSD.ShebaNumber,9,4) + ' ' + SUBSTRING(TFSD.ShebaNumber,13,4) + ' ' + SUBSTRING(TFSD.ShebaNumber,17,4) + ' ' + SUBSTRING(TFSD.ShebaNumber,21,4) + ' ' + SUBSTRING(TFSD.ShebaNumber,25,2) ShebaNumber,
						TFSD.PersonageName,
						TFSD.Amount,
						SUBSTRING(CAST(ISNULL(TFS.JDate,'') AS varchar(8)),1,4) + '/' + SUBSTRING(CAST(ISNULL(TFS.JDate,'') AS varchar(8)),5,2) + '/' +  SUBSTRING(CAST(ISNULL(TFS.JDate,'') AS varchar(8)),7,2) JDate,
						TFSD.[Description]
				FROM AZ.TransferFileSheba TFS
				INNER JOIN AZ.TransferFileShebaDetail TFSD ON TFS.VCode = TFSD.TransferFileShebaVCode
				WHERE TFS.JDate = @JDate
			) K
			GROUP BY TransferFileShebaVCode,DocumentNo,CompanyAccount,ShebaNumber,PersonageName,JDate,[Description]
			ORDER BY 1,DocumentNo,CompanyAccount,ShebaNumber,PersonageName,JDate,[Description]
		END
		ELSE
		BEGIN
			IF @JDate >= AZ.ConvertToJalaliDate(GETDATE())
			BEGIN
				SELECT @DocumentNo = ISNULL(MAX(DocumentNo),0) + 1 FROM AZ.TransferFileSheba 

				SELECT	IIB.VCode IdentificationInvoiceBatchVCode,
						IIB.TotalShare,
						U.ShebaNumber,
						U.[Name] PersonageName,
						U.VCode UserVCode
				INTO #Batches FROM AZ.IdentificationInvoiceBatch (NOLOCK) IIB 
				INNER JOIN [$(ICEUserManagement)].AZ.[User] (NOLOCK) U ON IIB.UserVCode = U.VCode
				WHERE IIB.StateVCode = 2 AND IIB.UserVCode IS NOT NULL AND ISNULL(U.ShebaNumber,'0') <> '0'
			
				IF EXISTS(SELECT 1 FROM #Batches)
				BEGIN
					SELECT 	ROW_NUMBER() OVER(ORDER BY PersonageName) R,
							'8110088235' CompanyAccount,
							ShebaNumber,
							PersonageName,
							SUM(TotalShare) Amount,
							AZ.ConvertToJalaliDate(GETDATE()) JDate,
							'بابت تسویه با ' + PersonageName [Description],
							UserVCode
					INTO #Data FROM
					(	
						SELECT * FROM #Batches
					) K
					WHERE TotalShare > 0
					GROUP BY ShebaNumber,PersonageName,UserVCode

					INSERT AZ.TransferFileSheba(DocumentNo,JDate,TotalRow,TotalPrice,StateVCode)
					SELECT @DocumentNo DocumentNo,@JDate JDate,COUNT(1) TotalRow,SUM(Amount) TotalPrice,3 StateVCode FROM #Data

					SET @TransferFileShebaVCode = SCOPE_IDENTITY()

					INSERT AZ.TransferFileShebaDetail(TransferFileShebaVCode,R,CompanyAccount,ShebaNumber,PersonageName,Amount,[Description],UserVCode)
					SELECT @TransferFileShebaVCode,R,CompanyAccount,ShebaNumber,PersonageName,Amount,[Description],UserVCode FROM #Data

					--SET @TransferFileShebaDetailVCode = (SELECT TOP 1 VCode FROM AZ.TransferFileShebaDetail WHERE TransferFileShebaVCode = @TransferFileShebaVCode)

					--DECLARE cur CURSOR FOR   
					--SELECT VCode FROM AZ.TransferFileShebaDetail WHERE TransferFileShebaVCode = @TransferFileShebaVCode
  
					--OPEN cur  
  
					--FETCH NEXT FROM cur INTO @TransferFileShebaDetailVCode  
  
					--WHILE @@FETCH_STATUS = 0  
					--BEGIN  

					SELECT IIB.VCode IIBVCode,TFSD.VCode TFSDVCode INTO #IIB FROM AZ.IdentificationInvoiceBatch IIB
					INNER JOIN AZ.TransferFileShebaDetail TFSD ON IIB.UserVCode = TFSD.UserVCode AND IIB.TransferFileShebaDetailVCode IS NULL
					WHERE TFSD.TransferFileShebaVCode = @TransferFileShebaVCode 

					UPDATE AZ.IdentificationInvoiceBatch SET TransferFileShebaDetailVCode = IIB.TFSDVCode,StateVCode = 3,LastModifiedDate = GETDATE()
					FROM #IIB IIB WHERE IIB.IIBVCode = AZ.IdentificationInvoiceBatch.VCode
					
					--WHERE EXISTS 
					--(
					--	SELECT 1 FROM #Batches WHERE IdentificationInvoiceBatchVCode = AZ.IdentificationInvoiceBatch.VCode 
					--	AND UserVCode = AZ.IdentificationInvoiceBatch.UserVCode
					--)

					--	FETCH NEXT FROM cur INTO @TransferFileShebaDetailVCode  
					--END   
					--CLOSE cur
					--DEALLOCATE cur

					INSERT AZ.IdentificationInvoiceBatchHistory(IdentificationInvoiceBatchVCode,StateVCode)
					SELECT IdentificationInvoiceBatchVCode,3 FROM #Batches

					UPDATE AZ.IdentificationInvoice SET IdentificationInvoiceStateVCode = 3,LastModifiedDate = GETDATE() 
					WHERE EXISTS(SELECT 1 FROM #Batches B WHERE B.IdentificationInvoiceBatchVCode = AZ.IdentificationInvoice.BatchVCode)

					INSERT AZ.IdentificationInvoiceHistory(IdentificationInvoiceVCode,IdentificationInvoiceStateVCode)
					SELECT VCode,3 FROM AZ.IdentificationInvoice 
					WHERE EXISTS(SELECT 1 FROM #Batches B WHERE B.IdentificationInvoiceBatchVCode = AZ.IdentificationInvoice.BatchVCode)

					DROP TABLE #IIB
				END
				

				SELECT	TransferFileShebaVCode,
						'تسویه شماره ' + CAST(((ROW_NUMBER() OVER(ORDER BY ShebaNumber) - 1) % 5) + 1 AS NVARCHAR(10)) ClearingNumber,
						DocumentNo,
						CompanyAccount,
						ShebaNumber,
						PersonageName,
						JDate,
						[Description],
						SUM(Amount) Amount,
						ROW_NUMBER() OVER(ORDER BY ShebaNumber) R
				FROM
				(
					SELECT	TFS.VCode TransferFileShebaVCode,
							'صورت پرداخت شماره ' + CAST(TFS.DocumentNo AS NVARCHAR(10)) DocumentNo,
							TFSD.R,
							TFSD.CompanyAccount,
							SUBSTRING(TFSD.ShebaNumber,1,4) + ' ' + SUBSTRING(TFSD.ShebaNumber,5,4) + ' ' + SUBSTRING(TFSD.ShebaNumber,9,4) + ' ' + SUBSTRING(TFSD.ShebaNumber,13,4) + ' ' + SUBSTRING(TFSD.ShebaNumber,17,4) + ' ' + SUBSTRING(TFSD.ShebaNumber,21,4) + ' ' + SUBSTRING(TFSD.ShebaNumber,25,2) ShebaNumber,
							TFSD.PersonageName,
							TFSD.Amount,
							SUBSTRING(CAST(ISNULL(TFS.JDate,'') AS varchar(8)),1,4) + '/' + SUBSTRING(CAST(ISNULL(TFS.JDate,'') AS varchar(8)),5,2) + '/' +  SUBSTRING(CAST(ISNULL(TFS.JDate,'') AS varchar(8)),7,2) JDate,
							TFSD.[Description]
					FROM AZ.TransferFileSheba TFS
					INNER JOIN AZ.TransferFileShebaDetail TFSD ON TFS.VCode = TFSD.TransferFileShebaVCode
					WHERE TFS.VCode = @TransferFileShebaVCode
				) K
				GROUP BY TransferFileShebaVCode,DocumentNo,CompanyAccount,ShebaNumber,PersonageName,JDate,[Description]
				ORDER BY 1,DocumentNo,CompanyAccount,ShebaNumber,PersonageName,JDate,[Description]
			END
		END

		COMMIT TRAN
	END TRY
	BEGIN CATCH
		DECLARE @ErrNo INT,@ErrMsg NVARCHAR(4000),@ErrSev INT,@ErrStt INT
		SELECT @ErrMsg = '(' + CONVERT(NVARCHAR(4000),ERROR_NUMBER()) + ') ' + ERROR_MESSAGE(),@ErrSev = ERROR_SEVERITY(),@ErrStt = ERROR_STATE()
		IF @@TRANCOUNT > 0 ROLLBACK TRAN
		RAISERROR(@ErrMsg,@ErrSev,@ErrStt)
	END CATCH
END