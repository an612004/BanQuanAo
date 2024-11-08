using BanQuanAo.Share.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanQuanAo.Share.ViewModels
{
    public class ProductVM
    {
        public Guid Id { get; set; }
        public Guid? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? CategoryNamePa { get; set; }
        public Guid? BrandId { get; set; }
        public string? BrandName { get; set; }
        public Guid? MaterialId { get; set; }
        public string? MaterialName { get; set; }
        public Guid? WishId { get; set; }
        public string? ProductName { get; set; }
        public int? AvailableQuantity { get; set; }
        public DateTime? Create_At { get; set; }
        public DateTime? Update_At { get; set; }
        public string? Description { get; set; }
        public string? Long_Description { get; set; }
        public decimal? MinPrice
        {
            get
            {
                if (ProductDvms != null && ProductDvms.Count > 0)
                    return ProductDvms.Where(c => c.ProductId == Id).Min(pd => pd.PriceSale);
                return 0;
            }
        }

        public decimal? MaxPrice
        {
            get
            {
                if (ProductDvms != null && ProductDvms.Count > 0)
                    return ProductDvms.Where(c => c.ProductId == Id).Max(pd => pd.PriceSale);
                return 0;
            }
        }
        public bool? Status { get; set; }
        public List<string>? Images { get; set; } //lay link anh cua tbProImage
        public virtual List<ProductDetail>? ProductDvms { get; set; }
		public virtual List<ProductImage>? ProductImages { get; set; }
	}
}
