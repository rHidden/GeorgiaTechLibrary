SELECT 
    u.SSN, 
    SUM(DATEDIFF(day, l.DueDate, l.ReturnDate)) AS SumOfDaysOfBeingLate
FROM 
    [User] u 
INNER JOIN 
    Loan l ON l.UserSSN = u.SSN
WHERE 
    l.LoanDate > DATEADD(YEAR, -1, GETDATE()) 
    AND l.ReturnDate > l.DueDate
GROUP BY 
    u.SSN
HAVING 
    AVG(DATEDIFF(day, l.DueDate, l.ReturnDate)) > 3
ORDER BY 
    SumOfDaysOfBeingLate DESC;
