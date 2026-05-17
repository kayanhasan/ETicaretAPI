using ETicaretAPI.Application.ViewModels.Products;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETicaretAPI.Application.Validators.Products
{
    public class CreateProductValidator:AbstractValidator<VM_Create_Product>
    {
        public CreateProductValidator()
        {
            RuleFor(p => p.Name).NotEmpty().NotNull().WithMessage("Ürün Adı Boş Geçilemez.").MaximumLength(150).MinimumLength(5).WithMessage("Ürün Adı 5-150 karakter ile sınırlıdır.");
            RuleFor(p => p.Stock).NotEmpty().NotNull().WithMessage("Stok Bilgisi Boş Geçilemez.").Must(s=> s>=0).WithMessage("Stok bilgisi eksi değer olamaz."); 
            RuleFor(p => p.Price).NotEmpty().NotNull().WithMessage("Fiyat Bilgisi Boş Geçilemez.").Must(s => s >= 0).WithMessage("Fiyat bilgisi eksi değer olamaz.");
        }
    }
}
