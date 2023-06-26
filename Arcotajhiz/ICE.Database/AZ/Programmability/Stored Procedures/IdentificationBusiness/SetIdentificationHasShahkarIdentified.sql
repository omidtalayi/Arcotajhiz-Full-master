CREATE PROCEDURE [AZ].[SetIdentificationHasShahkarIdentified](
	@IdentificationVCode BIGINT,
	@HasShahkarIdentified BIT
)
AS
BEGIN
	UPDATE AZ.Identification SET HasShahkarIdentified = @HasShahkarIdentified,HasCheckedKyc = 1 WHERE VCode = @IdentificationVCode
END