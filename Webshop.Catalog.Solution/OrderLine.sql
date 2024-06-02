CREATE TABLE [dbo].[OrderLine](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Quantity] [int] NOT NULL,
	[SubTotal] [float] NOT NULL,
	[ProductId] [int] NULL,
	[OrderId] [int] NULL,
 CONSTRAINT [PK_OrderLine] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)
)
GO

ALTER TABLE [dbo].[OrderLine]  WITH CHECK ADD CONSTRAINT [FK_ProductOrderLine] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
ON DELETE SET NULL
GO

ALTER TABLE [dbo].[OrderLine]  WITH CHECK ADD CONSTRAINT [FK_OrderOrderLine] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Order] ([Id])
ON DELETE CASCADE
GO

