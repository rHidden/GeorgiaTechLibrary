/*
The Query Processor estimates that implementing the following index could improve the query cost by 16.2123%.
*/


USE [GeorgiaTechLibrary]
GO
CREATE NONCLUSTERED INDEX IX_Loan_LoanType_ReturnDate_UserSSN_LoanDate
ON [dbo].[Loan] ([LoanType],[ReturnDate])
INCLUDE ([UserSSN],[LoanDate])
GO
