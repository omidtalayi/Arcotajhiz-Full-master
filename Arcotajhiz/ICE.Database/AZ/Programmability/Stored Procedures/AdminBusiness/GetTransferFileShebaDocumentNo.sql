﻿CREATE PROCEDURE AZ.GetTransferFileShebaDocumentNo
AS
BEGIN
	SELECT 0 DocumentNo
	UNION ALL
	SELECT DISTINCT DocumentNo FROM AZ.TransferFileSheba (NOLOCK)
END