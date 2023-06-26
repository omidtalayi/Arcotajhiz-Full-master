CREATE PROCEDURE [AZ].[Admin_PagesIns](
	@Name			NVARCHAR(MAX) = NULL,
	@Body			NVARCHAR(MAX) = NULL,
	@Title			NVARCHAR(MAX) = NULL,
	@PageTypeVCode	SMALLINT	  = 1,
	@Description	NVARCHAR(MAX) = NULL,
	@Link			NVARCHAR(MAX) = NULL,
	@ImageLink		NVARCHAR(MAX) = NULL,
	@Keywords		NVARCHAR(MAX) = NULL,
	@IsActivate		BIT = NULL,
	@EntryUserVCode BIGINT = NULL,
	@Images			XML = NULL,
	@TopicVCode		INT = NULL
)
AS
BEGIN
	DECLARE @PagesVCode INT

	IF @PageTypeVCode <> 1
	BEGIN
		SET @TopicVCode = NULL
	END

	SELECT Field.value('@N','NVARCHAR(500)') [Name]
	INTO #Images FROM @Images.nodes('PI') Details(Field)
	
	INSERT INTO [AZ].[Pages]([Name],[PagesTypeVCode],[Title],[Description],[Keywords],[Body],[Link],[ImageLink],[IsActivate],[IsDeleted],[EntryUserVCode],[TopicVCode])
    VALUES(@Name,@PageTypeVCode,@Title,@Description,@Keywords,@Body,@Link,@ImageLink,@IsActivate,0,@EntryUserVCode,@TopicVCode)

	SET @PagesVCode = SCOPE_IDENTITY()

	INSERT AZ.PagesImage(PagesVCode,[Name])
	SELECT @PagesVCode,[Name] FROM #Images

	DROP TABLE #Images
END
