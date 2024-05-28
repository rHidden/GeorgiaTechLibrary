CREATE FUNCTION dbo.GetAverageNumberOfDaysToReturnBooks()
RETURNS FLOAT
AS
BEGIN
    DECLARE @AverageNumberOfDays FLOAT;

    SELECT @AverageNumberOfDays = AVG(DATEDIFF(day, LoanDate, ReturnDate))
    FROM Loan
    WHERE ReturnDate IS NOT NULL
    AND BookInstanceId IS NOT NULL;

    RETURN @AverageNumberOfDays;
END;

SELECT dbo.GetAverageNumberOfDaysToReturnBooks() AS AverageNumberOfDaysToReturnBooks;
