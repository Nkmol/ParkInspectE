CREATE TABLE [dbo].[Contactperson]
(
	[id] INT NOT NULL IDENTITY , 
    [client_id] INT NOT NULL, 
    [firstname] VARCHAR(50) NOT NULL, 
    [lastname] VARCHAR(50) NOT NULL, 
    CONSTRAINT [FK_Client_Contactperson] FOREIGN KEY ([client_id]) REFERENCES Client([id]), 
    CONSTRAINT [PK_Contactperson] PRIMARY KEY ([id]) 
)
