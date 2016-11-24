MERGE INTO dbo.Region AS Target  
USING (values 
	('Groningen'),
	('Friesland'),
	('Drenthe'),
	('Overijssel'),
	('Flevoland'),
	('Gelderland'),
	('Utrecht'),
	('Noord-Holland'),	
	('Zuid-Holland'),
	('Zeeland'),	
	('Noord-Brabant'),
	('Limburg')
) AS Source (Name)  
ON Target.Name = Source.Name  
WHEN NOT MATCHED BY TARGET THEN  
 INSERT (Name)  
 VALUES (Name)  
WHEN MATCHED THEN
 UPDATE SET
  Name = Source.Name;