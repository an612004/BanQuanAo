using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanQuanAo.Share.Models
{
    public class CartItem
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? CartId { get; set; }
        public Guid? ProductDetail_ID { get; set; }
        public int? Quantity { get; set; }
        public int? Price { get; set; }
        [ForeignKey("CartId")]
        public Cart? Cart { get; set; }
        [ForeignKey("ProductDetail_ID")]
        public ProductDetail? ProductDetail { get; set; }
    }
}
