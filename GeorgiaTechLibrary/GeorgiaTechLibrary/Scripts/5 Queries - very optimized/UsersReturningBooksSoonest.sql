SELECT 
    u.SSN, 
    AVG(DATEDIFF(day, l.LoanDate, l.ReturnDate)) AS AvgLoanDuration
FROM 
    [User] u 
INNER JOIN 
    Loan l ON l.UserSSN = u.SSN 
WHERE 
    l.LoanType = 'Book' 
    AND l.ReturnDate IS NOT NULL
GROUP BY 
    u.SSN
ORDER BY 
    AvgLoanDuration ASC, 
    u.SSN ASC;
