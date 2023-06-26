CREATE TABLE [AZ].[UserPaymentType]
(
	[VCode]		SMALLINT		NOT NULL,
	[Code]		SMALLINT        NOT NULL,
	[Name]		NVARCHAR(50)    NOT NULL,
	[EnumName]	NVARCHAR(50)	NOT NULL,
    CONSTRAINT [PK_UserPaymentType] PRIMARY KEY ([VCode])
)