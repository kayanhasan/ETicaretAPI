using ETicaretAPI.Application.Abstractions.Caching;
using ETicaretAPI.Application.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETicaretAPI.Application.Features.Commands.Product.RemoveProduct
{
    public class RemoveProductCommandHandler : IRequestHandler<RemoveProductCommandRequest, RemoveProductCommandResponse>
    {
        readonly IProductWriteRepository _productWriteRepository;
        private readonly ICacheService _cacheService;
        public RemoveProductCommandHandler(IProductWriteRepository productWriteRepository, ICacheService cacheService = null)
        {
            _productWriteRepository = productWriteRepository;
            _cacheService = cacheService;
        }

        public async Task<RemoveProductCommandResponse> Handle(RemoveProductCommandRequest request, CancellationToken cancellationToken)
        {
            await _productWriteRepository.RemoveAsync(request.Id);
            await _productWriteRepository.SaveAsync();
            await _cacheService.RemoveByPatternAsync("products-all-*");
            return new();
        }
    }
}
