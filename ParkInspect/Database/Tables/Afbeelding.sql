CREATE TABLE [dbo].[Afbeelding]
(
	[afbeelding] VARCHAR(50) NOT NULL, 
    [rapportage_inspectie_id] INT NOT NULL, 
	PRIMARY KEY([rapportage_inspectie_id],[afbeelding]),
    CONSTRAINT [FK_Afbeelding_Rapportage] FOREIGN KEY ([rapportage_inspectie_id]) REFERENCES [Rapportage]([inspectie_id])
)
