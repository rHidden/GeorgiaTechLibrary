-- Populating the Library table
INSERT INTO [Library] (Name, Street, StreetNumber, City, Zipcode) VALUES 
('Central Library', 'Peachtree St', '100', 'Atlanta', '30332'),
('West End Library', 'Ralph David Abernathy Blvd', '525', 'Atlanta', '30310'),
('East Atlanta Library', 'Flat Shoals Ave', '400', 'Atlanta', '30316');

-- Populating the Book table with 1000 rows
DECLARE @i INT = 1;
WHILE @i <= 1000
BEGIN
    INSERT INTO Book (ISBN, [Name], CanLoan, [Description])
    VALUES (RIGHT('0000000000' + CAST(@i AS VARCHAR(10)), 13), 'Book ' + CAST(@i AS VARCHAR(10)), 1, 'Description for Book ' + CAST(@i AS VARCHAR(10)));
    SET @i = @i + 1;
END;

-- Populating the BookAuthor table with 1000 rows
DECLARE @j INT = 1;
WHILE @j <= 1000
BEGIN
    INSERT INTO BookAuthor (BookISBN, [Name])
    VALUES (RIGHT('0000000000' + CAST(@j AS VARCHAR(10)), 13), 'Author ' + CAST(@j AS VARCHAR(10)));
    SET @j = @j + 1;
END;

-- Populating the BookSubjectArea table with 1000 rows
DECLARE @k INT = 1;
WHILE @k <= 1000
BEGIN
    INSERT INTO BookSubjectArea (BookISBN, SubjectArea)
    VALUES (RIGHT('0000000000' + CAST(@k AS VARCHAR(10)), 13), 'Subject Area ' + CAST(@k AS VARCHAR(10)));
    SET @k = @k + 1;
END;

-- Populating the BookInstance table with 5000 rows
DECLARE @l INT = 1;
DECLARE @libIdx INT = 1;
WHILE @l <= 5000
BEGIN
    INSERT INTO BookInstance (Id, IsLoaned, BookISBN, LibraryName)
    VALUES (@l, 0, RIGHT('0000000000' + CAST((@l % 1000) + 1 AS VARCHAR(10)), 13), CASE WHEN @libIdx % 3 = 1 THEN 'Central Library' WHEN @libIdx % 3 = 2 THEN 'West End Library' ELSE 'East Atlanta Library' END);
    SET @l = @l + 1;
    SET @libIdx = @libIdx + 1;
END;

-- Populating the DigitalItem table with 1000 rows
-- Populating the DigitalItem table with 1000 rows with varying DigitalItemType and formats

DECLARE @m INT = 1;
DECLARE @digitalItemType VARCHAR(50);
DECLARE @format VARCHAR(50);
DECLARE @length VARCHAR(50);
DECLARE @resolutionWidth INT;
DECLARE @resolutionHeight INT;

WHILE @m <= 1000
BEGIN
    -- Determine DigitalItemType and corresponding format
    SET @digitalItemType = CASE 
        WHEN @m % 4 = 0 THEN 'Video'
        WHEN @m % 4 = 1 THEN 'Image'
        WHEN @m % 4 = 2 THEN 'Audio'
        ELSE 'Text'
    END;
    
    SET @format = CASE @digitalItemType
        WHEN 'Video' THEN 'MP4'
        WHEN 'Image' THEN 'JPEG'
        WHEN 'Audio' THEN 'MP3'
        ELSE 'PDF'
    END;
    
    SET @length = CASE @digitalItemType
        WHEN 'Video' THEN CAST((1 + @m % 3) AS VARCHAR(10)) + ' hours'
        WHEN 'Audio' THEN CAST((30 + @m % 60) AS VARCHAR(10)) + ' minutes'
        ELSE 'N/A'
    END;

    SET @resolutionWidth = CASE @digitalItemType
        WHEN 'Video' THEN 1920
        WHEN 'Image' THEN 1080
        ELSE NULL
    END;

    SET @resolutionHeight = CASE @digitalItemType
        WHEN 'Video' THEN 1080
        WHEN 'Image' THEN 720
        ELSE NULL
    END;

    INSERT INTO DigitalItem (Id, [Name], Size, [Format], DigitalItemType, [Length], Quality, ResolutionWidth, ResolutionHeight)
    VALUES (
        @m,
        'Digital Item ' + CAST(@m AS VARCHAR(10)),
        ROUND((10 + @m * 0.01), 2),
        @format,
        @digitalItemType,
        @length,
        CASE @digitalItemType
            WHEN 'Video' THEN 'HD'
            WHEN 'Image' THEN 'High'
            WHEN 'Audio' THEN 'High'
            ELSE 'N/A'
        END,
        @resolutionWidth,
        @resolutionHeight
    );

    SET @m = @m + 1;
END;


-- Populating the DigitalItemAuthor table with 1000 rows
DECLARE @n INT = 1;
WHILE @n <= 1000
BEGIN
    INSERT INTO DigitalItemAuthor (DigitalItemId, [Name])
    VALUES (@n, 'Digital Author ' + CAST(@n AS VARCHAR(10)));
    SET @n = @n + 1;
END;

-- Populating the DigitalItemLibrary table with 1000 rows
DECLARE @o INT = 1;
DECLARE @libIdx2 INT = 1;
WHILE @o <= 1000
BEGIN
    INSERT INTO DigitalItemLibrary (DigitalItemId, LibraryName)
    VALUES (@o, CASE WHEN @libIdx2 % 3 = 1 THEN 'Central Library' WHEN @libIdx2 % 3 = 2 THEN 'West End Library' ELSE 'East Atlanta Library' END);
    SET @o = @o + 1;
    SET @libIdx2 = @libIdx2 + 1;
END;

-- Populating the User table with 1000 rows
DECLARE @p INT = 1;
WHILE @p <= 1000
BEGIN
    INSERT INTO [User] (SSN, PhoneNumber, Street, StreetNumber, City, Zipcode, FirstName, LastName, LibraryName)
    VALUES (RIGHT('000000000' + CAST(@p AS VARCHAR(10)), 10), '404-555-' + RIGHT('0000' + CAST(@p AS VARCHAR(10)), 4), 'Street', CAST(@p AS VARCHAR(10)), 'Atlanta', '30332', 'FirstName' + CAST(@p AS VARCHAR(10)), 'LastName' + CAST(@p AS VARCHAR(10)), CASE WHEN @p % 3 = 1 THEN 'Central Library' WHEN @p % 3 = 2 THEN 'West End Library' ELSE 'East Atlanta Library' END);
    SET @p = @p + 1;
END;

-- Populating the Staff table with 1000 rows
DECLARE @q INT = 1;
WHILE @q <= 1000
BEGIN
    INSERT INTO Staff (UserSSN, [Role], LibrarianNumber)
    VALUES (RIGHT('000000000' + CAST(@q AS VARCHAR(10)), 10), 'Role ' + CAST(@q AS VARCHAR(10)), 'LIB' + RIGHT('000' + CAST(@q AS VARCHAR(10)), 3));
    SET @q = @q + 1;
END;

-- Populating the Member table with 1000 rows
DECLARE @r INT = 1;
WHILE @r <= 1000
BEGIN
    INSERT INTO [Member] (UserSSN, CardNumber, ExpiryDate, Photo, MemberType)
    VALUES (RIGHT('000000000' + CAST(@r AS VARCHAR(10)), 10), 'CARD' + RIGHT('000' + CAST(@r AS VARCHAR(10)), 3), '2025-12-31', 'photo' + CAST(@r AS VARCHAR(10)) + '.jpg', 'Regular');
    SET @r = @r + 1;
END;

-- Populating the Loan table with 1000 rows
DECLARE @s INT = 1;
WHILE @s <= 1000
BEGIN
    INSERT INTO Loan (Id, UserSSN, LoanDate, ReturnDate, LoanType, DigitalItemId, BookInstanceId)
    VALUES (@s, RIGHT('000000000' + CAST((@s % 1000) + 1 AS VARCHAR(10)), 10), GETDATE(), DATEADD(day, 30, GETDATE()), 'Book', NULL, CASE WHEN @s % 2 = 0 THEN @s ELSE NULL END);
    SET @s = @s + 1;
END;