﻿CREATE TABLE [AZ].[FAQ]
(
	[VCode]					INT IDENTITY(1,1)	NOT NULL,
	[Question]					NVARCHAR(MAX)		NULL,
	[Answer]			NVARCHAR(MAX)		NULL,
	[Link]					NVARCHAR(MAX)		NULL,
	[ImageLink]				NVARCHAR(MAX)		NULL,
	[IsDeleted]				BIT CONSTRAINT [DC_FAQ_IsDeleted] DEFAULT (0) NOT NULL,
	[EntryDate]				DATETIME CONSTRAINT [DC_FAQ_EntryDate] DEFAULT (GETDATE()) NOT NULL,
	[LastModifiedDate]		DATETIME			NULL,
	[EntryUserVCode]		INT					NULL,
	[TopicVCode]			INT					NULL, 
    CONSTRAINT [PK_FAQ] PRIMARY KEY CLUSTERED([VCode] ASC)
)
GO