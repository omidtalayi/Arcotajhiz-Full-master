CREATE PROCEDURE AZ.GetSettings(@Kies XML = NULL) AS
BEGIN
	IF @Kies IS NULL
	BEGIN
		SELECT ApplicationSetting.[Key],[Value],[Description],VCode FROM AZ.ApplicationSetting (NOLOCK) 
	END
	ELSE
	BEGIN
		SELECT ApplicationSetting.[Key],[Value],[Description],VCode FROM AZ.ApplicationSetting (NOLOCK) 
		INNER JOIN (SELECT Element.value('@V','VARCHAR(4000)') [Key] FROM @Kies.nodes('K') Kies(Element)) Kies
			ON ApplicationSetting.[Key] = Kies.[Key]
	END
	
END