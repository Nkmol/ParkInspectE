MERGE INTO dbo.Employee_Status AS Target  
USING (values 
	('Available'),
	('On Non-Pay Leave'),
	('Retired'),
	('Terminated'),
	('Suspended')
) AS Source (employee_status)  
ON Target.employee_status = Source.employee_status  
WHEN NOT MATCHED BY TARGET THEN  
 INSERT (employee_status)  
 VALUES (employee_status)  
WHEN MATCHED THEN
 UPDATE SET
  employee_status = Source.employee_status;