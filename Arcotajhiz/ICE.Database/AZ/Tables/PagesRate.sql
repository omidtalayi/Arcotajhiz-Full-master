CREATE TABLE [AZ].[PagesRate]
(
	[VCode]					BIGINT IDENTITY(1,1)	NOT NULL,
	[PagesVCode]			INT						NOT NULL,
	[Rate]					DECIMAL(5,2)			NOT NULL,
	[IP]					NVARCHAR(20)			NULL,
	[SessionID]				NVARCHAR(100)			NOT NULL,
	[EntryDate]				DATETIME CONSTRAINT [DC_PagesRate_EntryDate] DEFAULT (GETDATE()) NOT NULL,
	[LastModifiedDate]		DATETIME				NULL
    CONSTRAINT [PK_PagesRate] PRIMARY KEY CLUSTERED([VCode] ASC),
	CONSTRAINT [FK_PagesRate_Pages] FOREIGN KEY ([PagesVCode]) REFERENCES [AZ].[Pages]([id])
)
