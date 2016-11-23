MERGE INTO dbo.Client AS Target  
USING (values 
	('Scheepspark BV', '050-3127656', 'contact@scheepspark.nl'),
	('Rozet', '026-3775900', 'contact@rozet.nl'),
	('Noorderwerf', '0519-234339', 'contact@noorderwerf.nl'),
	('DeHavenParkeerplaats', '0519-332439', 'contact@dhp.nl'),
	('Wederzand', '0519-343423', 'contact@wederzand.nl'),
	('HelloCar', '0519-342342', 'contact@hellocar.nl'),
	('Duifpark', '0519-746432', 'contact@duifpark.nl')
) AS Source (name, phonenumber, email)  
ON Target.name = Source.name  
WHEN NOT MATCHED BY TARGET THEN  
 INSERT (name, phonenumber, email)  
 VALUES (name, phonenumber, email)  
WHEN MATCHED THEN
 UPDATE SET
  Name = Source.Name;