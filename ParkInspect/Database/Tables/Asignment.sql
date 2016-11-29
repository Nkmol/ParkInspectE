﻿CREATE TABLE [dbo].[Asignment]
(
	[client_id] INT NOT NULL, 
    [id] INT NOT NULL IDENTITY, 
    [date] DATE NULL, 
    [deadline] DATETIME NOT NULL, 
    [state] VARCHAR(45) NOT NULL, 
    [clarification] TEXT NULL, 
	PRIMARY KEY ([client_id],[id]),
    CONSTRAINT [FK_Asignment_Client] FOREIGN KEY ([client_id]) REFERENCES [Client]([id]), 
    CONSTRAINT [FK_Asignment_Status] FOREIGN KEY ([state]) REFERENCES [State]([state]),
	CONSTRAINT [CHK_assignment_date_before_deadline] CHECK ([date] <= [deadline])
)
