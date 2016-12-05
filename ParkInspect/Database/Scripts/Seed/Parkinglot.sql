MERGE INTO dbo.Parkinglot AS Target  
USING (values 
	('Utrecht', '3510 VK', 83, 'Parkeergarage Springweg', 'Vaak geen ruimte'),
	('Utrecht', '3511 EN', 22, 'Parkeergarage Moreelsepark', 'Vaak wel ruimte'),
	('Utrecht', '3511 BR', 1, 'Parkeergarage La Vie', 'Vaak geen ruimte'),

	('Noord-Brabant', '5211 XT', 119, 'Parkeergarage Arena', 'Vaak geen ruimte'),
	('Noord-Brabant', '5211 VZ', 25, 'Parkeergarage Emmaplein', 'Vaak geen ruimte'),
	('Noord-Brabant', '5211 SV', 28, 'Parkeergarage Barbaraplaats', 'Vaak geen ruimte')


) AS Source (region_name, zipcode, number, name, clarification)  
ON Target.name = Source.name 
WHEN NOT MATCHED BY TARGET THEN  
 INSERT (region_name, zipcode, number, name, clarification)  
 VALUES (region_name, zipcode, number, name, clarification)  
WHEN MATCHED THEN
 UPDATE SET
  name = Source.name;
