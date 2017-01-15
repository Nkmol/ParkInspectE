CREATE TABLE [dbo].[Report]
(
	[inspection_id] INT NOT NULL PRIMARY KEY, 
    [clarifaction] TEXT NULL, 
    [pdf] NTEXT NULL, 
    CONSTRAINT [FK_Raport_Inspection] FOREIGN KEY ([inspection_id]) REFERENCES [Inspection]([id])
)
