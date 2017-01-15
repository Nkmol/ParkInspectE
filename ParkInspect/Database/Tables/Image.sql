CREATE TABLE [dbo].[Image]
(
	[image] VARCHAR(50) NOT NULL, 
    [form_id] INT NOT NULL, 
	PRIMARY KEY([form_id],[image]),
    CONSTRAINT [FK_Image_Raport] FOREIGN KEY ([form_id]) REFERENCES [Form]([id])
)
