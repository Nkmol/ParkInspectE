CREATE TABLE [dbo].[Inspecteur]
(
	[werknemer_id] INT NOT NULL PRIMARY KEY, 
    [regio_naam] VARCHAR(50) NOT NULL , 
    CONSTRAINT [FK_Inspecteur_Regio] FOREIGN KEY ([regio_naam]) REFERENCES Regio([naam]),
	CONSTRAINT [FK_Inspecteur_Werknemer] FOREIGN KEY (werknemer_id) REFERENCES Werknemer([id])
)
