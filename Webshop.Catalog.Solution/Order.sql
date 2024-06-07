CREATE TABLE [dbo].[Order](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TotalPrice] [float](10,2) NOT NULL,
	[Discount] [int] NOT NULL,
	[Date] [date] NOT NULL,
	[BuyerId] [int] NULL,
 CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)
)
GO

ALTER TABLE [dbo].[Order]  WITH CHECK ADD CONSTRAINT [FK_BuyerOrder] FOREIGN KEY([BuyerId])
REFERENCES [dbo].[User] ([Id])
ON DELETE SET NULL
GO

