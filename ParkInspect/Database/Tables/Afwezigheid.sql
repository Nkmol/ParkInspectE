CREATE TABLE [dbo].[Afwezigheid]
(
	[start] DATETIME NOT NULL, 
    [werknemer_id] INT NOT NULL, 
    [einde] DATETIME NULL, 
	PRIMARY KEY ([start], [werknemer_id]),
    CONSTRAINT [FK_Afwezigheid_Werknemer] FOREIGN KEY ([werknemer_id]) REFERENCES [Werknemer]([id])
)
