CREATE PROCEDURE GetOverdueLoans 
AS
BEGIN
    SELECT l.Id, l.UserSSN, l.LoanDate, l.DueDate, l.LoanType, l.BookInstanceId, l.DigitalItemId, u.FirstName, u.LastName
    FROM Loan l
    JOIN [User] u ON l.UserSSN = u.SSN
    WHERE l.DueDate < GETDATE() AND l.ReturnDate IS NULL;
END;
