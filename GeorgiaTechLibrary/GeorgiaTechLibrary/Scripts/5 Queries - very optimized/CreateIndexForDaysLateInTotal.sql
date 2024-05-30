/*
Missing Index Details from SQLQuery5.sql - DESKTOP-F3GTIMV\SQLEXPRESS.GeorgiaTechLibrary (DESKTOP-F3GTIMV\Lenovo (62))
The Query Processor estimates that implementing the following index could improve the query cost by 25.8779%.
*/

USE [GeorgiaTechLibrary]
GO
CREATE NONCLUSTERED INDEX IX_Loan_LoanDate_UserSSN_ReturnDate_DueDate
ON [dbo].[Loan] ([LoanDate])
INCLUDE ([UserSSN],[ReturnDate],[DueDate])
GO
