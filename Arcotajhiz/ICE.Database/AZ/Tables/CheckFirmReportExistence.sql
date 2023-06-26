CREATE TABLE [AZ].[CheckFirmReportExistence]
(
	[VCode]				BIGINT			IDENTITY (1,1) NOT NULL,
	[UserVCode]			BIGINT			NOT NULL,
	[NationalCode]		NVARCHAR(20)	NOT NULL,
	[CompanyNationalID]	NVARCHAR(20)	NOT NULL,
	[Cellphone]			NVARCHAR(20)	NOT NULL,
	[IP]				NVARCHAR(100)	NULL,
	[EntryDate]			DATETIME		CONSTRAINT [DC_CheckFirmReportExistence_EntryDate] DEFAULT (GETDATE()) NOT NULL,
	CONSTRAINT [PK_CheckFirmReportExistence] PRIMARY KEY CLUSTERED ([VCode] ASC)
)
GO
CREATE NONCLUSTERED INDEX [IX_CheckFirmReportExistence_UserVCode]
    ON [AZ].[CheckFirmReportExistence]([UserVCode] ASC)
GO