CREATE TABLE [dbo].[Raport]
(
	[inspection_id] INT NOT NULL PRIMARY KEY, 
    [extra] TEXT NULL, 
    CONSTRAINT [FK_Raport_Inspection] FOREIGN KEY ([inspection_id]) REFERENCES [Inspection]([id])
)
