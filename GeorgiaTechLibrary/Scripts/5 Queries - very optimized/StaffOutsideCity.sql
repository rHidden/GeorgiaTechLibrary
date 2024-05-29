SELECT 
    l.Name, 
    COUNT(u.SSN) AS StaffLivingOutsideOfCity 
FROM 
    Staff s 
INNER JOIN 
    [User] u ON u.SSN = s.UserSSN
INNER JOIN 
    Library l ON l.Name = u.LibraryName
WHERE 
    u.City <> l.City
GROUP BY 
    l.Name
ORDER BY 
	StaffLivingOutsideOfCity DESC

SELECT 
    l.City, 
    COUNT(u.SSN) AS StaffLivingOutsideOfCity 
FROM 
    Staff s 
INNER JOIN 
    [User] u ON u.SSN = s.UserSSN
INNER JOIN 
    Library l ON l.Name = u.LibraryName
WHERE 
    u.City <> l.City
GROUP BY 
    l.City
ORDER BY 
	StaffLivingOutsideOfCity DESC
