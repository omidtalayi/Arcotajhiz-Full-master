CREATE FUNCTION [AZ].[Jleap](@Year_Renamed Int)
RETURNS INT AS
BEGIN 
	DECLARE @tmp Int
	DECLARE @JLEAP int

	SET @tmp = @Year_Renamed % 33
	SET @JLEAP = CASE WHEN @tmp = 1 Or @tmp = 5 Or @tmp = 9 Or @tmp = 13 Or @tmp = 17 Or @tmp = 22 Or @tmp = 26 Or @tmp = 30 THEN 1 ELSE 0 END
	RETURN @JLEAP
END
GO

