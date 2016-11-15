CREATE TABLE [dbo].[Vragenlijstveld]
(
	[vragenlijst_id] INT NOT NULL PRIMARY KEY, 
    [veld_titel] VARCHAR(50) NOT NULL, 
    [veld_template_id] INT NOT NULL, 
    [waarde] VARCHAR(50) NULL, 
    CONSTRAINT [FK_Vragenlijstveld_Veld_vregenlijst_id] FOREIGN KEY (vragenlijst_id) REFERENCES Vragenlijst(id), 
    CONSTRAINT [FK_Vragenlijstveld_Vragenlijst] FOREIGN KEY ([veld_titel],[veld_template_id]) REFERENCES Veld([titel],[template_id]), 
)
