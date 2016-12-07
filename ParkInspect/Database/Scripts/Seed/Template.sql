MERGE INTO dbo.Template AS Target  
USING (values 
	('Basic safety', '1.0'),
	('Intermediate security', '1.0'),
	('Advanced goverment check', '1.0')
) AS Source (name, version_number)  
ON Target.name = Source.name  
WHEN NOT MATCHED BY TARGET THEN  
 INSERT (name, version_number)  
 VALUES (name, version_number)  
WHEN MATCHED THEN
 UPDATE SET
  name = Source.name;