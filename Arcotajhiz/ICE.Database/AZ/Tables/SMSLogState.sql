﻿CREATE TABLE [AZ].[SMSLogState](
	[VCode] [int] NOT NULL,
	[Code] [int] NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[EnumName] [varchar](50) NULL,
 CONSTRAINT [PK_SMSLogState] PRIMARY KEY CLUSTERED 
(
	[VCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
