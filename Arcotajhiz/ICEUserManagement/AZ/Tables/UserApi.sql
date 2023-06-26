﻿CREATE TABLE [AZ].[UserApi]
(
	[VCode]				BIGINT		IDENTITY(1,1)	NOT NULL,
	[UserVCode]			BIGINT		NOT NULL,
	[ApiVCode]			SMALLINT	NOT NULL,
    [EntryDate]			DATETIME	CONSTRAINT [DC_UserApi_EntryDate] DEFAULT(GETDATE()) NOT NULL
    CONSTRAINT [PK_UserApi] PRIMARY KEY CLUSTERED ([VCode] ASC),
	CONSTRAINT [FK_UserApi_Api] FOREIGN KEY ([ApiVCode]) REFERENCES [AZ].[Api]([VCode]),
	CONSTRAINT [FK_UserApi_User] FOREIGN KEY ([UserVCode]) REFERENCES [AZ].[User]([VCode])
)