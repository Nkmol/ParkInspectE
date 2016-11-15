CREATE TABLE [dbo].[Vragenlijst]
(
	[id] INT NOT NULL PRIMARY KEY, 
    [template_id] INT NOT NULL, 
    CONSTRAINT [FK_Vragenlijst_Template] FOREIGN KEY (template_id) REFERENCES Template([id])
)
