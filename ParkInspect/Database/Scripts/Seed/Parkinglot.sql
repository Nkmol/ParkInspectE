MERGE INTO dbo.Parkinglot AS Target  
USING (values 
	('Utrecht', '3510 VK', 83, 'Parkeergarage Springweg','Strogsteeg', 'Vaak geen ruimte'),
	('Utrecht', '3511 EN', 22, 'Parkeergarage Moreelsepark','Spoorstraat', 'Vaak wel ruimte'),
	('Utrecht', '3511 BR', 1, 'Parkeergarage La Vie','Sint-Jacobsstraat', 'Vaak geen ruimte'),

	('Noord-Brabant', '5211 XT', 119, 'Parkeergarage Arena','Arena', 'Vaak geen ruimte'),
	('Noord-Brabant', '5211 VZ', 25, 'Parkeergarage Emmaplein','Emmaplein', 'Vaak geen ruimte'),
	('Noord-Brabant', '5211 SV', 28, 'Parkeergarage Barbaraplaats','Orthenstraat', 'Vaak geen ruimte')


) AS Source (region_name, zipcode, number, name, streetname, clarification)  
ON Target.name = Source.name 
WHEN NOT MATCHED BY TARGET THEN  
 INSERT (region_name, zipcode, number, name, streetname, clarification)  
 VALUES (region_name, zipcode, number, name, streetname, clarification)  
WHEN MATCHED THEN
 UPDATE SET
  name = Source.name;
