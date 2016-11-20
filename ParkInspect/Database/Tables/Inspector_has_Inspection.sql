CREATE TABLE [dbo].[Inspector_has_Inspection]
(
	[employee_id] INT NOT NULL, 
    [inspection_id] INT NOT NULL , 
	PRIMARY KEY([employee_id],[inspection_id]),
    CONSTRAINT [FK_Inspecteur_has_Inspectie_Inspector] FOREIGN KEY ([employee_id]) REFERENCES Employee([id]), 
    CONSTRAINT [FK_Inspecteur_has_Inspectie_Inspection] FOREIGN KEY ([inspection_id]) REFERENCES Inspection([id])
)
