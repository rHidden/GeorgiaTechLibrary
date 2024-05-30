CREATE TRIGGER trg_PreventEarlyReturnDate
ON Loan
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1
        FROM inserted i
        WHERE i.ReturnDate < i.LoanDate
    )
    BEGIN
        ROLLBACK TRANSACTION;
        RAISERROR ('Return date cannot be earlier than loan date.', 16, 1);
    END
END;
