﻿CREATE TABLE [dbo].[Template]
(
	[id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [name] VARCHAR(50) NULL, 
    [version_number] VARCHAR(50) NULL,
	UNIQUE([name])
)
