﻿CREATE PROCEDURE AZ.GetPagesComment(
	@VCode BIGINT
)
AS
BEGIN
	SELECT * FROM AZ.PagesComment (NOLOCK) WHERE VCode = @VCode
END
GO