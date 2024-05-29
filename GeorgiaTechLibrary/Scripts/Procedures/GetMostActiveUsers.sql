CREATE PROCEDURE GetMostActiveUsers
AS
BEGIN
    SELECT TOP 10 WITH TIES
		u.SSN, 
		u.PhoneNumber,
		u.FirstName, 
		u.LastName, 
		u.Street, 
		u.StreetNumber, 
		u.City, 
		u.Zipcode
	FROM 
		[User] u 
	INNER JOIN 
		Loan l ON l.UserSSN = u.SSN 
	WHERE
		l.LoanDate > DATEADD(YEAR, -1, GETDATE())
	GROUP BY 
		u.SSN, 
		u.PhoneNumber,
		u.FirstName, 
		u.LastName, 
		u.Street, 
		u.StreetNumber, 
		u.City, 
		u.Zipcode
	HAVING 
		COUNT(l.Id) > 10
	ORDER BY 
		COUNT(l.Id) DESC
END;