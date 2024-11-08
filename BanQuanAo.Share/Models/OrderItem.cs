using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanQuanAo.Share.Models
{
    public class OrderItem
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? OrderId { get; set; }
        public Guid? ProductDetailId { get; set; }
        public int? Quantity { get; set; }
        public int? Price { get; set; }
        //cái nào có giá thì sửa thành decimal
        public Rate Rate { get; set; }
        [ForeignKey("OrderId")]
        public Order? Order { get; set; }
        [ForeignKey("ProductDetailId")]
        public ProductDetail? productDetail { get; set; }

    }
}
