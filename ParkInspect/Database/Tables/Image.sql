CREATE TABLE [dbo].[Image]
(
	[image] NTEXT NOT NULL, 
    [form_id] INT NOT NULL, 
    CONSTRAINT [FK_Image_Raport] FOREIGN KEY ([form_id]) REFERENCES [Form]([id]), 
    CONSTRAINT [PK_Image] PRIMARY KEY ([form_id])
)
