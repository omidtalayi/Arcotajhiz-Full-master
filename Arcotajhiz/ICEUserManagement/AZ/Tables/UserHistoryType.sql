CREATE TABLE [AZ].[UserHistoryType]
(
	[VCode] TINYINT		  NOT NULL, 
    [Name]  NVARCHAR (50) NOT NULL,
	CONSTRAINT [PK_UserHistoryType] PRIMARY KEY CLUSTERED ([VCode])
);