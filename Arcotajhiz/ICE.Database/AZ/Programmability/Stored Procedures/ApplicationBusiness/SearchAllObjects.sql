create Proc	[AZ].[SearchAllObjects] (@Text NVARCHAR(4000))
AS



-- Get the schema name, table name, and table type for:

-- Table names
SELECT
      TABLE_SCHEMA  AS 'Object Schema',
      TABLE_NAME    AS 'Object Name',
      TABLE_TYPE    AS 'Object Type',
      'Table Name'  AS 'TEXT Location'
FROM  INFORMATION_SCHEMA.TABLES
WHERE TABLE_NAME LIKE '%'+@Text+'%'

UNION

 --Column names

SELECT
      TABLE_SCHEMA   AS 'Object Schema',
      COLUMN_NAME   AS 'Object Name',
      'COLUMN'      AS 'Object Type',
      'Column Name' AS 'TEXT Location'
FROM  INFORMATION_SCHEMA.COLUMNS
WHERE COLUMN_NAME LIKE '%'+@Text+'%'

--UNION

-- Function or procedure bodies

SELECT DISTINCT 
       o.name AS Object_Name,
       o.type_desc
  FROM sys.sql_modules m 
       INNER JOIN 
       sys.objects o 
         ON m.object_id = o.object_id
 WHERE m.definition Like '%'+@Text+'%';





--===================================================


--SELECT
--      SPECIFIC_SCHEMA     AS 'Object Schema',
--      ROUTINE_NAME       AS 'Object Name',
--      ROUTINE_TYPE       AS 'Object Type',
--      ROUTINE_DEFINITION AS 'TEXT Location'
--FROM  INFORMATION_SCHEMA.ROUTINES 
--WHERE ROUTINE_DEFINITION LIKE '%'+@Text+'%'
--      AND (ROUTINE_TYPE = 'function' OR ROUTINE_TYPE = 'procedure');









--========================================
--Column Values Search
--========================================
SELECT C.name AS Column_Name , T.name AS Table_Name 
FROM sys.columns AS C
INNER JOIN sys.tables AS T
ON C.object_id = T.object_id
WHERE c.name like '%'+@Text+'%'
GO

