CREATE TABLE [dbo].[Werknemer]
(
	[id] INT NOT NULL PRIMARY KEY, 
    [werknemer_status] VARCHAR(50) NOT NULL, 
    [rol] VARCHAR(50) NOT NULL, 
    [voornaam] VARCHAR(50) NOT NULL, 
    [achternaam] VARCHAR(50) NOT NULL, 
    [actief] BIT NOT NULL, 
    [telefoonnummer] VARCHAR(50) NULL, 
    [in-dienst-datum] DATETIME NOT NULL, 
    [uit-dienst-datum] DATETIME NULL, 
    [email] VARCHAR(50) NOT NULL, 
    CONSTRAINT [FK_Werknemer_Rol] FOREIGN KEY (rol) REFERENCES Rol(rol), 
    CONSTRAINT [FK_Werknemer_werknemer_status] FOREIGN KEY (werknemer_status) REFERENCES Werknemer_status(werknemer_status)
)
