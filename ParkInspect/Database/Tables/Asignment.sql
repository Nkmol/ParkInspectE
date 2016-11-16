CREATE TABLE [dbo].[Asignment]
(
	[client_id] INT NOT NULL, 
    [id] INT NOT NULL, 
    [date] DATE NULL, 
    [deadine] DATETIME NOT NULL, 
    [status] VARCHAR(45) NOT NULL, 
    [extra] TEXT NULL, 
	PRIMARY KEY ([client_id],[id]),
    CONSTRAINT [FK_Asignment_Client] FOREIGN KEY ([client_id]) REFERENCES [Client]([id]), 
    CONSTRAINT [FK_Asignment_Status] FOREIGN KEY ([status]) REFERENCES [Status]([status])
)
