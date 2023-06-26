CREATE PROCEDURE [AZ].[Rpt_GetIndividualFirmRequestRegistration](
	@VCode BIGINT
)
AS
BEGIN
	SELECT RR.[VCode] VCode
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
		,[RequestRegistrationStateVCode]
		,[FirmRegistrationVCode]
		,[StateDescription]
		,[PresenterCode]
		,RR.[EntryDate]
		,RR.[LastModifiedDate],
		('<RequestRegistrationFiles>' + 
			(SELECT * FROM AZ.RequestRegistrationFile (NOLOCK) RRF WHERE RR.VCode = RRF.RequestRegistrationVCode FOR XML PATH('RequestRegistrationFile')) + 
		'</RequestRegistrationFiles>') RequestRegistrationFile
		,RR.[Description]
	FROM [AZ].[RequestRegistration] RR
	LEFT JOIN [AZ].[Location] L ON RR.LocationVCode = L.VCode
	WHERE RR.VCode =  @VCode
END
