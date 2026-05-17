using ETicaretAPI.Application.Abstractions.Services; // IMailService için
using ETicaretAPI.Application.DTOs.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Consumers
{
    public class OrderCompletedEventConsumer : IConsumer<OrderCompletedEvent>
    {
        private readonly IMailService _mailService; // Real mail servisimizi buraya aldık
        private readonly ILogger<OrderCompletedEventConsumer> _logger;

        public OrderCompletedEventConsumer(IMailService mailService, ILogger<OrderCompletedEventConsumer> logger)
        {
            _mailService = mailService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<OrderCompletedEvent> context)
        {
            var eventData = context.Message;

            _logger.LogInformation($"[RabbitMQ] Sipariş tamamlanma mesajı işleniyor. Mail gönderilecek adres: {eventData.CustomerEmail}");

            // Gerçek mail gönderme kodunuzu asenkron olarak arka planda tetikliyoruz
            // Not: Event nesnenize OrderCode ve Username mülklerini eklerseniz burayı tam doldurabilirsiniz.
            await _mailService.SendCompletedOrderMailAsync(
                eventData.CustomerEmail,
                "KOD-123", // eventData.OrderCode 
                eventData.OrderDate,
                "Sayın Müşterimiz" // eventData.Username
            );

            _logger.LogInformation($"[RabbitMQ] Bilgilendirme maili arka planda başarıyla gönderildi.");
        }
    }
}