
CREATE TABLE [dbo].[User](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](150) NOT NULL,
	[Address] [nvarchar](200) NOT NULL,
	[Address2] [nvarchar](200) NULL,
	[City] [nvarchar](200) NOT NULL,
	[Region] [nvarchar](200) NOT NULL,
	[PostalCode] [nvarchar](50) NOT NULL,
	[Country] [nvarchar](150) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[IsBuyer] BIT NOT NULL,
	[IsSeller] BIT NOT NULL,

 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)
) 

