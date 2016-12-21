CREATE TABLE [dbo].[Parkinglot]
(
	[id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [region_name] VARCHAR(50) NULL, 
    [zipcode] VARCHAR(10) NULL, 
    [number] VARCHAR(50) NULL, 
    [name] VARCHAR(50) NULL, 
    [streetname] VARCHAR(50) NULL, 
    [clarification] TEXT NOT NULL, 
    CONSTRAINT [FK_Parkinglot_Region] FOREIGN KEY ([region_name]) REFERENCES [Region]([name]),
	UNIQUE([name])
)
