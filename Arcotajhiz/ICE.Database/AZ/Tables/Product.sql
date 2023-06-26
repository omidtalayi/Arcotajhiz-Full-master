CREATE TABLE [AZ].[Product]
(
	[id] UNIQUEIDENTIFIER DEFAULT NEWSEQUENTIALID() PRIMARY KEY, 
    [name] NVARCHAR(100) NULL, 
    [description] NVARCHAR(MAX) NULL, 
    [image] NVARCHAR(MAX) NULL, 
    [price] DECIMAL NULL, 
    [categoryID] UNIQUEIDENTIFIER NULL, 
    [isEnabled] BIT NOT NULL DEFAULT 1, 
    [isDeleted] BIT NOT NULL DEFAULT 0, 
    [isSpecialed] BIT NOT NULL DEFAULT 0, 
    [score] DECIMAL NULL, 
    [createDate] DATETIME CONSTRAINT [DC_Product_createDate] DEFAULT(GETDATE())  NOT NULL,
    [lastModifiedDate] DATE DEFAULT(GETDATE()) NOT NULL,

)

--CREATE NONCLUSTERED INDEX [IX_Product_createDate]
--    ON [AZ].[Product]([createDate] ASC);
--GO
