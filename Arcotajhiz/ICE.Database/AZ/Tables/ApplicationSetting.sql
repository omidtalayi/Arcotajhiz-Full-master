CREATE TABLE [AZ].[ApplicationSetting]
(
	[VCode]			INT				NOT NULL,
    [Key]			VARCHAR (100)	NOT NULL, 
    [Value]			VARCHAR (4000)	NOT NULL, 
    [Description]	VARCHAR (500)	NULL, 
	[EntryDate]		DATETIME		NOT NULL CONSTRAINT [DC_ApplicationSetting_EntryDate] DEFAULT (GETDATE()),
    CONSTRAINT [PK_ApplicationSetting] PRIMARY KEY ([VCode])
)
GO