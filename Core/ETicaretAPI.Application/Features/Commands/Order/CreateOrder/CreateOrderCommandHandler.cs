using ETicaretAPI.Application.Abstractions.Hubs;
using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.DTOs.Events; // 1. Event modelimizi dahil ettik
using MassTransit; // 2. MassTransit namespace'ini ekledik
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.Order.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommandRequest, CreateOrderCommandResponse>
    {
        readonly IOrderService _orderService;
        readonly IBasketService _basketService;
        readonly IOrderHubService _orderHubService;
        readonly IPublishEndpoint _publishEndpoint; // 3. MassTransit fırlatıcısını enjekte ediyoruz

        public CreateOrderCommandHandler(IOrderService orderService, IBasketService basketService, IOrderHubService orderHubService, IPublishEndpoint publishEndpoint)
        {
            _orderService = orderService;
            _basketService = basketService;
            _orderHubService = orderHubService;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<CreateOrderCommandResponse> Handle(CreateOrderCommandRequest request, CancellationToken cancellationToken)
        {
            // 1. Veritabanına sipariş kaydı (Senkron - kalması gerekir)
            await _orderService.CreateOrderAsync(new()
            {
                Address = request.Address,
                Description = request.Description,
                BasketId = _basketService.GetUserActiveBasket?.Id.ToString()
            });

            // 2. Admin paneline anlık web bildirim sinyali (SignalR - hızlı çalışır)
            await _orderHubService.OrderAddedMessageAsync("Heyy, yeni bir sipariş geldi! :) ");

            // 🔥 İŞTE SİHİRLİ DOKUNUŞ:
            // Sipariş sonrası tetiklenecek ağır işleri (Mail, stok düşümü vs.) kuyruğa fırlatıyoruz.
            // Bu satır milisaniyeler içinde mesajı RabbitMQ'ya bırakır ve müşteri hiç beklemez.
            await _publishEndpoint.Publish<OrderCreatedEvent>(new OrderCreatedEvent
            {
                OrderId = Guid.NewGuid(), // Eğer CreateOrderAsync geriye oluşturulan sipariş nesnesini veya ID'sini dönüyorsa onu verin
                CustomerEmail = "kayanhasan01@gmail.com", // Kullanıcı bilgisinden veya sepetten dinamik çekilebilir
                TotalPrice = 0 // İhtiyacınıza göre doldurabilirsiniz
            }, cancellationToken);

            return new();
        }
    }
}