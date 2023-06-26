CREATE TABLE [AZ].[PagesComment] (
    [Id]                  UNIQUEIDENTIFIER  DEFAULT NEWSEQUENTIALID() PRIMARY KEY, 
    [ParentId]            UNIQUEIDENTIFIER         NULL,
    [PagesId]             UNIQUEIDENTIFIER         NULL,
    [ProductId]           UNIQUEIDENTIFIER         NULL,
    [ApprovalStateVCode]  SMALLINT CONSTRAINT [DC_PagesComment_ApprovalStateVCode] DEFAULT ((1)) NOT NULL,
    [Name]                NVARCHAR (MAX) NOT NULL,
    [Message]             NVARCHAR (MAX) NOT NULL,
    [Email]               NVARCHAR (MAX) NULL,
    [Website]             NVARCHAR (MAX) NULL,
    [Cellphone]           NVARCHAR (11)  NULL,
    [EntryDate]           DATETIME       CONSTRAINT [DC_PagesComment_EntryDate] DEFAULT (getdate()) NOT NULL,
    [LastModifiedDate]    DATETIME       NULL,
    CONSTRAINT [PK_PagesComment] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_PagesComment_ApprovalState] FOREIGN KEY ([ApprovalStateVCode]) REFERENCES [AZ].[ApprovalState] ([VCode]),
    CONSTRAINT [FK_PagesComment_Pages] FOREIGN KEY ([PagesId]) REFERENCES [AZ].[Pages] ([id]),
    CONSTRAINT [FK_PagesComment_PagesComment] FOREIGN KEY ([ParentId]) REFERENCES [AZ].[PagesComment] ([Id])
);
GO