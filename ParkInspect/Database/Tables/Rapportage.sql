CREATE TABLE [dbo].[Rapportage]
(
	[inspectie_id] INT NOT NULL PRIMARY KEY, 
    [aantekeningen] TEXT NULL, 
    CONSTRAINT [FK_Rapportage_Inspectie] FOREIGN KEY ([inspectie_id]) REFERENCES [Inspectie]([id])
)
