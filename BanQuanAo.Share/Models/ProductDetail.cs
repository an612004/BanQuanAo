using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanQuanAo.Share.Models
{
    public class ProductDetail
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? ProductId { get; set; }
        public Guid? SizeId { get; set; }
        public Guid? ColorId { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public decimal? PriceSale { get; set; }
        public DateTime? Create_At { get; set; }
        public DateTime? Update_At { get; set; }
        public string? Description { get; set; }
        public bool? Status { get; set; }

        [ForeignKey("ProductId")]
        public Product? Product { get; set; }

        [ForeignKey("SizeId")]
        public Size? Size { get; set; }
        [ForeignKey("ColorId")]
        public Colors? Colors { get; set; }

        public virtual List<CartItem>? CartItem { get; set; }
        public virtual List<OrderItem>? OrderItems { get; set; }
        
        public virtual List<ProductImage>? ProductImages { get; set; }
        
    }
}
