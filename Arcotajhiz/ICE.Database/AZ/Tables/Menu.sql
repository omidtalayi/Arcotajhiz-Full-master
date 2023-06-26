﻿CREATE TABLE [AZ].[Menu](
	[id] UNIQUEIDENTIFIER DEFAULT NEWSEQUENTIALID() PRIMARY KEY, 
	[Title]				[NVARCHAR](max)			NULL,
	[Name]				[NVARCHAR](max)			NULL,
	[Link]				[NVARCHAR](MAX)			NULL,
	[ParentVCode]		[INT]					NOT NULL,
	[IsDeleted]			[BIT]					NULL,
	[IsEnable]			[BIT]					NULL,
	[EntryDate]			[DATETIME]				NULL,
	[LastModifiedDate]	[DATETIME]				NULL,
CONSTRAINT [PK_Menu] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)
WITH 
(
	PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
	ALTER TABLE [AZ].[Menu] ADD  CONSTRAINT [DF_Menu_EntryDate]  DEFAULT (getdate()) FOR [EntryDate]
GO

