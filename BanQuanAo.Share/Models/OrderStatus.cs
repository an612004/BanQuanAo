using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanQuanAo.Share.Models
{
    public class OrderStatus
    {
        [Key]
        public Guid Id { get; set; }
        public string? OrderStatusName { get; set; }
        public virtual List<Order>? Order { get; set; }
    }
}
