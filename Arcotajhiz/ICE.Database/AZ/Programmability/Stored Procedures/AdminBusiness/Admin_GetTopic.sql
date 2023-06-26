CREATE PROCEDURE AZ.Admin_GetTopic
AS
BEGIN
	SELECT 
		T.VCode,
		T.[Name] 
	FROM AZ.Topic T
	WHERE T.IsEnable = 1
END