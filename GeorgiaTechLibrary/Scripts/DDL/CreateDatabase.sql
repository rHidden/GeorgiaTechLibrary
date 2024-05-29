USE master;

IF DB_ID('GeorgiaTechLibrary') IS NOT NULL
BEGIN
	ALTER DATABASE GeorgiaTechLibrary SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE GeorgiaTechLibrary;
END

CREATE DATABASE GeorgiaTechLibrary;
ALTER DATABASE GeorgiaTechLibrary SET MULTI_USER;

GO
USE GeorgiaTechLibrary;

-- Create the tables
CREATE TABLE [Library] (
    [Name] VARCHAR(50) PRIMARY KEY,
    Street VARCHAR(50),
    StreetNumber VARCHAR(50),
    City VARCHAR(50),
    Zipcode VARCHAR(50)
);

CREATE TABLE Book (
    ISBN VARCHAR(50) PRIMARY KEY,
    [Name] VARCHAR(50),
    CanLoan BIT,
    [Description] VARCHAR(MAX),
    SubjectArea VARCHAR(50)
);

CREATE TABLE BookAuthor(
    BookISBN VARCHAR(50),
    [Name] VARCHAR(50),
    PRIMARY KEY (BookISBN, [Name]),
    FOREIGN KEY (BookISBN) REFERENCES Book(ISBN) ON DELETE CASCADE
);

CREATE TABLE BookInstance (
    Id INT PRIMARY KEY,
    IsLoaned BIT,
    BookISBN VARCHAR(50),
    LibraryName VARCHAR(50),
    FOREIGN KEY (BookISBN) REFERENCES Book(ISBN) ON DELETE CASCADE,
    FOREIGN KEY (LibraryName) REFERENCES [Library]([Name]) ON DELETE SET NULL
);

CREATE TABLE DigitalItem (
    Id INT PRIMARY KEY,
    [Name] VARCHAR(50),
    Size FLOAT,
    [Format] VARCHAR(50),
    DigitalItemType VARCHAR(50),
    [Length] int,
    ResolutionWidth INT,
    ResolutionHeight INT
);

CREATE TABLE DigitalItemAuthor(
    DigitalItemId INT,
    [Name] VARCHAR(50),
    PRIMARY KEY (DigitalItemId, [Name]),
    FOREIGN KEY (DigitalItemId) REFERENCES DigitalItem(Id) ON DELETE CASCADE
);

CREATE TABLE DigitalItemLibrary (
    DigitalItemId INT,
    LibraryName VARCHAR(50),
    PRIMARY KEY (DigitalItemId, LibraryName),
    FOREIGN KEY (DigitalItemId) REFERENCES DigitalItem(Id) ON DELETE CASCADE,
    FOREIGN KEY (LibraryName) REFERENCES [Library]([Name]) ON DELETE NO ACTION
);

CREATE TABLE [User] (
    SSN VARCHAR(10) PRIMARY KEY,
    PhoneNumber VARCHAR(50),
    Street VARCHAR(50),
    StreetNumber VARCHAR(50),
    City VARCHAR(50),
    Zipcode VARCHAR(50),
    FirstName VARCHAR(50),
    LastName VARCHAR(50),
    LibraryName VARCHAR(50),
    FOREIGN KEY (LibraryName) REFERENCES [Library]([Name]) ON DELETE SET NULL
);

CREATE TABLE Staff (
    UserSSN VARCHAR(10) PRIMARY KEY,
    [Role] VARCHAR(50),
    LibrarianNumber VARCHAR(50),
    FOREIGN KEY (UserSSN) REFERENCES [User](SSN) ON DELETE CASCADE
);

CREATE TABLE [Member] (
    UserSSN VARCHAR(10) PRIMARY KEY,
    CardNumber VARCHAR(50),
    ExpiryDate DATE,
    Photo VARCHAR(50),
    MemberType VARCHAR(50),
    FOREIGN KEY (UserSSN) REFERENCES [User](SSN) ON DELETE CASCADE
);

CREATE TABLE Loan (
    Id INT PRIMARY KEY,
    UserSSN VARCHAR(10),
    LoanDate DATE,
    ReturnDate DATE,
    DueDate DATE,
    LoanType VARCHAR(50),
    DigitalItemId INT,
    BookInstanceId INT,
    FOREIGN KEY (BookInstanceId) REFERENCES BookInstance(Id) ON DELETE SET NULL,
    FOREIGN KEY (DigitalItemId) REFERENCES DigitalItem(Id) ON DELETE SET NULL,
    FOREIGN KEY (UserSSN) REFERENCES [User](SSN) ON DELETE SET NULL
);
