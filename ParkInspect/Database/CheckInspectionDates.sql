CREATE FUNCTION [dbo].[CheckInspectionDates]
(
	@assignment_id int,
	@datestart datetime,
	@deadline datetime
)
RETURNS BIT
AS
BEGIN
	DECLARE @assignment TABLE (
		[date] datetime NOT NULL,
		[deadline] datetime NOT NULL
	);
	DECLARE @assignment_date datetime;
	DECLARE @assignment_deadline datetime;

	 -- !!Careful when returning more as 1 result!!
	INSERT INTO @assignment SELECT [date], [deadline] FROM Asignment WHERE id = @assignment_id;
	SET @assignment_date = (SELECT [date] FROM @assignment);
	SET @assignment_deadline = (SELECT deadline FROM @assignment);

	-- datestart and deadline of an inspection should be in the span of the whole assignment/project
	IF @datestart >= @assignment_date AND @deadline >= @assignment_date
			AND
		@datestart <= @assignment_deadline AND @deadline <= @assignment_deadline
		RETURN 1;
	ELSE
		RETURN 0;

	RETURN 0;
END
