using ETicaretAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETicaretAPI.Application.DTOs.Product
{
    public class ProductListDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Stock { get; set; }
        public float Price { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public ICollection<ProductImageFile> ProductImageFiles { get; set; }
    }
}
