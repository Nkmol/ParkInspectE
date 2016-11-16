CREATE TABLE [dbo].[QuestionairField]
(
	[questionair_id] INT NOT NULL PRIMARY KEY, 
    [field_title] VARCHAR(50) NOT NULL, 
    [field_template_id] INT NOT NULL, 
    [value] VARCHAR(50) NULL, 
    CONSTRAINT [FK_Vragenlijstveld_Veld_vregenlijst_id] FOREIGN KEY (questionair_id) REFERENCES Questionair(id), 
    CONSTRAINT [FK_Questionair_Questionair] FOREIGN KEY ([field_title],[field_template_id]) REFERENCES Field([title],[template_id]), 
)
