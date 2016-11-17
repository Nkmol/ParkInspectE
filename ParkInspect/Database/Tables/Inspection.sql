CREATE TABLE [dbo].[Inspection]
(
	[id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [parking_id] INT NOT NULL, 
    [form_id] INT NULL, 
    [follow_up_id] INT NULL, 
    [state] VARCHAR(45) NOT NULL, 
    [deadline] DATETIME NULL, 
    [date] DATETIME NULL, 
    [clarification] TEXT NULL, 
    [assigment_client_id] INT NOT NULL, 
    [assignment_id] INT NOT NULL, 
    CONSTRAINT [FK_Inspectie_Parkeerplaats] FOREIGN KEY ([parking_id]) REFERENCES [Parkinglot]([id]), 
    CONSTRAINT [FK_Inspectie_Vragenlijst] FOREIGN KEY ([form_id]) REFERENCES [Questionair]([id]), 
    CONSTRAINT [FK_Inspectie_Inspectie] FOREIGN KEY ([follow_up_id]) REFERENCES [Inspection]([id]), 
    CONSTRAINT [FK_Inspectie_Status] FOREIGN KEY ([state]) REFERENCES [State]([state]), 
    CONSTRAINT [FK_Inspectie_Opdracht] FOREIGN KEY ([assigment_client_id], [assignment_id]) REFERENCES [Asignment]([client_id], [id]),
)
