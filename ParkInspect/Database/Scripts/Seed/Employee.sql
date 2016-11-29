MERGE INTO dbo.Employee AS Target  
USING (values 
	('Available', 'Employee', 'Henk', 'Kate', 1, '06-14354341', '10-11-2014', null, 'h.k@parkinspect.nl', 'aap'),
	('Available', 'Inspector', 'Liza', 'Vermeer', 1, '06-23456522', '10-11-2013', null, 'l.v@parkinspect.nl', 'huis'),
	('Retired', 'Manager', 'Robin', 'Groen', 0, '06-26534372', '10-11-2008', '10-11-2016', 'r.g@parkinspect.nl', 'steen'),
	('Available', 'Manager', 'Harold', 'Richter', 1, '06-18454232', '10-11-2016', null, 'h.r@parkinspect.nl', 'rots'),
	('Available', 'Inspector', 'Richard', 'van der Pas', 1, '06-13541332', '10-11-2015', null, 'r.vnp@parkinspect.nl', 'ketel'),
	('Available', 'Inspector', 'Marianne', 'van Laarhoven', 1, '06-7644334', '10-11-2011', null, 'm.vl@parkinspect.nl', 'wit'),
	('Available', 'Inspector', 'Bas', 'Von Fillet', 1, '06-97834234', '10-11-2011', null, 'b.vf@parkinspect.nl', 'lasagna'),
	('Available', 'Inspector', 'Stefan', 'Broeks', 1, '06-79868764', '10-11-2012', null, 's.b@parkinspect.nl', 'test123'),
	('Available', 'Inspector', 'Justus', 'Kampwegen', 1, '06-08797845', '10-11-2012', null, 'j.k@parkinspect.nl', 'abc123'),
	('Available', 'Employee', 'Cedric', 'Almond', 1, '06-9997634', '10-11-2012', null, 'c.a@parkinspect.nl', 'abc123'),
	('Available', 'Employee', 'Bob', 'Huizes', 1, '06-456566464', '10-11-2014', null, 'b.h@parkinspect.nl', 'abc123'),
	('Available', 'Employee', 'Jordi', 'Bencks', 1, '06-943543', '10-11-2013', null, 'j.b@parkinspect.nl', 'abc123'),
	('Available', 'Inspector', 'Bram', 'Potjes', 1, '06-7644334', '10-11-2010', null, 'b.pt@parkinspect.nl', 'wit'),
	('Available', 'Inspector', 'Jasper', 'Rozen', 1, '06-97834234', '10-11-2010', null, 'j.r@parkinspect.nl', 'lasagna'),
	('Available', 'Inspector', 'Thomas', 'Bosch', 1, '06-79868764', '10-11-2010', null, 't.b@parkinspect.nl', 'test123')
) AS Source ([employee_status], [role], [firstname], [lastname], [active], [phonenumber], [in_service_date], [out_service_date], [email], [password])  
ON Target.[firstname] = Source.[firstname] 
WHEN NOT MATCHED BY TARGET THEN  
 INSERT ([employee_status], [role], [firstname], [lastname], [active], [phonenumber], [in_service_date], [out_service_date], [email], [password])  
 VALUES ([employee_status], [role], [firstname], [lastname], [active], [phonenumber], [in_service_date], [out_service_date], [email], [password])  
WHEN MATCHED THEN
 UPDATE SET
  [employee_status] = Source.[employee_status];
