CREATE TABLE [AZ].[UserWebHook]
(
	[VCode]				BIGINT			IDENTITY(1,1)	NOT NULL,
	[UserVCode]			BIGINT			NOT NULL,
	[WebHookTypeVCode]	SMALLINT		NOT NULL,
	[WebHook]			VARCHAR(MAX)	NOT NULL,
    [EntryDate]			DATETIME		CONSTRAINT [DC_UserWebHook_EntryDate] DEFAULT(GETDATE()) NOT NULL
    CONSTRAINT [PK_UserWebHook] PRIMARY KEY CLUSTERED ([VCode] ASC),
	CONSTRAINT [FK_UserWebHook_WebHookType] FOREIGN KEY ([WebHookTypeVCode]) REFERENCES [AZ].[WebHookType]([VCode]),
	CONSTRAINT [FK_UserWebHook_User] FOREIGN KEY ([UserVCode]) REFERENCES [AZ].[User]([VCode])
)
