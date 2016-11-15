CREATE TABLE [dbo].[Veld]
(
	[titel] VARCHAR(50) NOT NULL, 
    [template_id] INT NOT NULL, 
    [datatype] VARCHAR(50) NOT NULL, 
    [rapportVeldType_titel] VARCHAR(50) NULL, 
	PRIMARY KEY ([titel],[template_id]),
    CONSTRAINT [FK_Veld_Template] FOREIGN KEY ([template_id]) REFERENCES Template([id]), 
    CONSTRAINT [FK_Veld_Datatype] FOREIGN KEY ([datatype]) REFERENCES Datatype([datatype]), 
    CONSTRAINT [FK_Veld_RapportVeldType] FOREIGN KEY ([rapportVeldType_titel]) REFERENCES RapportVeldType([titel])
)
