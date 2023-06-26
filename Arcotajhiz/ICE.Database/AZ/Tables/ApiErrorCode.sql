CREATE TABLE [AZ].[ApiErrorCode]
(
	[VCode]					SMALLINT		NOT NULL,
	[ErrorCode]				SMALLINT        NOT NULL,
	[ErrorMessage]			NVARCHAR (500)   NOT NULL,
	[ErrorEnglishMessage]	VARCHAR (500)	NOT NULL
    CONSTRAINT [PK_ApiErrorCode] PRIMARY KEY ([VCode])
)