use GeorgiaTechLibrary

--create the different users and their logins
CREATE LOGIN A
    WITH PASSWORD = 'passwordA', CHECK_POLICY = OFF;
CREATE USER [Chief Librarian] FOR LOGIN A;

CREATE LOGIN B
    WITH PASSWORD = 'passwordB', CHECK_POLICY = OFF;
CREATE USER [Departmental Associate Librarian]  FOR LOGIN B;

CREATE LOGIN C 
    WITH PASSWORD = 'passwordC', CHECK_POLICY = OFF;
CREATE USER [Reference Librarian]  FOR LOGIN C;

CREATE LOGIN D
    WITH PASSWORD = 'passwordD', CHECK_POLICY = OFF;
CREATE USER [Checkout Staff] FOR LOGIN D;

CREATE LOGIN E 
    WITH PASSWORD = 'passwordE', CHECK_POLICY = OFF;
CREATE USER [Library Assistant] FOR LOGIN E;

-- Set privileges to A (copy the result using this small piece of SQL)
-- SELECT 'GRANT SELECT, UPDATE, DELETE, INSERT ON ' + ltrim(rtrim(name)) + ' TO [Chief Librarian] WITH GRANT OPTION' from sysobjects where type = 'U'

GRANT SELECT, UPDATE, DELETE, INSERT ON [Library] TO [Chief Librarian] WITH GRANT OPTION
GRANT SELECT, UPDATE, DELETE, INSERT ON Book TO [Chief Librarian] WITH GRANT OPTION
GRANT SELECT, UPDATE, DELETE, INSERT ON BookAuthor TO [Chief Librarian] WITH GRANT OPTION
GRANT SELECT, UPDATE, DELETE, INSERT ON BookInstance TO [Chief Librarian] WITH GRANT OPTION
GRANT SELECT, UPDATE, DELETE, INSERT ON DigitalItem TO [Chief Librarian] WITH GRANT OPTION
GRANT SELECT, UPDATE, DELETE, INSERT ON DigitalItemAuthor TO [Chief Librarian] WITH GRANT OPTION
GRANT SELECT, UPDATE, DELETE, INSERT ON DigitalItemLibrary TO [Chief Librarian] WITH GRANT OPTION
GRANT SELECT, UPDATE, DELETE, INSERT ON [User] TO [Chief Librarian] WITH GRANT OPTION
GRANT SELECT, UPDATE, DELETE, INSERT ON Staff TO [Chief Librarian] WITH GRANT OPTION
GRANT SELECT, UPDATE, DELETE, INSERT ON [Member] TO [Chief Librarian] WITH GRANT OPTION
GRANT SELECT, UPDATE, DELETE, INSERT ON Loan TO [Chief Librarian] WITH GRANT OPTION
GRANT SELECT, UPDATE, DELETE, INSERT ON BookReservation TO [Chief Librarian] WITH GRANT OPTION

GRANT SELECT ON [Library] TO [Departmental Associate Librarian] 
GRANT SELECT, UPDATE, DELETE, INSERT ON Book TO [Departmental Associate Librarian] WITH GRANT OPTION
GRANT SELECT, UPDATE, DELETE, INSERT ON BookAuthor TO [Departmental Associate Librarian] WITH GRANT OPTION
GRANT SELECT, UPDATE, DELETE, INSERT ON BookInstance TO [Departmental Associate Librarian] WITH GRANT OPTION
GRANT SELECT, UPDATE, DELETE, INSERT ON DigitalItem TO [Departmental Associate Librarian] WITH GRANT OPTION
GRANT SELECT, UPDATE, DELETE, INSERT ON DigitalItemAuthor TO [Departmental Associate Librarian] WITH GRANT OPTION
GRANT SELECT ON DigitalItemLibrary TO [Departmental Associate Librarian] 
GRANT SELECT, UPDATE, DELETE, INSERT ON [User] TO [Departmental Associate Librarian] WITH GRANT OPTION
GRANT SELECT, UPDATE, DELETE, INSERT ON Staff TO [Departmental Associate Librarian]
GRANT SELECT, UPDATE, DELETE, INSERT ON [Member] TO [Departmental Associate Librarian] WITH GRANT OPTION
GRANT SELECT, UPDATE, DELETE, INSERT ON Loan TO [Departmental Associate Librarian] WITH GRANT OPTION
GRANT SELECT, UPDATE, DELETE, INSERT ON BookReservation TO [Departmental Associate Librarian] WITH GRANT OPTION

GRANT SELECT, UPDATE, DELETE, INSERT ON Book TO [Reference Librarian] WITH GRANT OPTION
GRANT SELECT, UPDATE, DELETE, INSERT ON BookAuthor TO [Reference Librarian] WITH GRANT OPTION
GRANT SELECT, UPDATE, DELETE, INSERT ON BookInstance TO [Reference Librarian] WITH GRANT OPTION
GRANT SELECT, UPDATE, DELETE, INSERT ON DigitalItem TO [Reference Librarian] WITH GRANT OPTION
GRANT SELECT, UPDATE, DELETE, INSERT ON DigitalItemAuthor TO [Reference Librarian] WITH GRANT OPTION
GRANT SELECT, UPDATE ON [User] TO [Reference Librarian] 
GRANT SELECT, UPDATE ON [Member] TO [Reference Librarian]
GRANT SELECT, UPDATE, DELETE, INSERT ON Loan TO [Reference Librarian] WITH GRANT OPTION
GRANT SELECT, UPDATE, DELETE, INSERT ON BookReservation TO [Reference Librarian] WITH GRANT OPTION

CREATE VIEW CheckoutStaffUsers AS 
SELECT u.FirstName, u.LastName, u.PhoneNumber, m.CardNumber, m.ExpiryDate, m.MemberType
FROM [User] u
INNER JOIN [Member] m ON m.UserSSN = u.SSN

GRANT SELECT ON Book TO [Checkout Staff] 
GRANT SELECT ON BookAuthor TO [Checkout Staff] 
GRANT SELECT, INSERT ON BookInstance TO [Checkout Staff] WITH GRANT OPTION
GRANT SELECT, INSERT ON DigitalItem TO [Checkout Staff] WITH GRANT OPTION
GRANT SELECT, INSERT ON DigitalItemAuthor TO [Checkout Staff]
GRANT SELECT ON CheckoutStaffUsers TO [Checkout Staff]
GRANT SELECT, INSERT ON Loan TO [Checkout Staff]
GRANT UPDATE ON Loan (ReturnDate, DueDate) TO [Checkout Staff]
GRANT SELECT, UPDATE, DELETE, INSERT ON BookReservation TO [Checkout Staff] WITH GRANT OPTION

GRANT SELECT ON Book TO [Library Assistant] 
GRANT SELECT ON BookAuthor TO [Library Assistant] 
GRANT SELECT ON BookInstance TO [Library Assistant] 
GRANT SELECT ON DigitalItem TO [Library Assistant] 
GRANT SELECT ON DigitalItemAuthor TO [Library Assistant]
GRANT SELECT ON CheckoutStaffUsers TO [Library Assistant]
GRANT SELECT, INSERT ON Loan TO [Library Assistant]
GRANT UPDATE ON Loan (ReturnDate, DueDate) TO [Library Assistant]
GRANT SELECT ON BookReservation TO [Library Assistant] 