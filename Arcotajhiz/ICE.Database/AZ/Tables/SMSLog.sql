CREATE TABLE [AZ].[SMSLog] (
    [VCode]               BIGINT         IDENTITY (1, 1) NOT NULL,
    [SendID]              BIGINT         NULL,
    [SendFromNumber]      VARCHAR (50)   NULL,
    [SendToNumber]        VARCHAR (50)   NULL,
    [SendMessage]         NVARCHAR (MAX) NULL,
    [SendState]           SMALLINT       NULL,
    [SendDelivery]        VARCHAR (50)   NULL,
    [IdentificationVCode] BIGINT         NULL,
    [SMSLogTypeVCode]     SMALLINT       NULL,
    [EntryDate]           DATETIME       CONSTRAINT [DF_SMSLog_EntryDate] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_SMSLog] PRIMARY KEY CLUSTERED ([VCode] ASC),
    CONSTRAINT [FK_SMSLog_SMSLogType] FOREIGN KEY ([SMSLogTypeVCode]) REFERENCES [AZ].[SMSLogType] ([VCode])
);
GO

