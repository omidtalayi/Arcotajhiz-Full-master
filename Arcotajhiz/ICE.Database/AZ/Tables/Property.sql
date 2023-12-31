﻿CREATE TABLE [AZ].[Property]
(
	[id] UNIQUEIDENTIFIER DEFAULT NEWSEQUENTIALID() PRIMARY KEY, 
    [name] NCHAR(50) NOT NULL, 
    [defaultValue] NCHAR(50) NULL, 
    [createDate] DATE DEFAULT(GETDATE()) NOT NULL,
    [lastModifiedDate] DATE DEFAULT(GETDATE()) NOT NULL, 
    [isDeleted] BIT NOT NULL DEFAULT 0, 
    [isEnabled] BIT NOT NULL DEFAULT 1,
)
