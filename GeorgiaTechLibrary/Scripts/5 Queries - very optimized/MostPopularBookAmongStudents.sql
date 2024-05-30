SELECT TOP 5 WITH TIES
    b.ISBN, 
    COUNT(l.Id) AS NumberOfLoans
FROM 
    Book b 
INNER JOIN 
    BookInstance bi ON bi.BookISBN = b.ISBN
INNER JOIN 
    Loan l ON l.BookInstanceId = bi.Id 
INNER JOIN 
    [Member] m ON m.UserSSN = l.UserSSN 
WHERE 
	m.MemberType = 'Student'
GROUP BY 
    b.ISBN
ORDER BY 
	NumberOfLoans DESC
