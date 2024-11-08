using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanQuanAo.Share.ViewModels
{
    public class ProductImageVM
    {
        public Guid Id { get; set; }
        public Guid? ProductDetailId { get; set; }
        public string? ProductName { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
