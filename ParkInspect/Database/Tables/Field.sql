CREATE TABLE [dbo].[Field]
(
	[title] VARCHAR(50) NOT NULL, 
    [template_id] INT NOT NULL, 
    [datatype] VARCHAR(50) NOT NULL, 
    [reportFieldType_title] VARCHAR(50) NULL, 
	PRIMARY KEY ([title],[template_id]),
    CONSTRAINT [FK_Field_Template] FOREIGN KEY ([template_id]) REFERENCES Template([id]), 
    CONSTRAINT [FK_Field_Datatype] FOREIGN KEY ([datatype]) REFERENCES Datatype([datatype]), 
    CONSTRAINT [FK_Field_RapportFieldType] FOREIGN KEY ([reportFieldType_title]) REFERENCES ReportFieldType([title])
)
