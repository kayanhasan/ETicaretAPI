using System;
using System.Collections.Generic;
using System.Text;

namespace ETicaretAPI.Application.Abstractions.Services
{
    public interface IProductService
    {
        Task<byte[]> QrCodeToProductAsync(string productId);
        Task StockUpdateToProductAsync(string productId, int stock);
    }
}
