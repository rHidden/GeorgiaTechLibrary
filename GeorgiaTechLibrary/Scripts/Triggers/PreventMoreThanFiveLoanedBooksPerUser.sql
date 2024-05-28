CREATE TRIGGER trg_PreventMoreThanFiveLoanedBooksPerUser
ON Loan
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @UserSSN VARCHAR(10);
    DECLARE @BookInstanceId INT;

    SELECT @UserSSN = i.UserSSN, @BookInstanceId = i.BookInstanceId
    FROM inserted i;

    IF @BookInstanceId IS NOT NULL
    BEGIN
        DECLARE @CurrentBookLoanCount INT;
        SELECT @CurrentBookLoanCount = COUNT(*)
        FROM Loan
        WHERE UserSSN = @UserSSN
        AND BookInstanceId IS NOT NULL
        AND ReturnDate IS NULL;

        IF @CurrentBookLoanCount >= 5
        BEGIN
            ROLLBACK TRANSACTION;
            RAISERROR ('User cannot have more than 5 books loaned out at the same time.', 16, 1);
        END
    END
END;
