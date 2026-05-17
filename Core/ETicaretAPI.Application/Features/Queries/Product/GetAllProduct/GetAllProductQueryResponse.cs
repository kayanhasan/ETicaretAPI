using ETicaretAPI.Application.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETicaretAPI.Application.Features.Queries.Product.GetAllProduct
{
    public class GetAllProductQueryResponse
    {
        public int TotalProductCount { get; set; }
        public List<ProductListDto> Products { get; set; }
    }
}
