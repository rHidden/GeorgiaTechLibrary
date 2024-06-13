/*
The Query Processor estimates that implementing the following index could improve the query cost by 16.2493%.
*/

USE [GeorgiaTechLibrary]
GO
CREATE NONCLUSTERED INDEX IX_Loan_UserSSN_BookInstanceId
ON [dbo].[Loan] ([UserSSN])
INCLUDE ([BookInstanceId])
GO

