MERGE INTO dbo.ReportFieldType AS Target  
USING (values 
	('Textbox'),
	('Checkbox'),
	('Datepicker'),
	('Combobox')
) AS Source (title)  
ON Target.title = Source.title  
WHEN NOT MATCHED BY TARGET THEN  
 INSERT (title)  
 VALUES (title)  
WHEN MATCHED THEN
 UPDATE SET
  title = Source.title;
