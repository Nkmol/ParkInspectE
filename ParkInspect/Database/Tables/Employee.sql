CREATE TABLE [dbo].[Employee]
(
	[id] INT NOT NULL PRIMARY KEY, 
    [employee_status] VARCHAR(50) NOT NULL, 
    [job] VARCHAR(50) NOT NULL, 
    [firstname] VARCHAR(50) NOT NULL, 
    [lastname] VARCHAR(50) NOT NULL, 
    [active] BIT NOT NULL, 
    [phonenumber] VARCHAR(50) NULL, 
    [in-service-date] DATETIME NOT NULL, 
    [out-service-date] DATETIME NULL, 
    [email] VARCHAR(50) NOT NULL, 
    CONSTRAINT [FK_Employee_Job] FOREIGN KEY (job) REFERENCES Job([job]), 
    CONSTRAINT [FK_Employee_Employee_status] FOREIGN KEY (employee_status) REFERENCES Employee_status(employee_status)
)
