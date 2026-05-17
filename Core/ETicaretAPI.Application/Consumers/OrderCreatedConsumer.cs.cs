using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.DTOs.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Consumers
{
    public class OrderCreatedConsumer : IConsumer<OrderCreatedEvent>
    {
        private readonly IMailService _mailService;
        private readonly ILogger<OrderCreatedConsumer> _logger;

        public OrderCreatedConsumer(IMailService mailService, ILogger<OrderCreatedConsumer> logger)
        {
            _mailService = mailService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            var message = context.Message;
            _logger.LogInformation($"[RabbitMQ] Yeni sipariş oluşturulma eventi yakalandı. ID: {message.OrderId}");

            // 🔥 GERÇEK MAİL OPERASYONU: 
            // IMailService içerisindeki ilk sipariş e-posta metodunuzu buraya bağlıyoruz.
            // (Eğer metodun parametre yapısı veya ismi sizde biraz farklıysa projenize göre güncelleyebilirsiniz, 
            // genellikle Mail, Sipariş Kodu veya Sipariş Nesnesi ister.)
            await _mailService.SendOrderReceivedMailAsync(
                message.CustomerEmail,
                "TEST1",
                DateTime.UtcNow,
                "Sayın Müşterimiz"
            );

            _logger.LogInformation($"[RabbitMQ] {message.CustomerEmail} için 'Siparişiniz Alındı' maili başarıyla sıraya alındı ve gönderildi.");
        }
    }
}