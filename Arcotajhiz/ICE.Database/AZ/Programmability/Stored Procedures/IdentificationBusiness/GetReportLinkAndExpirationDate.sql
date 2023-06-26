CREATE PROCEDURE [AZ].[GetReportLinkAndExpirationDate](
	@NationalCode VARCHAR(20),
	@Cellphone VARCHAR(20),
	@ReceiverCellphone VARCHAR(20) = NULL,
	@UserVCode BIGINT = NULL,
	@CompanyNationalID VARCHAR(20) = NULL
)
AS
BEGIN
	IF @UserVCode IS NULL
	BEGIN
		SELECT TOP 1 ReportExpirationDate,ReportLink,I.VCode IdentificationVCode,I.IdentificationStateVCode FROM AZ.Identification (NOLOCK) I
		INNER JOIN AZ.IdentificationHistory (NOLOCK) IH ON IH.IdentificationVCode = I.Vcode AND IH.IdentificationStateVCode = 11
		WHERE Cellphone = @Cellphone AND 
			NationalCode = @NationalCode AND 
			ISNULL(CompanyNationalID,0) = ISNULL(@CompanyNationalID,ISNULL(@CompanyNationalID,0)) AND 
			ISNULL(I.ReceiverCellphone,0) = ISNULL(@ReceiverCellphone,0) AND
			ReportExpirationDate > GETDATE() AND
			ReportLink IS NOT NULL
		ORDER BY I.VCode DESC
	END
	ELSE
	BEGIN
		DECLARE @Now DATETIME = GETDATE(),@UserTypeVCode INT 
		SELECT @UserTypeVCode = UserTypeVCode FROM [$(ICEUserManagement)].AZ.[User] WHERE VCode = @UserVCode

		IF @UserTypeVCode = 1 AND @UserVCode <> 4
		BEGIN
			SELECT TOP 1 ReportExpirationDate,ReportLink,I.VCode IdentificationVCode,I.IdentificationStateVCode FROM AZ.Identification (NOLOCK) I
			INNER JOIN AZ.IdentificationHistory (NOLOCK) IH ON IH.IdentificationVCode = I.Vcode AND IH.IdentificationStateVCode = 11
			WHERE Cellphone = @Cellphone AND 
				NationalCode = @NationalCode AND 
				ISNULL(CompanyNationalID,0) = ISNULL(@CompanyNationalID,ISNULL(@CompanyNationalID,0)) AND 
				ISNULL(I.ReceiverCellphone,0) = ISNULL(@ReceiverCellphone,0) AND
				ReportExpirationDate > GETDATE() AND
				(EXISTS(SELECT 1 FROM [$(ICEUserManagement)].AZ.[User] WHERE VCode = I.UserVCode AND UserTypeVCode = 3) OR I.UserVCode = @UserVCode)
			ORDER BY I.VCode DESC
		END
		ELSE
		BEGIN
			SELECT TOP 1 ReportExpirationDate,ReportLink,I.VCode IdentificationVCode,I.IdentificationStateVCode FROM AZ.Identification (NOLOCK) I
			INNER JOIN AZ.IdentificationHistory (NOLOCK) IH ON IH.IdentificationVCode = I.Vcode AND IH.IdentificationStateVCode = 11
			WHERE Cellphone = @Cellphone AND 
				NationalCode = @NationalCode AND 
				ISNULL(CompanyNationalID,0) = ISNULL(@CompanyNationalID,ISNULL(@CompanyNationalID,0)) AND 
				ISNULL(I.ReceiverCellphone,0) = ISNULL(@ReceiverCellphone,0) AND
				ReportExpirationDate > GETDATE() AND
				(EXISTS(SELECT 1 FROM [$(ICEUserManagement)].AZ.[User] WHERE VCode = I.UserVCode AND UserTypeVCode = 3) OR I.UserVCode = @UserVCode) 
				--AND AppIcs24HashCode IS NOT NULL 
			ORDER BY I.VCode DESC
		END
	END
END
GO

