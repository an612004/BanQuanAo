using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanQuanAo.Share.ViewModels
{
    public class ViewCartDetails
    {
        public Guid? Id { get; set; }
        public string? ImageName { get; set; }
        public string? ProductName { get; set; }
        public decimal? Price { get; set; }
        public decimal? PriceSale { get; set; }
        public int? Quantity { get; set; }
        public int? TotalPrice { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? CartId { get; set; }
        public Guid? ProductDetail_Id { get; set; }
        public Guid? SizeId { get; set; }
        public Guid? ColorsId { get; set; }
    }
}
