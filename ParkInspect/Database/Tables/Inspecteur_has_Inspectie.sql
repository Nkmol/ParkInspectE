CREATE TABLE [dbo].[Inspecteur_has_Inspectie]
(
	[inspecteur_werknemer_id] INT NOT NULL, 
    [inspectie_id] INT NOT NULL , 
	PRIMARY KEY([inspecteur_werknemer_id],[inspectie_id]),
    CONSTRAINT [FK_Inspecteur_has_Inspectie_Inspecteur] FOREIGN KEY ([inspecteur_werknemer_id]) REFERENCES Werknemer([id]), 
    CONSTRAINT [FK_Inspecteur_has_Inspectie_Inspectie] FOREIGN KEY ([inspectie_id]) REFERENCES Inspectie([id])
)
