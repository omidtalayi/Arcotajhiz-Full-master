CREATE TABLE [AZ].[Identification] (
    [VCode]                             BIGINT         IDENTITY (1, 1) NOT NULL,
    [Cellphone]                         VARCHAR (20)   NOT NULL,
    [NationalCode]                      VARCHAR (12)   NOT NULL,
    [CompanyNationalID]                 VARCHAR (12)   NULL,
    [VerificationCode]                  VARCHAR (6)    NULL,
    [VerificationLink]                  NVARCHAR (MAX) NULL,
    [IsVerified]                        BIT            CONSTRAINT [DC_Identification_IsVerified] DEFAULT ((0)) NOT NULL,
    [HasShahkarIdentified]              BIT            CONSTRAINT [DC_Identification_HasShahkarIdentified] DEFAULT ((0)) NULL,
    [ExpirationDate]                    DATETIME       CONSTRAINT [DC_Identification_ExpirationDate] DEFAULT (getdate()) NOT NULL,
    [IdentificationStateVCode]          SMALLINT       CONSTRAINT [DC_Identification_IdentificationStateVCode] DEFAULT ((1)) NOT NULL,
    [IdentificationTypeVCode]           SMALLINT       CONSTRAINT [DC_Identification_IdentificationTypeVCode] DEFAULT ((1)) NOT NULL,
    [UserVCode]                         INT            NULL,
    [AvailabilityCheck]                 BIT            CONSTRAINT [DC_Identification_AvailabilityCheck] DEFAULT ((0)) NULL,
    [ReportExpirationDate]              DATETIME       CONSTRAINT [DC_Identification_ReportExpirationDate] DEFAULT (getdate()) NOT NULL,
    [ReportLink]                        NVARCHAR (MAX) NULL,
    [PayFromApp]                        BIT            CONSTRAINT [DC_Identification_PayFromApp] DEFAULT ((0)) NOT NULL,
    [HasCheckedKyc]                     BIT            CONSTRAINT [DC_Identification_HasCheckedKyc] DEFAULT ((0)) NULL,
    [UserPaymentTypeVCode]              SMALLINT       CONSTRAINT [DC_Identification_UserPaymentTypeVCode] DEFAULT ((1)) NOT NULL,
    [FromFirmPanel]                     BIT            CONSTRAINT [DC_Identification_FromFirmPanel] DEFAULT ((0)) NOT NULL,
    [IncorrectNationalCodeCount]        SMALLINT       CONSTRAINT [DC_Identification_IncorrectNationalCodeCount] DEFAULT ((0)) NOT NULL,
    [SecondCellphone]                   VARCHAR (20)   NULL,
    [SecondCellphoneIsVerified]         BIT            CONSTRAINT [DC_Identification_SecondCellphoneIsVerified] DEFAULT ((0)) NOT NULL,
    [SecondCellphoneVerificationCode]   VARCHAR (6)    NULL,
    [SecondCellphoneExpirationDate]     DATETIME       CONSTRAINT [DC_Identification_SecondCellphoneExpirationDate] DEFAULT (getdate()) NOT NULL,
    [SecondCellphoneHasCheckedKyc]      BIT            CONSTRAINT [DC_Identification_SecondCellphoneHasCheckedKyc] DEFAULT ((0)) NULL,
    [ReceiverCellphone]                 VARCHAR (20)   NULL,
    [ReceiverCellphoneVerificationCode] VARCHAR (6)    NULL,
    [ReceiverCellphoneExpirationDate]   DATETIME       CONSTRAINT [DC_Identification_ReceiverCellphoneExpirationDate] DEFAULT (getdate()) NOT NULL,
    [ReceiverCellphoneIsVerified]       BIT            CONSTRAINT [DC_Identification_ReceiverCellphoneIsVerified] DEFAULT ((0)) NOT NULL,
    [SendSmsTryCount]                   SMALLINT       CONSTRAINT [DC_Identification_SendSmsTryCount] DEFAULT ((0)) NOT NULL,
    [SiteHasBeenRepaired]               BIT            CONSTRAINT [DC_Identification_SiteHasBeenRepaired] DEFAULT ((0)) NOT NULL,
    [ReportSuccessfullySent]            BIT            CONSTRAINT [DC_Identification_ReportSuccessfullySent] DEFAULT ((0)) NOT NULL,
    [IsLegalPerson]                     BIT            CONSTRAINT [DC_Identification_IsLegalPerson] DEFAULT ((0)) NULL,
    [RedirectUrlICS24]                  NVARCHAR (MAX) NULL,
    [IdICS24]                           NVARCHAR (200) NULL,
    [SendSmsPeygiri]                    BIT            CONSTRAINT [DC_Identification_SendSmsPeygiri] DEFAULT ((0)) NULL,
    [IsFromSendSmsPeygiri]              BIT            CONSTRAINT [DC_Identification_IsFromSendSmsPeygiri] DEFAULT ((0)) NULL,
    [IsPendingICS24Service]             BIT            CONSTRAINT [DC_Identification_IsPendingICS24Service] DEFAULT ((0)) NULL,
    [IsRepairMessageSent]               BIT            CONSTRAINT [DC_Identification_IsRepairMessageSent] DEFAULT ((0)) NULL,
    [AppIcs24HashCode]                  NVARCHAR (MAX) NULL,
    [IdentificationComplaintStateVCode] SMALLINT       NULL,
    [TrackingId]                        NVARCHAR (MAX) NULL,
    [InvitationVCode]                   BIGINT         NULL,
    [IsPaidByInvitations]               BIT            CONSTRAINT [DC_Identification_IsPaidByInvitations] DEFAULT ((0)) NULL,
    [IsRepairedSupport]                 BIT            CONSTRAINT [DC_Identification_IsRepairedSupport] DEFAULT ((0)) NULL,
    [FromApp]                           BIT            CONSTRAINT [DC_Identification_FromApp] DEFAULT ((0)) NULL,
    [Rate]                              DECIMAL (5, 2) NULL,
    [EntryDate]                         DATETIME       CONSTRAINT [DC_Identification_EntryDate] DEFAULT (getdate()) NOT NULL,
    [LastModifiedDate]                  DATETIME       CONSTRAINT [DC_Identification_LastModifiedDate] DEFAULT (getdate()) NOT NULL,
    [GUID]                              NVARCHAR (50)  NULL,
    [ReportIsCorrupted]                 BIT            CONSTRAINT [DF_Identification_ReportIsCorrupted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Identification] PRIMARY KEY CLUSTERED ([VCode] ASC),
    CONSTRAINT [FK_Identification_IdentificationComplaintState] FOREIGN KEY ([IdentificationComplaintStateVCode]) REFERENCES [AZ].[IdentificationComplaintState] ([VCode]),
    CONSTRAINT [FK_Identification_IdentificationState] FOREIGN KEY ([IdentificationStateVCode]) REFERENCES [AZ].[IdentificationState] ([VCode]),
    CONSTRAINT [FK_Identification_IdentificationType] FOREIGN KEY ([IdentificationTypeVCode]) REFERENCES [AZ].[IdentificationType] ([VCode])
);
GO

CREATE NONCLUSTERED INDEX [IX_Identification_IdentificationStateVCode]
    ON [AZ].[Identification]([IdentificationStateVCode] ASC)
    INCLUDE([InvitationVCode]);
GO

CREATE NONCLUSTERED INDEX [IX_Identification_IdentificationTypeVCode]
    ON [AZ].[Identification]([IdentificationTypeVCode] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_Identification_UserPaymentTypeVCode]
    ON [AZ].[Identification]([UserPaymentTypeVCode] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_Identification_UserVCode]
    ON [AZ].[Identification]([UserVCode] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_Identification_UserVCode_EntryDate]
    ON [AZ].[Identification]([UserVCode] ASC, [EntryDate] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_IdentificationStateVCode_InvitationVCode]
    ON [AZ].[Identification]([IdentificationStateVCode] ASC, [InvitationVCode] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_Identification_EntryDate]
    ON [AZ].[Identification]([EntryDate] ASC);
GO