using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Catalog.Application.Features.Product.Dtos;

namespace Webshop.Catalog.Application.Features.Product.Queries.GetProductsOfSeller
{
    public class GetProductsOfSellerQuery : IQuery<List<ProductDto>>
    {
        public GetProductsOfSellerQuery(int sellerId)
        {
            SellerId = sellerId;
        }
        public int SellerId { get; private set; }
    }
}
