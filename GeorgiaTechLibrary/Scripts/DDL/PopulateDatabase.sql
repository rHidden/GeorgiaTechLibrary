USE GeorgiaTechLibrary;

DECLARE @i INT = 1;

-- Insert data into Library table
DECLARE @randomStreet NVARCHAR(50);
DECLARE @randomCity NVARCHAR(50);
DECLARE @randomZipcode NVARCHAR(50);
DECLARE @randomStreetNumber INT;

WHILE @i <= 10
BEGIN
    SET @randomStreet = (SELECT TOP 1 value FROM (VALUES ('Dannebrogsgade'), ('Vesterbro'), ('Norregade'), ('Bispensgade'), ('Engvej'), ('Frederiksvej')) AS Streets(value) ORDER BY NEWID());
    SET @randomCity = (SELECT TOP 1 value FROM (VALUES ('Aalborg'), ('Aarhus'), ('Kobenhavn')) AS Cities(value) ORDER BY NEWID());
    SET @randomZipcode = CASE @randomCity
                            WHEN 'Aalborg' THEN '9000'
                            WHEN 'Aarhus' THEN '8000'
                            WHEN 'Kobenhavn' THEN '1050'
                         END;
	SET @randomStreetNumber = FLOOR(RAND() * 100) + 1;

    INSERT INTO [Library] (Name, Street, StreetNumber, City, Zipcode)
    VALUES (CONCAT('Library ', @i), @randomStreet, @randomStreetNumber, @randomCity, @randomZipcode);
    SET @i = @i + 1;
END


-- Insert data into Book table
DECLARE @randomSubjectArea NVARCHAR(50);
DECLARE @randomDescription NVARCHAR(50);
DECLARE @canLoan BIT;

SET @i = 1;
WHILE @i <= 1000
BEGIN
    SET @randomSubjectArea = (SELECT TOP 1 value FROM (VALUES 
        ('Maths'), 
        ('Physics'), 
        ('Chemistry'), 
        ('Biology'), 
        ('History'), 
        ('Geography'), 
        ('Literature'), 
        ('Art'), 
        ('Computer Science'), 
        ('Economics')
    ) AS SubjectAreas(value) ORDER BY NEWID());

    SET @randomDescription = (SELECT TOP 1 value FROM (VALUES 
        ('An in-depth study of the subject matter.'), 
        ('A comprehensive guide to the topic.'), 
        ('An essential read for enthusiasts.'), 
        ('Detailed analysis and explanations.'), 
        ('A critical examination of key concepts.'), 
        ('Insightful and engaging content.'), 
        ('A fundamental resource for learners.'), 
        ('Thoroughly researched and well-written.'), 
        ('An authoritative text in the field.'), 
        ('An informative and accessible introduction.'), 
        ('Rich with examples and case studies.'), 
        ('Highly recommended for students.'), 
        ('Covers all essential aspects.'), 
        ('An invaluable reference guide.'), 
        ('An extensive overview of the subject.'), 
        ('Packed with practical information.'), 
        ('Provides clear and concise explanations.'), 
        ('Well-organized and easy to follow.'), 
        ('A deep dive into the topic.'), 
        ('A masterful presentation of the subject.')
    ) AS Descriptions(value) ORDER BY NEWID());

	SET @canLoan = CASE 
        WHEN RAND() <= 0.9 THEN 1 
        ELSE 0 
    END;

    INSERT INTO Book (ISBN, [Name], CanLoan, [Description], SubjectArea)
    VALUES (CONCAT('ISBN', (RIGHT('000' + CAST(@i AS VARCHAR(3)), 3))), CONCAT('Book nr.', @i), @canLoan, @randomDescription, @randomSubjectArea);
    
    SET @i = @i + 1;
END;

-- Insert data into BookAuthor table
SET @i = 1;
DECLARE @randomAuthor NVARCHAR(50);
DECLARE @randomBookISBN NVARCHAR(50);

WHILE @i <= 1000
BEGIN
    SET @randomAuthor = (SELECT TOP 1 value FROM (VALUES 
        ('J.K. Rowling'), 
        ('George Orwell'), 
        ('Jane Austen'), 
        ('Mark Twain'), 
        ('Charles Dickens'), 
        ('Ernest Hemingway'), 
        ('F. Scott Fitzgerald'), 
        ('William Shakespeare'), 
        ('J.R.R. Tolkien'), 
        ('Agatha Christie'), 
        ('Leo Tolstoy'), 
        ('H.G. Wells'), 
        ('Isaac Asimov'), 
        ('Mary Shelley'), 
        ('Herman Melville'), 
        ('Virginia Woolf'), 
        ('Oscar Wilde'), 
        ('Arthur Conan Doyle'), 
        ('J.D. Salinger'), 
        ('Edgar Allan Poe'), 
        ('Aldous Huxley'), 
        ('Emily Brontë'), 
        ('Lewis Carroll'), 
        ('James Joyce'), 
        ('Marcel Proust'), 
        ('Gabriel Garcia Marquez'), 
        ('Franz Kafka'), 
        ('Fyodor Dostoevsky'), 
        ('Victor Hugo'), 
        ('Homer'), 
        ('Dante Alighieri'), 
        ('Geoffrey Chaucer'), 
        ('Miguel de Cervantes'), 
        ('Jules Verne'), 
        ('Thomas Hardy'), 
        ('Gustave Flaubert'), 
        ('Anton Chekhov'), 
        ('John Steinbeck'), 
        ('George Eliot'), 
        ('Rudyard Kipling'), 
        ('Harper Lee'), 
        ('Tennessee Williams'), 
        ('Henry James'), 
        ('Jack London'), 
        ('Edith Wharton'), 
        ('Joseph Conrad'), 
        ('Robert Louis Stevenson'), 
        ('Ray Bradbury'), 
        ('Sylvia Plath')
    ) AS Authors(value) ORDER BY NEWID());

    SET @randomBookISBN = CONCAT('ISBN',(RIGHT('000' + CAST(CAST((RAND() * 1000) + 1 AS INT) AS VARCHAR(3)), 3)));

    INSERT INTO BookAuthor (BookISBN, [Name])
    VALUES (@randomBookISBN, @randomAuthor);
    
		IF CAST(@i AS INT) % 2 = 0
	BEGIN
        SET @randomAuthor = (
            SELECT TOP 1 [Name]
            FROM (
                VALUES 
                    ('David Buncek'),
                    ('Max Kendra'),
                    ('Cosmin Stefan Raita'),
                    ('Aziz Kadem'),
                    ('Andrej Piecka'),
                    ('Viktor Tindula')
            ) AS AdditionalAuthors([Name])
            ORDER BY NEWID()
        );
		INSERT INTO DigitalItemAuthor (DigitalItemId, [Name])
		VALUES (@i, @randomAuthor);
	END

    SET @i = @i + 1;
END;


-- Insert data into BookInstance table
SET @i = 1;
DECLARE @bookISBN VARCHAR(50);
DECLARE @isLoaned BIT;

WHILE @i <= 10000
BEGIN
    SET @bookISBN = CONCAT('ISBN', RIGHT('000' + CAST(((@i - 1) / 10 + 1) AS VARCHAR(3)), 3)); --10 instances for each book ensured
    
    -- to ensure that isLoaned cannot be true if canLoan is false, otherwise 50/50 chance it is loaned
    SELECT @canLoan = CanLoan FROM Book WHERE ISBN = @bookISBN;
    IF @canLoan = 1
    BEGIN
        SET @isLoaned = CASE WHEN RAND() <= 0.5 THEN 1 ELSE 0 END;
    END
    ELSE
    BEGIN
        SET @isLoaned = 0;
    END

    -- Insert into BookInstance
    INSERT INTO BookInstance (Id, IsLoaned, BookISBN, LibraryName)
    VALUES (@i, @isLoaned, @bookISBN, CONCAT('Library ', @i % 10 + 1));

    SET @i = @i + 1;
END


-- Insert data into DigitalItem table
SET @i = 1;
WHILE @i <= 1000
BEGIN
    DECLARE @digitalItemType VARCHAR(50);
    DECLARE @format VARCHAR(50);
    DECLARE @length VARCHAR(50);
    DECLARE @resolutionWidth INT;
    DECLARE @resolutionHeight INT;

    -- Randomly select DigitalItemType
    DECLARE @randomType INT = ROUND(RAND() * 3, 0);
    IF @randomType = 0
        SET @digitalItemType = 'Audio';
    ELSE IF @randomType = 1
        SET @digitalItemType = 'Video';
    ELSE IF @randomType = 2
        SET @digitalItemType = 'Text';
    ELSE
        SET @digitalItemType = 'Image';

    -- Determine Format based on DigitalItemType
    IF @digitalItemType = 'Audio'
        SET @format = (SELECT TOP 1 FORMAT FROM (VALUES ('WAV'), ('MP3'), ('AAC'), ('FLAC'), ('Ogg')) AS Formats(Format) ORDER BY NEWID());
    ELSE IF @digitalItemType = 'Video'
        SET @format = (SELECT TOP 1 FORMAT FROM (VALUES ('MP4'), ('AVI'), ('MOV'), ('Mkv'), ('Ogg')) AS Formats(Format) ORDER BY NEWID());
    ELSE IF @digitalItemType = 'Image'
        SET @format = (SELECT TOP 1 FORMAT FROM (VALUES ('JPEG'), ('PNG'), ('GIF')) AS Formats(Format) ORDER BY NEWID());
    ELSE
        SET @format = 'PDF';

    -- Determine Length for Audio and Video
    IF @digitalItemType IN ('Audio', 'Video')
        SET @length = CONVERT(VARCHAR(50), ROUND(RAND() * 3600, 0)); -- Length in seconds
    ELSE
        SET @length = NULL;

    -- Determine ResolutionWidth and ResolutionHeight for Audio and Video
    IF @digitalItemType IN ('Image', 'Video')
    BEGIN
        DECLARE @randomResolution INT = ROUND(RAND() * 3, 0);
        IF @randomResolution = 0
            SET @resolutionWidth = 3840; -- 4K
        ELSE IF @randomResolution = 1
            SET @resolutionWidth = 2560; -- 2K
        ELSE IF @randomResolution = 2
            SET @resolutionWidth = 1920; -- Full HD
        ELSE
            SET @resolutionWidth = 1280; -- HD

        IF @resolutionWidth = 3840 -- 4K
            SET @resolutionHeight = 2160;
        ELSE IF @resolutionWidth = 2560 -- 2K
            SET @resolutionHeight = 1440;
        ELSE IF @resolutionWidth = 1920 -- Full HD
            SET @resolutionHeight = 1080;
        ELSE
            SET @resolutionHeight = 720; -- HD
    END
    ELSE
    BEGIN
        SET @resolutionWidth = NULL;
        SET @resolutionHeight = NULL;
    END

    -- Insert into DigitalItem table
    INSERT INTO DigitalItem (Id, [Name], Size, [Format], DigitalItemType, [Length], ResolutionWidth, ResolutionHeight)
    VALUES (@i, CONCAT('DigitalItem nr. ', @i), ROUND(RAND() * 100, 2), @format, @digitalItemType, @length, @resolutionWidth, @resolutionHeight);

    SET @i = @i + 1;
END


SET @i = 1;
DECLARE @authorName VARCHAR(50);
WHILE @i <= 1000
BEGIN
    
    -- Randomly select author name
    SET @authorName = (
        SELECT TOP 1 [Name]
        FROM (
            VALUES 
                ('Michael Crichton'),
                ('H.P. Lovecraft'),
                ('Aldous Huxley'),
                ('Philip Roth'),
                ('Khaled Hosseini'),
                ('Orson Scott Card'),
                ('Roald Dahl'),
                ('Margaret Mitchell'),
                ('Robert Louis Stevenson'),
                ('Hermann Melville'),
                ('Lewis Carroll'),
                ('Emily Brontë'),
                ('Charlotte Brontë'),
                ('James Baldwin'),
                ('Gillian Flynn'),
                ('Jonathan Swift'),
                ('Nathaniel Hawthorne'),
                ('Malcolm Gladwell'),
                ('Maya Angelou'),
                ('Neil Gaiman'),
                ('Junot Díaz'),
                ('Richard Adams'),
                ('R.L. Stine'),
                ('Nicholas Sparks'),
                ('Isaac Asimov'),
                ('Elie Wiesel'),
                ('Jodi Picoult'),
                ('Don DeLillo'),
                ('E.L. James'),
                ('Ernest Cline'),
                ('John Green'),
                ('John Grisham'),
                ('Graham Greene'),
                ('Hunter S. Thompson'),
                ('J.R. Ward'),
                ('Jack Kerouac'),
                ('J.R. Tolkien'),
                ('J.D. Robb'),
                ('J.D. Vance'),
                ('Jojo Moyes'),
                ('Jacqueline Wilson'),
                ('Jenny Han'),
                ('Jennifer Weiner'),
                ('Jeff Kinney'),
                ('Julia Quinn'),
                ('James Patterson'),
                ('John Irving'),
                ('Jude Deveraux')
        ) AS Authors([Name])
        ORDER BY NEWID()
    );

    -- Insert into DigitalItemAuthor table
    INSERT INTO DigitalItemAuthor (DigitalItemId, [Name])
    VALUES (@i, @authorName);

	IF CAST(@i AS INT) % 2 = 0
	BEGIN
        SET @authorName = (
            SELECT TOP 1 [Name]
            FROM (
                VALUES 
                    ('David Buncek'),
                    ('Max Kendra'),
                    ('Cosmin Stefan Raita'),
                    ('Aziz Kadem'),
                    ('Andrej Piecka'),
                    ('Viktor Tindula')
            ) AS AdditionalAuthors([Name])
            ORDER BY NEWID()
        );
		INSERT INTO DigitalItemAuthor (DigitalItemId, [Name])
		VALUES (@i, @authorName);
	END

    SET @i = @i + 1;
END

-- Insert data into DigitalItemLibrary table
SET @i = 1;
WHILE @i <= 1000
BEGIN
    INSERT INTO DigitalItemLibrary (DigitalItemId, LibraryName)
    VALUES (@i, CONCAT('Library ', @i % 10 + 1));
    SET @i = @i + 1;
END

-- Insert data into User table
SET @i = 1;
DECLARE @randomPhoneNumber VARCHAR(50);
DECLARE @randomFirstName VARCHAR(50);
DECLARE @randomLastName VARCHAR(50);

WHILE @i <= 1000
BEGIN
    SET @randomStreet = (SELECT TOP 1 value FROM (VALUES ('Dannebrogsgade'), ('Vesterbro'), ('Norregade'), ('Bispensgade'), ('Engvej'), ('Frederiksvej')) AS Streets(value) ORDER BY NEWID());
    SET @randomCity = (SELECT TOP 1 value FROM (VALUES ('Aalborg'), ('Aarhus'), ('Kobenhavn')) AS Cities(value) ORDER BY NEWID());
    SET @randomZipcode = CASE @randomCity
                            WHEN 'Aalborg' THEN '9000'
                            WHEN 'Aarhus' THEN '8000'
                            WHEN 'Kobenhavn' THEN '1050'
                         END;
    SET @randomStreetNumber = FLOOR(RAND() * 100) + 1;
	SET @randomPhoneNumber = CONCAT('+45 ', 90000000 + @i*1000);

    -- Randomly select first name
    SET @randomFirstName = (
        SELECT TOP 1 value 
        FROM (VALUES 
                ('John'), ('William'), ('James'), ('Charles'), ('George'), 
                ('Thomas'), ('Robert'), ('Edward'), ('Henry'), ('Frank'), 
                ('Joseph'), ('David'), ('Walter'), ('Harry'), ('Arthur'),
                ('Mary'), ('Anna'), ('Emma'), ('Elizabeth'), ('Margaret'), 
                ('Alice'), ('Bertha'), ('Sarah'), ('Annie'), ('Clara'), 
                ('Ella'), ('Florence'), ('Cora'), ('Martha'), ('Laura')
            ) AS FirstNames(value) 
        ORDER BY NEWID()
    );

    -- Randomly select last name
    SET @randomLastName = (
        SELECT TOP 1 value 
        FROM (VALUES 
                ('Smith'), ('Johnson'), ('Williams'), ('Brown'), ('Jones'), 
                ('Miller'), ('Davis'), ('Garcia'), ('Rodriguez'), ('Martinez'), 
                ('Hernandez'), ('Lopez'), ('Gonzalez'), ('Wilson'), ('Anderson'), 
                ('Thomas'), ('Taylor'), ('Moore'), ('Jackson'), ('Martin'), 
                ('Lee'), ('Perez'), ('Thompson'), ('White'), ('Harris'), 
                ('Sanchez'), ('Clark'), ('Ramirez'), ('Lewis'), ('Robinson')
            ) AS LastNames(value) 
        ORDER BY NEWID()
    );

    INSERT INTO [User] (SSN, PhoneNumber, Street, StreetNumber, City, Zipcode, FirstName, LastName, LibraryName)
    VALUES (CONCAT('SSN', (RIGHT('000' + CAST(@i AS VARCHAR(3)), 3))), @randomPhoneNumber, @randomStreet, @randomStreetNumber, @randomCity, @randomZipcode, @randomFirstName, @randomLastName, CONCAT('Library ', @i % 10 + 1));
    SET @i = @i + 1;
END


-- Insert data into Staff table
SET @i = 1;
DECLARE @randomRole VARCHAR(50);

WHILE @i <= 100
BEGIN
    -- Randomly select a role
    SET @randomRole = (
        SELECT TOP 1 value 
        FROM (VALUES 
                ('Chief librarian'), ('Departmental associate librarian'), ('Reference librarian'), ('Check-out staff'), ('Library assistant')
            ) AS Roles(value) 
        ORDER BY NEWID()
    );

    INSERT INTO Staff (UserSSN, [Role], LibrarianNumber)
    VALUES (CONCAT('SSN', (RIGHT('000' + CAST(@i AS VARCHAR(3)), 3))), @randomRole, CONCAT('LibNum', @i));
    SET @i = @i + 1;
END

-- Insert data into Member table
SET @i = 101;
DECLARE @memberType VARCHAR(50);
DECLARE @expiryDate DATE;

WHILE @i <= 1000
BEGIN
    -- Randomly select member type
	SET @memberType = CASE 
        WHEN RAND() <= 0.9 THEN 'Student'
        ELSE 'Professor'
    END;

    -- Set expiry date based on member type
    IF @memberType = 'Student'
    BEGIN
        SET @expiryDate = DATEADD(YEAR, 1 + FLOOR(RAND() * 3), GETDATE()); -- Random expiry date within 1 to 4 years
    END
    ELSE
    BEGIN
        SET @expiryDate = NULL; -- Expiry date is null for professors
    END

    INSERT INTO [Member] (UserSSN, CardNumber, ExpiryDate, Photo, MemberType)
    VALUES (CONCAT('SSN', (RIGHT('000' + CAST(@i AS VARCHAR(3)), 3))), CONCAT('CardNum', @i), @expiryDate, CONCAT('Photo', @i), @memberType);
    SET @i = @i + 1;
END


-- Insert data into Loan table
SET @i = 1
DECLARE @userSSN VARCHAR(10);
DECLARE @loanDate DATE;
DECLARE @dueDate DATE;
DECLARE @returnDate DATE;
DECLARE @loanType VARCHAR(50);
DECLARE @digitalItemId INT;
DECLARE @bookInstanceId INT;


WHILE @i <= 10000
BEGIN
    -- Randomly select a user with a weighted distribution
    SET @userSSN = (SELECT TOP 1 SSN FROM [User] ORDER BY NEWID());

    -- Randomly select a loan type
    SET @loanType = CASE WHEN RAND() <= 0.8 THEN 'Book' ELSE 'DigitalItem' END;

    -- Set loan date and due date
    SET @loanDate = DATEADD(DAY, -FLOOR(RAND() * 30), GETDATE()); -- Loan date within the last 30 days
    SET @dueDate = DATEADD(DAY, 14, @loanDate); -- Due date 14 days after loan date

    -- Initialize return date to NULL
    SET @returnDate = NULL;

    IF @loanType = 'Book'
    BEGIN
        -- Randomly select a book instance with a weighted distribution
        SET @bookInstanceId = (SELECT TOP 1 Id FROM BookInstance ORDER BY NEWID());
        
        -- Check if the selected book instance can be loaned
        SELECT @canLoan = CanLoan FROM Book WHERE ISBN = (SELECT BookISBN FROM BookInstance WHERE Id = @bookInstanceId);
        SELECT @isLoaned = IsLoaned FROM BookInstance WHERE Id = @bookInstanceId;

        IF @canLoan = 1 AND @isLoaned = 0
        BEGIN
            -- 80% chance that the item is returned
            IF RAND() <= 0.8
            BEGIN
                SET @returnDate = DATEADD(DAY, 14, @loanDate); -- Return date 7 days after loan date
                UPDATE BookInstance SET IsLoaned = 0 WHERE Id = @bookInstanceId;
            END
            ELSE
            BEGIN
                UPDATE BookInstance SET IsLoaned = 1 WHERE Id = @bookInstanceId;
            END

            -- Insert the loan record
            INSERT INTO Loan (Id, UserSSN, LoanDate, ReturnDate, DueDate, LoanType, BookInstanceId)
            VALUES (@i, @userSSN, @loanDate, @returnDate, @dueDate, @loanType, @bookInstanceId);
        END
    END
    ELSE
    BEGIN
        -- Randomly select a digital item with a weighted distribution
        SET @digitalItemId = (SELECT TOP 1 Id FROM DigitalItem ORDER BY NEWID());
        
        -- 80% chance that the item is returned
        IF RAND() <= 0.8
        BEGIN
            SET @returnDate = DATEADD(DAY, 7, @loanDate); -- Return date 7 days after loan date
        END

        -- Insert the loan record
        INSERT INTO Loan (Id, UserSSN, LoanDate, ReturnDate, DueDate, LoanType, DigitalItemId)
        VALUES (@i, @userSSN, @loanDate, @returnDate, @dueDate, @loanType, @digitalItemId);
    END

    SET @i = @i + 1;
END