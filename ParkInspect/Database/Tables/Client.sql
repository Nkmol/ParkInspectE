CREATE TABLE [dbo].[Client]
(
	[id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [name] VARCHAR(50) NOT NULL, 
    [phonenumber] VARCHAR(20) NOT NULL, 
    [email] VARCHAR(50) NOT NULL, 
    [password] NVARCHAR(MAX) NULL
)
