CREATE TABLE [dbo].[Employee]
(
	[id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [employee_status] VARCHAR(50) NOT NULL, 
    [role] VARCHAR(50) NOT NULL, 
    [firstname] VARCHAR(50) NOT NULL, 
    [lastname] VARCHAR(50) NOT NULL, 
    [active] BIT NOT NULL, 
    [phonenumber] VARCHAR(20) NULL, 
    [in_service_date] DATETIME NOT NULL, 
    [out_service_date] DATETIME NULL, 
    [email] VARCHAR(50) NOT NULL, 
    [password] VARCHAR(50) NULL, 
    CONSTRAINT [FK_Employee_Job] FOREIGN KEY ([role]) REFERENCES [Role]([role]), 
    CONSTRAINT [FK_Employee_Employee_status] FOREIGN KEY (employee_status) REFERENCES Employee_status(employee_status)
)
