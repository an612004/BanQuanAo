using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanQuanAo.Share.Models
{
    public class Rate
    {
        [Key]
        public Guid Id { get; set; }
        public Guid OrderItemId { get; set; }
        public string? Content { get; set; }
        public string? Reply { get; set; }
        public string? ImageUrl { get; set; }
        public float? Rating { get; set; }
        public int Status { get; set; }
        //thêm thuộc tính reply
        [ForeignKey("OrderItemId")]
        public OrderItem? OrderItem { get; set; }


    }
}
