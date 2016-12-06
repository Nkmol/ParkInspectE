MERGE INTO dbo.Inspector AS Target  
USING (values 
	(2, 'Groningen'),
	(5, 'Friesland'),
	(6, 'Drenthe'),
	(7, 'Overijssel'),
	(8, 'Flevoland'),
	(9, 'Gelderland'),
	(13,'Utrecht'),
	(14,'Noord-Holland'),		
	(15,'Noord-Brabant')
) AS Source (employee_id, region_name)  
ON Target.employee_id = Source.employee_id  
WHEN NOT MATCHED BY TARGET THEN  
 INSERT (employee_id, region_name)  
 VALUES (employee_id, region_name)  
WHEN MATCHED THEN
 UPDATE SET
  employee_id = Source.employee_id;