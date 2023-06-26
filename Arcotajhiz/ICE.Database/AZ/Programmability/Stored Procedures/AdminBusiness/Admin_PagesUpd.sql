CREATE PROCEDURE [AZ].[Admin_PagesUpd](
	@VCode			INT = NULL,
	@Name			NVARCHAR(MAX) = NULL,
	@Title			NVARCHAR(MAX) = NULL,
	@PageTypeVCode	SMALLINT	 = NULL,
	@Description	NVARCHAR(MAX) = NULL,
	@Keywords		NVARCHAR(MAX) = NULL,
	@Body			NVARCHAR(MAX) = NULL,
	@Link			NVARCHAR(MAX) = NULL,
	@ImageLink		NVARCHAR(MAX) = NULL,
	@IsActivate		BIT = NULL,
	@TopicVCode		INT = NULL
)
AS
BEGIN
		IF @PageTypeVCode <> 1
		BEGIN
			SET @TopicVCode = NULL
		END

		UPDATE AZ.Pages
		SET	[Name] = ISNULL(@Name,[Name]),
			[Title] = ISNULL(@Title,[Title]),
			[PagesTypeVCode] = ISNULL(@PageTypeVCode,[PagesTypeVCode]),
			[Description] =ISNULL(@Description,[Description]),
			[Body] = ISNULL(@Body,[Body]),
			[Link] = ISNULL(@Link,[Link]),
			[ImageLink] = ISNULL(@ImageLink,[ImageLink]),
			[IsActivate] = ISNULL(@IsActivate,[IsActivate]),
			[TopicVCode] = ISNULL(@TopicVCode,[TopicVCode]),
			[Keywords] = ISNULL(@Keywords,[Keywords]),
			[LastModifiedDate] = GETDATE()
		WHERE VCode = @VCode
END