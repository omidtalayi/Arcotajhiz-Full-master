CREATE TABLE [AZ].[UserHistory]
(
	[VCode]					BIGINT			IDENTITY (1, 1) NOT NULL,
    [UserVCode]				BIGINT				NOT NULL,
    [UserHistoryTypeVCode]	TINYINT			NOT NULL,
    [JDate]					DATETIME		NOT NULL CONSTRAINT [DC_UserHistory_JDate] DEFAULT (GETDATE()),
	[Old]					XML				NULL,
	[New]					XML				NULL,
	[LoginTypeVCode]		INT				CONSTRAINT [DC_UserHistory_LoginTypeVCode] DEFAULT ((1)) NOT NULL,
	[AdditionalInfo]		NVARCHAR (1000)	NULL,
    CONSTRAINT [PK_UserHistory] PRIMARY KEY CLUSTERED ([VCode]),
    CONSTRAINT [FK_UserHistory_User] FOREIGN KEY ([UserVCode]) REFERENCES [AZ].[User] ([VCode]),
    CONSTRAINT [FK_UserHistory_UserHistoryType] FOREIGN KEY ([UserHistoryTypeVCode]) REFERENCES [AZ].[UserHistoryType] ([VCode])
)
