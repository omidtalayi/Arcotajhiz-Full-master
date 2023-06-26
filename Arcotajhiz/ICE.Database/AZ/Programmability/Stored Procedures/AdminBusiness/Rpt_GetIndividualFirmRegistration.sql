CREATE PROCEDURE [AZ].[Rpt_GetIndividualFirmRegistration]
(
@VCode BIGINT
)
AS
BEGIN
	SELECT FR.[VCode] VCode
		  ,[FirmRegistrationStateVCode]
		  ,[FirmName]
		  ,[RegisteredIdentificationNo]
		  ,[RegistrationNo]
		  ,[Email]
		  ,[Telephone]
		  ,[RegisterDate]
		  ,L.[Name] [LocationName]
		  ,[PostalCode]
		  ,[Address]
		  ,[ContactPointName]
		  ,[ContactPointFamilyName]
		  ,[ContactPointCellphone]
		  ,[LetterFileName]
		  ,[OfficialPaperFileName]
		  ,[EmailLink]
		  ,[EmailIsVerified]
		  ,[PresenterCode]
		  ,[ContactPointCellphoneVerificationCode]
		  ,[VerificationCodeExpirationDate]
		  ,[ContanctPointCellPhoneIsVerified]
		  ,[EntryDate]
		  ,[LastModifiedDate]
		  ,[description]
	  FROM [AZ].[FirmRegistration] FR
			LEFT JOIN [AZ].[Location] L ON FR.LocationVCode = L.VCode
	  WHERE FR.VCode =  @VCode
END
GO

