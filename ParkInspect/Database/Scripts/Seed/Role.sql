MERGE INTO dbo.[Role] AS Target  
USING (values 
	('Inspecteur'),
	('Manager'),
	('Werknemer')
) AS Source ([role])  
ON Target.[role] = Source.[role]  
WHEN NOT MATCHED BY TARGET THEN  
 INSERT ([role])  
 VALUES ([role])  
WHEN MATCHED THEN
 UPDATE SET
  [role] = Source.[role];