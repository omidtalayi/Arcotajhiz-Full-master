CREATE TABLE [AZ].[Pages](
	[id] UNIQUEIDENTIFIER DEFAULT NEWSEQUENTIALID() PRIMARY KEY, 
	[Name]					NVARCHAR(MAX)		NULL,
	[PagesTypeId]		SMALLINT			NULL,
	[Title]					NVARCHAR(MAX)		NULL,
	[Image]				NVARCHAR(MAX)		NULL,
	[Description]			NVARCHAR(MAX)		NULL,
	[Body]					NVARCHAR(MAX)		NULL,
	[Link]					NVARCHAR(MAX)		NULL,
	[ImageLink]				NVARCHAR(MAX)		NULL,
	[Keywords]				NVARCHAR(MAX)		NULL,
	[IsActivate]			BIT CONSTRAINT [DC_Pages_IsActivate] DEFAULT (1) NOT NULL,
	[IsDeleted]				BIT CONSTRAINT [DC_Pages_IsDeleted] DEFAULT (0) NOT NULL,
	[EntryDate]				DATETIME CONSTRAINT [DC_Pages_EntryDate] DEFAULT (GETDATE()) NOT NULL,
	[LastModifiedDate]		DATETIME			NULL,
	[EntryUserVCode]		INT					NULL,
	[LastModifiedUserVCode] INT					NULL,
	[TopicVCode]			INT					NULL 
 --   CONSTRAINT [PK_Page] PRIMARY KEY CLUSTERED([id] ASC),
	--CONSTRAINT [FK_Pages_PagesType] FOREIGN KEY ([PagesTypeVCode]) REFERENCES [AZ].[PagesType]([VCode])
)
GO

