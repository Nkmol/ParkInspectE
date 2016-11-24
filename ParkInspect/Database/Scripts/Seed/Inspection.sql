MERGE INTO dbo.Inspection AS Target  
USING (values 
	(1, 1, null, 'Unbegun', '2016-11-26 10:00:00', '2016-11-24','1 out of 2 that needs checking for the client', 1, 1),
	(2, 1, null, 'Unbegun', '2016-11-29 11:00:00', '2016-11-25','Very dear client of us', 2, 2),
	(4, 1, null, 'Finished', '2016-11-24 10:00:00', '2016-11-24','Takes more time due circumstance', 3, 3)

) AS Source ([parking_id], [form_id], [follow_up_id], [state], [deadline], [date], [clarification], [assigment_client_id], [assignment_id])  
ON Target.[assignment_id] = Source.[assignment_id] 
WHEN NOT MATCHED BY TARGET THEN  
 INSERT ([parking_id], [form_id], [follow_up_id], [state], [deadline], [date], [clarification], [assigment_client_id], [assignment_id])  
 VALUES ([parking_id], [form_id], [follow_up_id], [state], [deadline], [date], [clarification], [assigment_client_id], [assignment_id])  
WHEN MATCHED THEN
 UPDATE SET
  [assignment_id] = Source.[assignment_id];