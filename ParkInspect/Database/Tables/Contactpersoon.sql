CREATE TABLE [dbo].[Contactpersoon]
(
	[id] INT NOT NULL PRIMARY KEY, 
    [klant_id] INT NOT NULL, 
    [voornaam] VARCHAR(50) NOT NULL, 
    [achternaam] VARCHAR(50) NOT NULL, 
    CONSTRAINT [FK_Contactpersoon_Klant] FOREIGN KEY ([klant_id]) REFERENCES Klant([id])
)
