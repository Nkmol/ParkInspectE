MERGE INTO dbo.[Asignment] AS Target  
USING (values 
	(1, '2016-11-24', '2016-11-27 10:00:00', 'Unbegun', 'Watch out for the pitbulls outside the parkingspaces'),
	(2, '2016-11-22', '2017-01-02 08:00:00', 'Unbegun', 'Please report back to the owner after this'),
	(3, '2016-11-26', '2017-02-14 08:30:00', 'Unbegun', 'This is a singular assignment'),
	(4, '2016-11-28', '2016-03-15 14:15:00', 'Halted', 'Payment not received yet'),
	(5, '2016-11-14', '2016-04-01 10:00:00', 'Finished', 'Owner was satisfied')

) AS Source ([client_id], [date], [deadine], [state], [clarification])  
ON Target.[client_id] = Source.[client_id] 
WHEN NOT MATCHED BY TARGET THEN  
 INSERT ([client_id], [date], [deadine], [state], [clarification])  
 VALUES ([client_id], [date], [deadine], [state], [clarification])  
WHEN MATCHED THEN
 UPDATE SET
  [client_id] = Source.[client_id];