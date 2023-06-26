CREATE PROCEDURE [AZ].[GetIdentificationState](
	@IdentificationStateVCode SMALLINT
)	
AS
BEGIN
	SELECT * FROM AZ.Identification WHERE VCode = @IdentificationStateVCode
END
