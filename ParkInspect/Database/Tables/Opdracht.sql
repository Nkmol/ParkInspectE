CREATE TABLE [dbo].[Opdracht]
(
	[klant_id] INT NOT NULL, 
    [id] INT NOT NULL, 
    [datum] DATE NULL, 
    [deadine] DATETIME NOT NULL, 
    [status] VARCHAR(45) NOT NULL, 
    [toelichting] TEXT NULL, 
	PRIMARY KEY ([klant_id],[id]),
    CONSTRAINT [FK_Opdracht_Klant] FOREIGN KEY ([klant_id]) REFERENCES [Klant]([id]), 
    CONSTRAINT [FK_Opdracht_Status] FOREIGN KEY ([status]) REFERENCES [Status]([status])
)
