CREATE TABLE [AZ].[Application] (
    [VCode]                     INT            NOT NULL,
    [Name]                      NVARCHAR (100) NOT NULL,
    [DefaultUserActive]         BIT            CONSTRAINT [DC_Application_DefaultUserActive] DEFAULT ((1)) NOT NULL,
    [DefaultUserApprove]        BIT            CONSTRAINT [DC_Application_DefaultUserApprove] DEFAULT ((0)) NOT NULL,
    [FailedLoginAttemptCount]   TINYINT        CONSTRAINT [DC_Application_FailedLoginAttemptCount] DEFAULT ((5)) NOT NULL,
    [UnlockingUserTimeInMinute] TINYINT        CONSTRAINT [DC_Application_UnlockingUserTimeInMinute] DEFAULT ((5)) NOT NULL,
    [EntryDate]                 DATETIME       CONSTRAINT [DC_Application_EntryDate] DEFAULT (GETDATE()) NOT NULL,
    [SMSSendingLimit]           SMALLINT       CONSTRAINT [DC_Application_SMSSendingLimit] DEFAULT ((0)) NOT NULL,
    [EmailSendingLimit]         SMALLINT       CONSTRAINT [DC_Application_EmailSendingLimit] DEFAULT ((0)) NOT NULL,
    [PasswordResetLimit]        SMALLINT       CONSTRAINT [DC_Application_PasswordResetLimit] DEFAULT ((0)) NOT NULL,
	[SMSCodeDuration]           SMALLINT       CONSTRAINT [DC_Application_SMSCodeDuration] DEFAULT ((180)) NOT NULL,
    [EmailCodeDuration]         SMALLINT       CONSTRAINT [DC_Application_EmailCodeDuration] DEFAULT ((180)) NOT NULL,
    CONSTRAINT [PK_Application] PRIMARY KEY CLUSTERED ([VCode] ASC)
);