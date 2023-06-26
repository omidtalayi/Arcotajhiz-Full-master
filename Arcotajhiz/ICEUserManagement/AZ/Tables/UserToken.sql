CREATE TABLE [AZ].[UserToken]
(
	[VCode]						BIGINT			NOT NULL IDENTITY(1,1),
	[UserVCode]					BIGINT			NOT NULL,
	[Token]						VARCHAR(1000)	NOT NULL,	
	[TokenExpirationDate]		DATETIME		NOT NULL,
	[IP]						VARCHAR(20)		NULL,
	[DeviceTypeVCode]			SMALLINT		NULL,
	[SecretCode]				VARCHAR(500)	NULL,
	[DeviceId]					NVARCHAR(200)   NULL,
	[EntryDate]					DATETIME        CONSTRAINT [DC_UserToken_EntryDate] DEFAULT (GETDATE()) NOT NULL,
	CONSTRAINT [PK_UserToken] PRIMARY KEY CLUSTERED ([VCode]),
	CONSTRAINT [FK_UserToken_User] FOREIGN KEY ([UserVCode]) REFERENCES [AZ].[User] ([VCode]),
	CONSTRAINT [FK_UserToken_DeviceType] FOREIGN KEY ([DeviceTypeVCode]) REFERENCES [AZ].[DeviceType] ([VCode])
)
