using System;
using System.Collections.Generic;
using System.Text;

namespace ETicaretAPI.Application.DTOs.Events
{
    public record OrderCompletedEvent
    {
        public Guid OrderId { get; init; }
        public string CustomerEmail { get; init; }
        public decimal TotalPrice { get; init; }
        public DateTime OrderDate { get; init; }
    }
}
