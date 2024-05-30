CREATE TRIGGER trg_SetIsLoanedBasedOnEvent
ON [Loan]
AFTER INSERT, UPDATE
AS
BEGIN
    DECLARE @BookInstanceId INT;

    SELECT @BookInstanceId = i.BookInstanceId
    FROM inserted i;

    IF EXISTS (SELECT * FROM inserted WHERE ReturnDate IS NULL)
    BEGIN
        UPDATE BookInstance
        SET isLoaned = 1
		WHERE Id = @BookInstanceId
    END

	ELSE
    BEGIN
        UPDATE BookInstance
        SET isLoaned = 0
		WHERE Id = @BookInstanceId
    END
END;