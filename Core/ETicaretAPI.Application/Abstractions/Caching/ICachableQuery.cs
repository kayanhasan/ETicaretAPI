using System;
using System.Collections.Generic;
using System.Text;

namespace ETicaretAPI.Application.Abstractions.Caching
{
    public interface ICachableQuery
    {
        // Önbellekte saklanacak benzersiz isim (Örn: "products-all")
        string CacheKey { get; }

        // Verinin hafızada ne kadar taze kalacağı (Örn: 30 dakika)
        TimeSpan? Expiration { get; }
    }
}
