using ETicaretAPI.Application.Abstractions.Caching;
using ETicaretAPI.Application.Abstractions.Hubs;
using ETicaretAPI.Application.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETicaretAPI.Application.Features.Commands.Product.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommandRequest, CreateProductCommandResponse>
    {
        readonly IProductWriteRepository _productWriteRepository;
        readonly IProductHubService _productHubService;
        private readonly ICacheService _cacheService;
        public CreateProductCommandHandler(IProductWriteRepository productWriteRepository, IProductHubService productHubService, ICacheService cacheService = null)
        {
            _productWriteRepository = productWriteRepository;
            _productHubService = productHubService;
            _cacheService = cacheService;
        }

        public async Task<CreateProductCommandResponse> Handle(CreateProductCommandRequest request, CancellationToken cancellationToken)
        {
            await _productWriteRepository.AddAsync(new()
            {
                Name = request.Name,
                Price = request.Price,
                Stock = request.Stock
            });
            await _productWriteRepository.SaveAsync();
            await _productHubService.ProductAddedMessageAsync($"{request.Name} isminde ürün eklenmiştir.");
            await _cacheService.RemoveByPatternAsync("products-all-*");
            Console.WriteLine("Mesaj Hub'dan gönderildi");
            return new();
        }
    }
}
