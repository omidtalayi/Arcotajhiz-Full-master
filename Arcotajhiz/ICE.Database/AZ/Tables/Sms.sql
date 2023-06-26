CREATE TABLE [AZ].[Sms]
(
	[VCode]					BIGINT		  NOT NULL IDENTITY(1,1),
	[IdentificationVCode]	BIGINT		  NOT NULL,
	[Message]				VARCHAR(MAX)  NULL,
	[Cellphone]				VARCHAR(20)   NULL,
	[SMSProviderID]			BIGINT		  NULL,
	[EntryDate]				DATETIME	  NOT NULL CONSTRAINT [DC_Sms_EntryDate] DEFAULT (GETDATE()),
	CONSTRAINT [PK_Sms] PRIMARY KEY CLUSTERED([VCode] ASC),
	CONSTRAINT [FK_Sms_Identification] FOREIGN KEY([IdentificationVCode]) REFERENCES [AZ].[Identification]([VCode])
)
GO
