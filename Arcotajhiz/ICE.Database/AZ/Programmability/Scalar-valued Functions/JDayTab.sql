CREATE FUNCTION [AZ].[JDayTab](
    @Leap INT,
    @Month INT
)
RETURNS INT AS  
BEGIN 
    DECLARE @X int

    SET @X = 
        CASE    WHEN @Month = 0 THEN 0 
                WHEN @Month > 0 AND @Month < 7 THEN 31
 	            WHEN @Month >= 7 AND @Month < 12 THEN 30 
 		ELSE 
            CASE @Leap WHEN 0 THEN 29 ELSE 30 END
 	    END
    RETURN @X
END
GO

