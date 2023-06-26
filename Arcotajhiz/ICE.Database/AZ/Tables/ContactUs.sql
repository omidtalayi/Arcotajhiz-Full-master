CREATE TABLE [AZ].[ContactUs]
(
	[VCode]					BIGINT				IDENTITY (1, 1) NOT NULL,
	[Title]					NVARCHAR(100)							NULL,
	[FullName]				NVARCHAR(50)						NOT NULL,
    [Email]					NVARCHAR(100)						NOT NULL,
    [CellPhone]				VARCHAR(15)								NULL,
    [Description]			VARCHAR(MAX)							NULL,
    [EntryDate]				DATETIME CONSTRAINT [DC_ContactUs_EntryDate] DEFAULT (GETDATE()) NOT NULL,
	[LastModifiedDate]		DATETIME CONSTRAINT [DC_ContactUs_LastModifiedDate] DEFAULT(GETDATE()) NOT NULL,
    [AdminDescription]		NVARCHAR(MAX)							NULL, 
    [LastModifiedUserVCode] BIGINT									NULL, 
    CONSTRAINT [PK_ContactUs] PRIMARY KEY		CLUSTERED ([VCode] ASC)
)
