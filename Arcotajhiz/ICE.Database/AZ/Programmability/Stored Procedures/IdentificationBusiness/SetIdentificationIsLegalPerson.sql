CREATE PROCEDURE [AZ].[SetIdentificationIsLegalPerson](
	@IdentificationVCode BIGINT,
	@IsLegalPerson BIT
)
AS
BEGIN
	UPDATE AZ.Identification SET IsLegalPerson = @IsLegalPerson WHERE VCode = @IdentificationVCode
END