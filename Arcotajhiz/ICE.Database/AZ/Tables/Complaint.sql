CREATE TABLE [AZ].[Complaint]
(
	[VCode]					BIGINT				IDENTITY (1, 1) NOT NULL,
	[Title]					NVARCHAR(200)						NOT NULL,
	[FirstName]				NVARCHAR(100)							NULL,
	[LastName]				NVARCHAR(100)							NULL,
	[NationalCode]			NVARCHAR(20)							NULL,
    [Email]					NVARCHAR(100)						NOT NULL,
    [CellPhone]				VARCHAR(15)								NULL,
    [Description]			VARCHAR(MAX)							NULL,
    [EntryDate]				DATETIME CONSTRAINT [DC_Complaint_EntryDate] DEFAULT (GETDATE()) NOT NULL,
	[LastModifiedDate]		DATETIME CONSTRAINT [DC_Complaint_LastModifiedDate] DEFAULT(GETDATE()) NOT NULL,
    [AdminDescription]	 	NVARCHAR(MAX)		 	 		 		NULL, 
    [LastModifiedUserVCode] BIGINT									NULL, 
    CONSTRAINT [PK_Complaint] PRIMARY KEY		CLUSTERED ([VCode] ASC)
)