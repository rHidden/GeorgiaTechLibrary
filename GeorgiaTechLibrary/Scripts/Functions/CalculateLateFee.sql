CREATE FUNCTION CalculateLateFee (@ReturnDate DATE, @DueDate DATE)
RETURNS DECIMAL(10, 2)
AS
BEGIN
    DECLARE @LateFee DECIMAL(10, 2)
    SET @LateFee = 1 * DATEDIFF(DAY, @DueDate, @ReturnDate) --add 1 dkk per day? maybe less/more?
    RETURN CASE 
               WHEN @LateFee > 0 THEN @LateFee 
               ELSE 0 
           END
END;
