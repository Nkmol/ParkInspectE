CREATE TABLE [dbo].[Parkeerplaats]
(
	[id] INT NOT NULL PRIMARY KEY, 
    [regio_naam] VARCHAR(50) NULL, 
    [postcode] VARCHAR(50) NULL, 
    [nummer] INT NULL, 
    [naam] VARCHAR(50) NULL, 
    [toelichting] TEXT NOT NULL, 
    CONSTRAINT [FK_Parkeerplaats_Regio] FOREIGN KEY ([regio_naam]) REFERENCES [Regio]([naam])
)
