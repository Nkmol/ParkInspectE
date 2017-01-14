MERGE INTO dbo.Client AS Target  
USING (values 
	('Scheepspark BV', '050-3127656', 'contact@scheepspark.nl', 'schip'),
	('Rozet', '026-3775900', 'contact@rozet.nl', 'roos'),
	('Noorderwerf', '0519-234339', 'contact@noorderwerf.nl', 'werf'),
	('DeHavenParkeerplaats', '0519-332439', 'contact@dhp.nl', 'haven'),
	('Wederzand', '0519-343423', 'contact@wederzand.nl', 'zand'),
	('HelloCar', '0519-342342', 'contact@hellocar.nl', 'car'),
	('Duifpark', '0519-746432', 'contact@duifpark.nl', 'duif')
) AS Source ([name], [phonenumber], [email], [password])  
ON Target.[name] = Source.[name]  
WHEN NOT MATCHED BY TARGET THEN  
 INSERT ([name], [phonenumber], [email], [password])  
 VALUES ([name], [phonenumber], [email], [password])  
WHEN MATCHED THEN
 UPDATE SET
  [Name] = Source.[Name];