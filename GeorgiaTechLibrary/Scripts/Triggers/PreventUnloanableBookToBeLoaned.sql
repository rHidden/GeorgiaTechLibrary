CREATE TRIGGER trg_PreventUnloanableBookToBeLoaned
ON [Loan]
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1
        FROM inserted i
        JOIN BookInstance bi ON i.BookInstanceId = bi.Id
        JOIN Book b ON bi.BookISBN = b.ISBN
        WHERE bi.IsLoaned = 1
        OR b.Status <> 'loanable'
    )
    BEGIN
        -- Rollback the transaction and raise an error
        ROLLBACK TRANSACTION;
        RAISERROR ('Cannot loan a book that is not loanable or is currently loaned.', 16, 1);
    END
END;