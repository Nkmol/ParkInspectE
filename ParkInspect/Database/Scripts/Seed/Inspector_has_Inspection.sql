MERGE INTO dbo.Inspector_has_Inspection AS Target  
USING (values 
	(13, 1),
	(15, 3)
) AS Source (employee_id, inspection_id)  
ON Target.employee_id = Source.employee_id  
WHEN NOT MATCHED BY TARGET THEN  
 INSERT (employee_id, inspection_id)  
 VALUES (employee_id, inspection_id)  
WHEN MATCHED THEN
 UPDATE SET
  employee_id = Source.employee_id;