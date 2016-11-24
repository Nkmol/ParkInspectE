/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/


:r .\Seed\Region.sql
:r .\Seed\Client.sql
:r .\Seed\Role.sql
:r .\Seed\State.sql
:r .\Seed\Employee_Status.sql
:r .\Seed\Template.sql
:r .\Seed\Datatype.sql
:r .\Seed\ReportFieldType.sql
:r .\Seed\Contactperson.sql
:r .\Seed\Parkinglot.sql
:r .\Seed\Employee.sql
:r .\Seed\Inspector.sql
:r .\Seed\Assignment.sql
:r .\Seed\Form.sql
:r .\Seed\Inspection.sql
:r .\Seed\Inspector_has_Inspection.sql
:r .\Seed\Absence.sql
:r .\Seed\Report.sql