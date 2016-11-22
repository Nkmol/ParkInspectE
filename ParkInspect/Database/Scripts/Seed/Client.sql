MERGE INTO dbo.Client AS Target  
USING (values 
	('Scheepspark BV', '050-3127656', 'contact@scheepspark.nl'),
	('Rozet', '026-3775900', 'contact@rozet.nl'),
	('Noorderwerf', '0519-349139', 'contact@noorderwerf.nl')
) AS Source (name, phonenumber, email)  
ON Target.name = Source.name  
WHEN NOT MATCHED BY TARGET THEN  
 INSERT (name, phonenumber, email)  
 VALUES (name, phonenumber, email)  
WHEN MATCHED THEN
 UPDATE SET
  Name = Source.Name;