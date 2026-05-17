using System;
using System.Collections.Generic;
using System.Text;

namespace ETicaretAPI.Domain.Entities
{
    public class ProductImageFile :File
    {
        public bool Showcase { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
