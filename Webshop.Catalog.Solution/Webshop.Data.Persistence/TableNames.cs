using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.Data.Persistence
{
    public static class TableNames
    {
        public static class Catalog
        {
            public static string CATEGORYTABLE = "Category";
            public static string PRODUCTTABLE = "Product";
            public static string PRODUCTCATEGORYTABLE = "ProductCategory";
        }

        public static class User
        {
            public static string USERTABLE = "User";
        }

        public static class Order
        {
            public static string ORDERTABLE = "Order";
            public static string ORDERLINETABLE = "OrderLine";
        }
    }
}
