CREATE TABLE [AZ].[Role]
(
	[VCode]				INT				NOT NULL,
	[Code]				INT				NOT NULL,
	[Name]				NVARCHAR(50)    NOT NULL,
	[EnumName]			NVARCHAR(50)	NOT NULL,
	[SubSystemVCode]	SMALLINT		CONSTRAINT [DC_Role_SubSystemVCode] DEFAULT(3) NOT NULL,
    CONSTRAINT [PK_Role] PRIMARY KEY ([VCode]),
	CONSTRAINT [FK_Role_SubSystem] FOREIGN KEY ([SubSystemVCode]) REFERENCES [AZ].[SubSystem]([VCode])
)