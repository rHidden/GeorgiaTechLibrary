﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Webshop.Help.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private string connectionString; //the server connectionstring without database
        private string mainconnectionString;
        private string server = "localhost";
        private List<string> Errors = new List<string>();

        public IndexModel(ILogger<IndexModel> logger, IConfiguration config)
        {
            _logger = logger;
            this.connectionString = config.GetConnectionString("DefaultConnection");
            this.mainconnectionString = this.connectionString;
            string newServer = Environment.GetEnvironmentVariable("SERVER");
            if (!string.IsNullOrEmpty(newServer))
            {
                this.server = newServer;
            }
            this.mainconnectionString = this.mainconnectionString.Replace("{server}", this.server);
        }

        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            //create the database
            this.connectionString = this.mainconnectionString + ";database=master";
            CreateDatabase();
            this.connectionString = this.mainconnectionString + ";database=psuwebshop"; //make sure they are created in the right database
            CreateCategoryTable();
            CreateCustomerTable();
            CreateProductTable();
            CreateProductCategoryTable();
            CreateOrderTable();
            CreateOrderLineTable();
            TempData["errors"] = Errors;
            return Redirect("/?seed=1");
        }

        private void CreateDatabase()
        {            
            ExecuteSQL("CREATE DATABASE psuwebshop", this.connectionString);           
        }

        private void CreateCategoryTable()
        {
            string sql = "CREATE TABLE Category(" +
            "[Id] [int] IDENTITY(1,1) NOT NULL," +
            "[Name] [nvarchar](150) NOT NULL," +
            "[ParentId] [int] NOT NULL," +
            "[Description] [ntext] NOT NULL," +
            "CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED " +
            "(" +
            "[Id] ASC" +
            ")" +
            ")";
            ExecuteSQL(sql, this.connectionString);
        }

        private void CreateCustomerTable()
        {
            string sql = "CREATE TABLE [dbo].[Customer](" +
            "[Id] [int] IDENTITY(1,1) NOT NULL," +
            "[Name] [nvarchar](150) NOT NULL," +
            "[Address] [nvarchar](200) NOT NULL," +
            "[Address2] [nvarchar](200) NULL," +
            "[City] [nvarchar](200) NOT NULL," +
            "[Region] [nvarchar](200) NOT NULL," +
            "[PostalCode] [nvarchar](50) NOT NULL," +
            "[Country] [nvarchar](150) NOT NULL," +
            "[Email] [nvarchar](100) NOT NULL," +
            "[BuyerFlag] [bit] NOT NULL," +
            "[BuyerDescription] [nvarchar](200) NULL," +
            "[SellerFlag] [bit] NOT NULL," +
            "[SellerReview] [float] NULL," +
            "CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED " +
            "(" +
            "[Id] ASC" +
            ")" +
            ")";
            ExecuteSQL(sql, this.connectionString);
        }

        private void CreateProductTable()
        {
            string sql = "CREATE TABLE Product(" +
            "[Id] [int] IDENTITY(1,1) NOT NULL," +
            "[Name] [nvarchar](150) NOT NULL," +
            "[SKU] [nvarchar](50) NOT NULL," +
            "[Price] [int] NOT NULL," +
            "[Currency] [nvarchar](3) NOT NULL," +
            "[Description] [ntext] NULL," +
            "[AmountInStock] [int] NULL," +
            "[MinStock] [int] NULL," +
            "[SellerId] [int] NOT NULL," +
            "CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED " +
            "(" +
            "[Id] ASC" +
            ")" +
            "); " +
            "ALTER TABLE Product " +
            "WITH CHECK ADD CONSTRAINT [FK_CustomerProduct] FOREIGN KEY([SellerId]) " +
            "REFERENCES Customer ([Id]) " +
            "ON DELETE CASCADE";
            ExecuteSQL(sql, this.connectionString);
        }

        private void CreateProductCategoryTable()
        {
            string sql = "CREATE TABLE ProductCategory(" +
            "[ProductId] [int] NOT NULL," +
            "[CategoryId] [int] NOT NULL," +
            "CONSTRAINT [PK_ProductCategory] PRIMARY KEY CLUSTERED " +
            "(" +
            "[ProductId] ASC," +
            "[CategoryId] ASC" +
            ")" +
            ")";
            ExecuteSQL(sql, this.connectionString);
        }

        private void CreateOrderTable()
        {
            string sql = "CREATE TABLE [dbo].[Order](" +
            "[Id] [int] IDENTITY(1,1) NOT NULL," +
            "[TotalPrice] [float] NOT NULL," +
            "[Discount] [int] NOT NULL," +
            "[Date] [date] NOT NULL," +
            "[CustomerId] [int] NULL," +
            "CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED " +
            "(" +
            "[Id] ASC" +
            ")" +
            "); " +
            "ALTER TABLE [dbo].[Order] " +
            "WITH CHECK ADD CONSTRAINT [FK_CustomerOrder] FOREIGN KEY([CustomerId]) " +
            "REFERENCES [dbo].[Customer] ([Id]) " +
            "ON DELETE SET NULL";
            ExecuteSQL(sql, this.connectionString);
        }

        private void CreateOrderLineTable()
        {
            string sql = "CREATE TABLE [dbo].[OrderLine](" +
            "[Id] [int] IDENTITY(1,1) NOT NULL," +
            "[Quantity] [int] NOT NULL," +
            "[SubTotal] [float] NOT NULL," +
            "[ProductId] [int] NULL," +
            "[OrderId] [int] NULL," +
            "CONSTRAINT [PK_OrderLine] PRIMARY KEY CLUSTERED " +
            "(" +
            "[Id] ASC" +
            ")" +
            "); " +
            "ALTER TABLE [dbo].[OrderLine] " +
            "WITH CHECK ADD CONSTRAINT [FK_ProductOrderLine] FOREIGN KEY([ProductId]) " +
            "REFERENCES [dbo].[Product] ([Id]) " +
            "ON DELETE SET NULL; " +
            "ALTER TABLE [dbo].[OrderLine] " +
            "WITH CHECK ADD CONSTRAINT [FK_OrderOrderLine] FOREIGN KEY([OrderId]) " +
            "REFERENCES [dbo].[Order] ([Id]) " +
            "ON DELETE CASCADE";
            ExecuteSQL(sql, this.connectionString);
        }
        private void ExecuteSQL(string sql, string localConnectionString)
        {
            try
            {
                Console.WriteLine("Connection: " + localConnectionString);
                using (SqlConnection connection = new SqlConnection(localConnectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            } catch(Exception ex)
            {
                Errors.Add(ex.Message);
            }
        }
    }
}