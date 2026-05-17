using ETicaretAPI.Application.Abstractions.Caching;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETicaretAPI.Application.Features.Queries.Product.GetAllProduct
{
    public class GetAllProductQueryRequest : IRequest<GetAllProductQueryResponse>, ICachableQuery
    {
        //public Pagination Pagination { get; set; }
        public int Page { get; set; } = 0;
        public int Size { get; set; } = 5;

        // Redis'te hangi isimle saklanacağını dinamik yapıyoruz.
        // Sayfa numarası ve boyutu değiştikçe Redis'te farklı birer anahtar (key) oluşmalı.
        // Örn: "products-all-p-0-s-10", "products-all-p-1-s-10"
        public string CacheKey => $"products-all-p-{Page}-s-{Size}";

        // Veri Redis'te ne kadar süre taze kalsın? (Örn: 10 dakika)
        public TimeSpan? Expiration => TimeSpan.FromMinutes(10);
    }
}
