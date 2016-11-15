CREATE TABLE [dbo].[Inspectie]
(
	[id] INT NOT NULL PRIMARY KEY, 
    [parkeerplaats_id] INT NOT NULL, 
    [vragenlijst_id] INT NULL, 
    [vervolg_inspectie_id] INT NULL, 
    [status] VARCHAR(45) NOT NULL, 
    [deadline] DATETIME NULL, 
    [datum] DATETIME NULL, 
    [toelichting] NCHAR(10) NULL, 
    [opdracht_klant_id] INT NOT NULL, 
    [opdracht_id] INT NOT NULL, 
    CONSTRAINT [FK_Inspectie_Parkeerplaats] FOREIGN KEY ([parkeerplaats_id]) REFERENCES [Parkeerplaats]([id]), 
    CONSTRAINT [FK_Inspectie_Vragenlijst] FOREIGN KEY ([vragenlijst_id]) REFERENCES [Vragenlijst]([id]), 
    CONSTRAINT [FK_Inspectie_Inspectie] FOREIGN KEY ([vervolg_inspectie_id]) REFERENCES [Inspectie]([id]), 
    CONSTRAINT [FK_Inspectie_Status] FOREIGN KEY ([status]) REFERENCES [Status]([status]), 
    CONSTRAINT [FK_Inspectie_Opdracht] FOREIGN KEY ([opdracht_klant_id], [opdracht_id]) REFERENCES [Opdracht]([klant_id], [id]),
)
