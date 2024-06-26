﻿CREATE TABLE [dbo].[Product](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](150) NOT NULL,
	[SKU] [nvarchar](50) NOT NULL,
	[Price] [int] NOT NULL,
	[Currency] [nvarchar](3) NOT NULL,
	[Description] [ntext] NULL,
	[AmountInStock] [int] NULL,
	[MinStock] [int] NULL,
	[SellerId] [int] NOT NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)
)
GO

ALTER TABLE [dbo].[Product]  WITH CHECK ADD CONSTRAINT [FK_SellerProduct] FOREIGN KEY([SellerId])
REFERENCES [dbo].[Customer] ([Id])
ON DELETE CASCADE
GO