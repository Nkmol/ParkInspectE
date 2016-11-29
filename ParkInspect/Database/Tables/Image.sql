CREATE TABLE [dbo].[Image]
(
	[image] VARCHAR(50) NOT NULL, 
    [inspection_id] INT NOT NULL, 
	PRIMARY KEY([inspection_id],[image]),
    CONSTRAINT [FK_Image_Raport] FOREIGN KEY ([inspection_id]) REFERENCES [Report]([inspection_id])
)
