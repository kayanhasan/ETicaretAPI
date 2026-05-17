using System;
using System.Collections.Generic;
using System.Text;

namespace ETicaretAPI.Application.DTOs.Events
{
    public record OrderCreatedEvent
    {
        public Guid OrderId { get; init; }
        public string CustomerEmail { get; init; }
        public decimal TotalPrice { get; init; }
    }
}
