MERGE INTO dbo.Report AS Target  
USING (values 
	(3, 'This is a finished report on the following conducts')
) AS Source (inspection_id, clarifaction)  
ON Target.inspection_id = Source.inspection_id  
WHEN NOT MATCHED BY TARGET THEN  
 INSERT (inspection_id, clarifaction)  
 VALUES (inspection_id, clarifaction)  
WHEN MATCHED THEN
 UPDATE SET
  inspection_id = Source.inspection_id;