﻿CREATE TABLE [AZ].[UserIP]
(
	[VCode]		BIGINT		NOT NULL IDENTITY(1,1),
	[UserVCode] BIGINT		NOT NULL,
	[IP]		VARCHAR(50) NOT NULL,
	[EntryDate]	DATETIME	CONSTRAINT [DC_UserIP_EntryDate] DEFAULT (GETDATE()) NOT NULL,
    CONSTRAINT [PK_UserIP] PRIMARY KEY CLUSTERED ([VCode]),
	CONSTRAINT [FK_UserIP_User] FOREIGN KEY ([UserVCode]) REFERENCES [AZ].[User]([VCode])
)