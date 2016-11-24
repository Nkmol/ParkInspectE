MERGE INTO dbo.Datatype AS Target  
USING (values 
	('Text'),
	('Integer'),
	('Boolean'),
	('Double'), 
	('Date'),
	('Time')
) AS Source (datatype)  
ON Target.datatype = Source.datatype  
WHEN NOT MATCHED BY TARGET THEN  
 INSERT (datatype)  
 VALUES (datatype)  
WHEN MATCHED THEN
 UPDATE SET
  datatype = Source.datatype;
