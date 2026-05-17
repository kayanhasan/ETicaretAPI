using System;
using System.Collections.Generic;
using System.Text;

namespace ETicaretAPI.Application.Abstractions.Caching
{
    public interface ICacheService
    {
        // Redis'ten veri çekmek için generic metot
        Task<T?> GetAsync<T>(string key);

        // Redis'e veri yazmak için (opsiyonel süre parametreli)
        Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);

        // Önbellekten belirli bir anahtarı silmek için
        Task RemoveAsync(string key);

        // Anahtarın Redis'te olup olmadığını kontrol etmek için
        Task<bool> AnyAsync(string key);
        //Belirli bir şablona uyan tüm cache'leri siler (Örn: "products-*")
        Task RemoveByPatternAsync(string pattern);
    }
}
