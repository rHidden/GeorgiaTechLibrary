USE master

DROP DATABASE IF EXISTS GeorgiaTechLibrary;

GO

CREATE DATABASE GeorgiaTechLibrary;

GO

USE GeorgiaTechLibrary

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
    [Description] VARCHAR(MAX)
);

CREATE TABLE BookAuthor(
	BookISBN VARCHAR(50),
	[Name] VARCHAR(50),
	PRIMARY KEY (BookISBN, [Name]),
	FOREIGN KEY (BookISBN) REFERENCES Book(ISBN)
);

CREATE TABLE BookSubjectArea(
	BookISBN VARCHAR(50),
	SubjectArea VARCHAR(50),
	PRIMARY KEY (BookISBN, SubjectArea),
	FOREIGN KEY (BookISBN) REFERENCES Book(ISBN)
);

CREATE TABLE BookInstance (
    Id INT PRIMARY KEY,
    IsLoaned BIT,
    BookISBN VARCHAR(50),
    LibraryName VARCHAR(50),
    FOREIGN KEY (BookISBN) REFERENCES Book(ISBN),
    FOREIGN KEY (LibraryName) REFERENCES [Library]([Name])
);

CREATE TABLE DigitalItem (
    Id INT PRIMARY KEY,
	[Name] VARCHAR(50),
    Size float,
    [Format] VARCHAR(50),
    DigitalItemType VARCHAR(50),
    [Length] VARCHAR(50),
    Quality VARCHAR(50),
    ResolutionWidth INT,
    ResolutionHeight INT
);

CREATE TABLE DigitalItemAuthor(
	DigitalItemId INT,
	[Name] VARCHAR(50),
	PRIMARY KEY (DigitalItemId, [Name]),
	FOREIGN KEY (DigitalItemId) REFERENCES DigitalItem(Id)
);

CREATE TABLE DigitalItemLibrary (
    DigitalItemId INT,
    LibraryName VARCHAR(50),
	PRIMARY KEY (DigitalItemId, LibraryName),
    FOREIGN KEY (DigitalItemId) REFERENCES DigitalItem(Id),
    FOREIGN KEY (LibraryName) REFERENCES Library([Name])
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
    FOREIGN KEY (LibraryName) REFERENCES [Library]([Name])
);

CREATE TABLE Staff (
    UserSSN VARCHAR(10),
    [Role] VARCHAR(50),
    LibrarianNumber VARCHAR(50),
    FOREIGN KEY (UserSSN) REFERENCES [User](SSN)
);

CREATE TABLE [Member] (
    UserSSN VARCHAR(10),
    CardNumber VARCHAR(50),
    ExpiryDate DATE,
    Photo VARCHAR(50),
    MemberType VARCHAR(50),
    FOREIGN KEY (UserSSN) REFERENCES [User](SSN)
);

CREATE TABLE Loan (
    Id INT PRIMARY KEY,
	UserSSN VARCHAR(10),
    LoanDate DATE,
    ReturnDate DATE,
	LoanType VARCHAR(50),
    DigitalItemId INT,
    BookInstanceId INT,
    FOREIGN KEY (BookInstanceId) REFERENCES BookInstance(Id),
    FOREIGN KEY (DigitalItemId) REFERENCES DigitalItem(Id),
	FOREIGN KEY (UserSSN) REFERENCES [User](SSN)
);
