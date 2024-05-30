SELECT 
    CAST(COUNT(l.BookInstanceId) AS FLOAT) / COUNT(l.Id) * 100 AS Books,
    CAST(SUM(CASE WHEN di.DigitalItemType = 'Video' THEN 1 ELSE 0 END) AS FLOAT) / COUNT(l.Id) * 100 AS Videos,
    CAST(SUM(CASE WHEN di.DigitalItemType = 'Audio' THEN 1 ELSE 0 END) AS FLOAT) / COUNT(l.Id) * 100 AS Audios,
    CAST(SUM(CASE WHEN di.DigitalItemType = 'Text' THEN 1 ELSE 0 END) AS FLOAT) / COUNT(l.Id) * 100 AS Texts,
    CAST(SUM(CASE WHEN di.DigitalItemType = 'Image' THEN 1 ELSE 0 END) AS FLOAT) / COUNT(l.Id) * 100 AS Images
FROM 
    Loan l
LEFT JOIN 
    DigitalItem di ON di.Id = l.DigitalItemId;