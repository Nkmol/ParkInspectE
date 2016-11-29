MERGE INTO dbo.Absence AS Target  
USING (values 
	('2016-11-26 09:00:00', 7, '2016-11-26 17:00:00'),
	('2016-12-04 09:00:00', 8, null),
	('2016-12-07 09:00:00', 9, '2016-12-10 09:00:00')
) AS Source ([start], [employee_id], [end])  
ON Target.[employee_id] = Source.[employee_id] 
WHEN NOT MATCHED BY TARGET THEN  
 INSERT ([start], [employee_id], [end])  
 VALUES ([start], [employee_id], [end])  
WHEN MATCHED THEN
 UPDATE SET
  [employee_id] = Source.[employee_id];