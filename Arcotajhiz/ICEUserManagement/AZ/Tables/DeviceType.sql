CREATE TABLE [AZ].[DeviceType]
(
	[VCode]		SMALLINT		NOT NULL,
	[Code]		SMALLINT        NOT NULL,
	[Name]		NVARCHAR (50)   NOT NULL,
	[EnumName]	NVARCHAR (50)	NOT NULL,
    CONSTRAINT [PK_DeviceType] PRIMARY KEY ([VCode])
)