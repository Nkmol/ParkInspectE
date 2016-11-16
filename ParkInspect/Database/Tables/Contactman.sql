CREATE TABLE [dbo].[Contactman]
(
	[id] INT NOT NULL PRIMARY KEY, 
    [client_id] INT NOT NULL, 
    [firstname] VARCHAR(50) NOT NULL, 
    [lastname] VARCHAR(50) NOT NULL, 
    CONSTRAINT [FK_Client_Contactman] FOREIGN KEY ([client_id]) REFERENCES Client([id])
)
