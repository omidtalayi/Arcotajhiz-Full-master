﻿CREATE PROCEDURE [AZ].[GetSmsLogs](
	@SMSLogStateVCode SMALLINT,
	@SMSTypeVCode SMALLINT = NULL
)
AS
BEGIN
	SELECT * FROM AZ.SMSLog
	WHERE SendState = 0 OR  SendState = 1 OR (SendState = 4 AND (DATEDIFF(DAY,CAST(EntryDate AS DATE),CAST(GETDATE() AS DATE))) < 1)
END
GO
