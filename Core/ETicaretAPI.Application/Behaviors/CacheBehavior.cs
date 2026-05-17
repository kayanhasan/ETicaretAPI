using ETicaretAPI.Application.Abstractions.Caching;
using MediatR;

namespace ETicaretAPI.Application.Behaviors
{
    // Bu pipeline, sadece ICachableQuery interface'ini uygulayan istekler (TRequest) için tetiklenir
    public class CacheBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>, ICachableQuery
    {
        private readonly ICacheService _cacheService;

        public CacheBehavior(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // 1. İstek geldiğinde önce Redis'e bak, bu key'e ait veri var mı?
            if (await _cacheService.AnyAsync(request.CacheKey))
            {
                var cachedResponse = await _cacheService.GetAsync<TResponse>(request.CacheKey);
                if (cachedResponse != null)
                {
                    // EĞER VARSA: Veritabanına hiç gitme, direkt Redis'teki veriyi dön!
                    return cachedResponse;
                }
            }

            // 2. EĞER YOKSA: Yoluna devam et (next metodu asıl Handler'ı tetikler ve veritabanından veriyi çeker)
            var response = await next();

            // 3. Veritabanından gelen taze veriyi, bir sonraki istekler için Redis'e kaydet
            await _cacheService.SetAsync(request.CacheKey, response, request.Expiration);

            return response;
        }
    }
}