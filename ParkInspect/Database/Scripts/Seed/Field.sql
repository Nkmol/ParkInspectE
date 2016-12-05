MERGE INTO dbo.Field AS Target  
USING (values 
	('ExampleField1', 1, 'String', 'Textbox'),
	('ExampleField2', 1, 'Boolean', 'Checkbox'),
	('ExampleField3', 1, 'Integer', 'Combobox'),
	('ExampleField4', 2, 'String', 'Textbox'),
	('ExampleField5', 2, 'Boolean', 'Checkbox'),
	('ExampleField6', 2, 'Integer', 'Combobox')
) AS Source (title, template_id, datatype, reportFieldType_title)  
ON Target.title = Source.title  
WHEN NOT MATCHED BY TARGET THEN  
 INSERT (title, template_id, datatype, reportFieldType_title)  
 VALUES (title, template_id, datatype, reportFieldType_title)  
WHEN MATCHED THEN
 UPDATE SET
  title = Source.title;