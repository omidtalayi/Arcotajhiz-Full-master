CREATE TABLE [AZ].[UserSubSystem]
(
	[VCode]				BIGINT			IDENTITY(1,1)	NOT NULL,
	[UserVCode]			BIGINT			NOT NULL,
	[SubSystemVCode]	SMALLINT		NOT NULL,
    [EntryDate]			DATETIME		CONSTRAINT [DC_UserSubSystem_EntryDate] DEFAULT(GETDATE()) NOT NULL
    CONSTRAINT [PK_UserSubSystem] PRIMARY KEY CLUSTERED ([VCode] ASC),
	CONSTRAINT [FK_UserSubSystem_SubSystem] FOREIGN KEY ([SubSystemVCode]) REFERENCES [AZ].[SubSystem]([VCode]),
	CONSTRAINT [FK_UserSubSystem_User] FOREIGN KEY ([UserVCode]) REFERENCES [AZ].[User]([VCode])
)
