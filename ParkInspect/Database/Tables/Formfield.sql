CREATE TABLE [dbo].[Formfield]
(
	[form_id] INT NOT NULL , 
    [field_title] VARCHAR(50) NOT NULL, 
    [field_template_id] INT NOT NULL, 
    [value] VARCHAR(50) NULL, 
    CONSTRAINT [FK_Vragenlijstveld_Veld_vregenlijst_id] FOREIGN KEY ([form_id]) REFERENCES Form(id), 
    CONSTRAINT [FK_Questionair_Questionair] FOREIGN KEY ([field_title],[field_template_id]) REFERENCES Field([title],[template_id]), 
    PRIMARY KEY ([form_id], [field_template_id], [field_title]), 
)
