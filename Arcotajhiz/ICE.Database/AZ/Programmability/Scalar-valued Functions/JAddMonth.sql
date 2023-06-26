CREATE FUNCTION [AZ].[JAddMonth](
	@JDate CHAR(8),
	@JMonthOff SMALLINT
) 
RETURNS CHAR(8) AS   
BEGIN  
	DECLARE @JYear CHAR(4),
			@JMonth CHAR(2),
			@JDay CHAR(2),
			@JMonthParry INT,
			@Result CHAR(8)

	IF @JMonthOff < 0 
	BEGIN 
		DECLARE @Temp SMALLINT 
		SET @Temp = -1 * @JMonthOff 
		SET @Result = [AZ].[JSubMonth](@JDate,@Temp) 
	END 
	ELSE 
	BEGIN 
		IF Len(@JDate) = 6 AND SUBSTRING(@JDate,1,2) > 10  SET @JDate = '13' + @JDate  
        ELSE IF Len(@JDate) = 6 AND SUBSTRING(@JDate,1,2) <= 10  SET @JDate = '14' + @JDate  

		SET @JYear = LEFT(@JDate,4) 
		SET @JMonth = SUBSTRING(@JDate,5,2) 
		SET @JDay = RIGHT(@JDate,2) 
		SET @JMonth = CAST((CAST(@JMonth AS SMALLINT) + @JMonthOff % 12) AS CHAR(2)) 
		SET @JMonthParry = (CAST(@JMonth AS SMALLINT) - 1) / 12 
		SET @JMonth = CAST((CAST(@JMonth AS SMALLINT) % 12) AS CHAR(2)) 

		If CAST(@JMonth AS INT) <= 0 
		BEGIN 
			SET @JMonth = '12' 
		END 

		SET @JYear = CAST(((CAST(@JYear AS SMALLINT) + @JMonthOff / 12) + @JMonthParry) AS CHAR(4)) 

		If CAST(@JDay AS Int) > AZ.JDayTab(AZ.Jleap(@JYear),@JMonth) 
		BEGIN
			SET @JDay = AZ.JDayTab(AZ.Jleap(@JYear),@JMonth) 
		END

		IF LEN(@JMonth) = 1 
		BEGIN
			SET @JMonth = '0' + @JMonth 
		END

		IF LEN(@JDay) = 1 
		BEGIN
			SET @JDay = '0' + @JDay 
		END

		SET @Result = CAST((@JYear + @JMonth + @JDay) AS CHAR(8)) 
	END 

	RETURN @Result 
END
GO

