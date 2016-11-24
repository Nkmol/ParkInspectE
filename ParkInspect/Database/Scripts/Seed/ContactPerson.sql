MERGE INTO dbo.Contactperson AS Target  
USING (values 
	(1, 'Henk', 'Huize'),
	(2, 'Jos', 'Brinks'),
	(3, 'Herman', 'Kate'),
	(4, 'Fred', 'Versteeg')
) AS Source (client_id, firstname, lastname)  
ON Target.client_id = Source.client_id 
WHEN NOT MATCHED BY TARGET THEN  
 INSERT (client_id, firstname, lastname)  
 VALUES (client_id, firstname, lastname)  
WHEN MATCHED THEN
 UPDATE SET
  client_id = Source.client_id;
