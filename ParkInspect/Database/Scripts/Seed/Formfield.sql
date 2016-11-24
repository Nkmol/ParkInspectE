MERGE INTO dbo.Formfield AS Target  
USING (values 
	(1, 'ExampleField1', 1, 'There are no missing exits'),
	(1, 'ExampleField2', 1, 'true'),
	(1, 'ExampleField3', 1, '42'),
	(2, 'ExampleField4', 2, 'Several cars are parked at the wrong side'),
	(2, 'ExampleField5', 2, 'false'),
	(2, 'ExampleField6', 2, '2')
) AS Source ([form_id], [field_title], [field_template_id], [value])  
ON Target.[field_title] = Source.[field_title]  
WHEN NOT MATCHED BY TARGET THEN  
 INSERT ([form_id], [field_title], [field_template_id], [value])  
 VALUES ([form_id], [field_title], [field_template_id], [value])  
WHEN MATCHED THEN
 UPDATE SET
  [field_title] = Source.[field_title];