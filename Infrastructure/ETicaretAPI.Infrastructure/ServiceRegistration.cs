using ETicaretAPI.Application.Abstractions.Caching;
using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.Abstractions.Services.Configurations;
using ETicaretAPI.Application.Abstractions.Storage;
using ETicaretAPI.Application.Abstractions.Token;
using ETicaretAPI.Application.Consumers;
using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Infrastructure.Enums;
using ETicaretAPI.Infrastructure.Services;
using ETicaretAPI.Infrastructure.Services.Caching;
using ETicaretAPI.Infrastructure.Services.Configurations;
using ETicaretAPI.Infrastructure.Services.Storage;
using ETicaretAPI.Infrastructure.Services.Storage.Azure;
using ETicaretAPI.Infrastructure.Services.Storage.Local;
using ETicaretAPI.Infrastructure.Services.Token;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETicaretAPI.Infrastructure
{
    public static class ServicesRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IStorageService, StorageService>();
            serviceCollection.AddScoped<ITokenHandler, TokenHandler>();
            serviceCollection.AddScoped<IMailService, MailService>();
            serviceCollection.AddScoped<IApplicationService, ApplicationService>();
            serviceCollection.AddScoped<IQRCodeService, QRCodeService>();
            serviceCollection.AddSingleton<IConnectionMultiplexer>(sp =>
                ConnectionMultiplexer.Connect("localhost:6379"));
            serviceCollection.AddScoped<ICacheService, RedisCacheService>();
            serviceCollection.AddMassTransit(configurator =>
            {
                // 1. Yazdığımız Tüketiciyi (Consumer) MassTransit sistemine tanıtıyoruz
                configurator.AddConsumer<OrderCreatedConsumer>();
                configurator.AddConsumer<OrderCompletedEventConsumer>();
                // 2. Taşıma veri yolu olarak RabbitMQ kullanacağımızı belirtiyoruz
                configurator.UsingRabbitMq((context, cfg) =>
                {
                    // Docker üzerinde ayaklandırdığımız RabbitMQ adresi ve portu
                    cfg.Host("amqp://localhost:5672", hostConfigurator =>
                    {
                        // Varsayılan RabbitMQ kimlik bilgileri (Gerekirse güncelleyebilirsiniz)
                        hostConfigurator.Username("guest");
                        hostConfigurator.Password("guest");
                    });

                    // 3. Kuyruk (Queue) Tanımlaması
                    // Bu isimle RabbitMQ üzerinde otomatik bir kuyruk oluşacak
                    cfg.ReceiveEndpoint("order-completed-queue", endpoint =>
                    {
                        // Bu kuyruğa gelen mesajları hangi Consumer işleyecek?
                        endpoint.ConfigureConsumer<OrderCompletedEventConsumer>(context);
                    });
                    cfg.ReceiveEndpoint("order-created-queue", endpoint =>
                    {
                        endpoint.ConfigureConsumer<OrderCreatedConsumer>(context);
                    });
                });
            });
        }
        public static void AddStorage<T>(this IServiceCollection serviceCollection) where T : Storage, IStorage
        {
            serviceCollection.AddScoped<IStorage, T>();
        }
        public static void AddStorage(this IServiceCollection serviceCollection, StorageType storageType)
        {
            switch (storageType)
            {
                case StorageType.Local:
                    serviceCollection.AddScoped<IStorage, LocalStorage>();
                    break;
                case StorageType.Azure:
                    serviceCollection.AddScoped<IStorage, AzureStorage>();
                    break;
                case StorageType.AWS:

                    break;
                default:
                    serviceCollection.AddScoped<IStorage, LocalStorage>();
                    break;
            }
        }
    }
}
