CREATE TABLE [AZ].[UserWebHookCall]
(
	[VCode]					BIGINT		NOT NULL IDENTITY(1,1),
	[UserVCode]				BIGINT		NOT NULL,
	[UserWebHookVCode]		BIGINT		NOT NULL,
	[IdentificationVCode]	BIGINT		NOT NULL,
	[EntryDate]				DATETIME	CONSTRAINT [DC_UserWebHookCall_EntryDate] DEFAULT (GETDATE()) NOT NULL,
    CONSTRAINT [PK_UserWebHookCall] PRIMARY KEY CLUSTERED ([VCode]),
	CONSTRAINT [FK_UserWebHookCall_User] FOREIGN KEY ([UserVCode]) REFERENCES [AZ].[User]([VCode]),
	CONSTRAINT [FK_UserWebHookCall_UserWebHook] FOREIGN KEY ([UserWebHookVCode]) REFERENCES [AZ].[UserWebHook]([VCode])
)
