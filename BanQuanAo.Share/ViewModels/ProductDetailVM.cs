using BanQuanAo.Share.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanQuanAo.Share.ViewModels
{
    public class ProductDetailVM
    {
        public Guid? Id { get; set; }
        public Guid? ProductId { get; set; }
        public string? ProductName { get; set; }
        public Guid? SizeId { get; set; }
        public string? SizeName { get; set; }
        public Guid? ColorId { get; set; }
        public string? ColorName { get; set; }
        public string? ColorCode { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public decimal? PriceSale { get; set; }
        public DateTime? Create_At { get; set; }
        public DateTime? Update_At { get; set; }
        public string? Description { get; set; }
        public bool? Status { get; set; }
        public List<string>? Images { get; set; } //lay link anh cua tbProImage
        public List<IFormFile>? filecollection { get; set; }
    }
}
