CREATE TABLE [dbo].[Absence]
(
	[start_date] DATETIME NOT NULL, 
    [employee_id] INT NOT NULL, 
    [end_date] DATETIME NULL, 
	PRIMARY KEY ([start_date], [employee_id]),
    CONSTRAINT [FK_Absence_Employee] FOREIGN KEY ([employee_id]) REFERENCES [Employee]([id])
)
