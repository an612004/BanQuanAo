using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanQuanAo.Share.Models
{
    public class ProductImage
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? ProductDetailId { get; set; }
        public string? ImageUrl { get; set; }

        [ForeignKey("ProductDetailId")]
        public ProductDetail? Product { get; set; }
    }
}
