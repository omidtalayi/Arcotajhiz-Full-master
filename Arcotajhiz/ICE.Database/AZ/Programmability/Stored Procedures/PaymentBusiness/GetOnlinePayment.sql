﻿CREATE PROCEDURE [AZ].[GetOnlinePayment](
	@VCode BIGINT
)
AS
BEGIN
	SELECT * FROM AZ.OnlinePayment (NOLOCK) WHERE VCode = @VCode
END