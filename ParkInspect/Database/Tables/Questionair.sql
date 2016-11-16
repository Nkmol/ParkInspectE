CREATE TABLE [dbo].[Questionair]
(
	[id] INT NOT NULL PRIMARY KEY, 
    [template_id] INT NOT NULL, 
    CONSTRAINT [FK_Questionair_Template] FOREIGN KEY (template_id) REFERENCES Template([id])
)
