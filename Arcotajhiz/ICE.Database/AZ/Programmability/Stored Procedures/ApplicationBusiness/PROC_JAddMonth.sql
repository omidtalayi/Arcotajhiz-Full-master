﻿CREATE PROCEDURE AZ.PROC_JAddMonth(
	@JDate CHAR(8),
	@Month SMALLINT
)
AS
BEGIN
	SELECT [AZ].[AddMonths](@JDate,@Month)
END
GO

