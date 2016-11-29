CREATE TABLE [dbo].[Inspector]
(
	[employee_id] INT NOT NULL PRIMARY KEY, 
    [region_name] VARCHAR(50) NOT NULL , 
    CONSTRAINT [FK_Inspector_Region] FOREIGN KEY ([region_name]) REFERENCES Region([name]),
	CONSTRAINT [FK_Inspector_Employee] FOREIGN KEY ([employee_id]) REFERENCES Employee([id])
)
