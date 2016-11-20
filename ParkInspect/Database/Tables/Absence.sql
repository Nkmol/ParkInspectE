CREATE TABLE [dbo].[Absence]
(
	[start] DATETIME NOT NULL, 
    [employee_id] INT NOT NULL, 
    [end] DATETIME NULL, 
	PRIMARY KEY ([start], [employee_id]),
    CONSTRAINT [FK_Absence_Employee] FOREIGN KEY ([employee_id]) REFERENCES [Employee]([id])
)
