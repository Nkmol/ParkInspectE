MERGE INTO dbo.Form AS Target  
USING (values 
	(1),
	(2),
	(3)
) AS Source (template_id)  
ON Target.template_id = Source.template_id  
WHEN NOT MATCHED BY TARGET THEN  
 INSERT (template_id)  
 VALUES (template_id)  
WHEN MATCHED THEN
 UPDATE SET
  template_id = Source.template_id;