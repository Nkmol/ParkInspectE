CREATE TABLE [dbo].[Form]
(
	[id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [template_id] INT NOT NULL, 
    CONSTRAINT [FK_Form_Template] FOREIGN KEY (template_id) REFERENCES Template([id])
)
