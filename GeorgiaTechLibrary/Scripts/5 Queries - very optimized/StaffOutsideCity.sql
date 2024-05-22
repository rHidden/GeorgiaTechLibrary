SELECT l.name, COUNT(u.SSN)
as StaffLivingOutsideOfCity 
FROM Staff s 
LEFT JOIN [User] u on u.ssn = s.UserSSN
LEFT JOIN Library l on l.Name = u.LibraryName
WHERE NOT u.City = l.City
GROUP BY l.Name

SELECT l.city, COUNT(u.SSN)
as StaffLivingOutsideOfCity 
FROM Staff s 
LEFT JOIN [User] u on u.ssn = s.UserSSN
LEFT JOIN Library l on l.Name = u.LibraryName
WHERE NOT u.City = l.City
GROUP BY l.City