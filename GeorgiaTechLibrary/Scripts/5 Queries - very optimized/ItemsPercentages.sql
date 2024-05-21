SELECT CAST(COUNT(l.BookInstanceId) as FLOAT)/COUNT(l.Id)*100 as Books,
CAST((SELECT COUNT(di.Id) FROM DigitalItem di RIGHT JOIN Loan l on di.Id = l.DigitalItemId WHERE DigitalItemType = 'Video')as FLOAT)/COUNT(l.Id)*100 as Videos,
CAST((SELECT COUNT(di.Id) FROM DigitalItem di RIGHT JOIN Loan l on di.Id = l.DigitalItemId WHERE DigitalItemType = 'Audio')as FLOAT)/COUNT(l.Id)*100 as Audios,
CAST((SELECT COUNT(di.Id) FROM DigitalItem di RIGHT JOIN Loan l on di.Id = l.DigitalItemId WHERE DigitalItemType = 'Text')as FLOAT)/COUNT(l.Id)*100 as Texts,
CAST((SELECT COUNT(di.Id) FROM DigitalItem di RIGHT JOIN Loan l on di.Id = l.DigitalItemId WHERE DigitalItemType = 'Image')as FLOAT)/COUNT(l.Id)*100 as Images
FROM Loan l