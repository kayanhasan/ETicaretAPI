using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.DTOs.Events; // 1. Yazdığımız Event modelini ekledik
using ETicaretAPI.Application.DTOs.Order;
using MassTransit; // 2. MassTransit'i ekledik
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.Order.CompleteOrder
{
    public class CompleteOrderCommandHandler : IRequestHandler<CompleteOrderCommandRequest, CompleteOrderCommandResponse>
    {
        readonly IOrderService _orderService;
        readonly IPublishEndpoint _publishEndpoint; // 3. IMailService yerine MassTransit fırlatıcısını enjekte ediyoruz

        public CompleteOrderCommandHandler(IOrderService orderService, IPublishEndpoint publishEndpoint)
        {
            _orderService = orderService;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<CompleteOrderCommandResponse> Handle(CompleteOrderCommandRequest request, CancellationToken cancellationToken)
        {
            (bool succeeded, CompletedOrderDTO dto) = await _orderService.CompleteOrderAsync(request.Id);

            if (succeeded)
            {
                // 🔥 İŞTE DEĞİŞİM: Senkron mail göndermek yerine olayı kuyruğa fırlatıyoruz!
                // Sistem mail gitmesini BEKLEMEDEN direkt return new() satırına atlayacak.
                await _publishEndpoint.Publish<OrderCompletedEvent>(new OrderCompletedEvent
                {
                    OrderId = Guid.Parse(request.Id),
                    CustomerEmail = dto.EMail,
                    TotalPrice = 0, // Eğer DTO içinde varsa doldurabilirsiniz, yoksa opsiyonel
                    OrderDate = dto.OrderDate
                    // Dilerseniz Event kaydınıza OrderCode ve Username mülklerini de ekleyip buraya geçebilirsiniz.
                }, cancellationToken);
            }

            return new();
        }
    }
}