﻿CREATE TABLE [AZ].[ApprovalState]
(
	[VCode]	   SMALLINT		NOT NULL,
	[Code]     SMALLINT		NOT NULL,
	[Name]	   VARCHAR(100) NOT NULL,
	[EnumName] VARCHAR(100) NOT NULL,
	CONSTRAINT [PK_ApprovalState] PRIMARY KEY CLUSTERED ([VCode] ASC)
)

