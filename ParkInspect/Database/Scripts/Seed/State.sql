MERGE INTO dbo.[State] AS Target  
USING (values 
	('In progress'),
	('Finished'),
	('Halted'),
	('Unbegun')
) AS Source ([state])  
ON Target.[state] = Source.[state]  
WHEN NOT MATCHED BY TARGET THEN  
 INSERT ([state])  
 VALUES ([state])  
WHEN MATCHED THEN
 UPDATE SET
  [state] = Source.[state];