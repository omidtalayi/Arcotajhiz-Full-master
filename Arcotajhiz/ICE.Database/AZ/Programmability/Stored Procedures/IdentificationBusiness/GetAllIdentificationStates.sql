CREATE PROCEDURE [AZ].[GetAllIdentificationStates](
	@UserVCode INT
)
AS
BEGIN
	SELECT	IdentificationStateVCode,
			COUNT(1) cnt
	FROM AZ.Identification (NOLOCK) I
	WHERE I.IdentificationStateVCode IN (1,3,4,7,10,11,13,14,15)
	GROUP BY IdentificationStateVCode
END