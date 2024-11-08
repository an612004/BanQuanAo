using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanQuanAo.Share.Models
{
    public class Voucher
    {
        [Key]
        public Guid Id { get; set; }
        public string? Code { get; set; }
        public int? Quantity { get; set; }
        public int? Value { get; set; }
        public int? Discount_Type { get; set; } // hinh thuc giam gia, 1 là %, 0 là theo tiền mặt
        public int? Minimum_order_value { get; set; }
        public int? MaxDiscountAmount { get; set; }
        public string? Description { get; set; } 
        public DateTime? Create_Date { get; set; }
        public DateTime? Start_Date { get; set; }
        public DateTime? End_Date { get; set; }
        public bool? Status { get; set; } 
        public virtual List<Order>? Orders { get; set; }
        public virtual List<UserVoucher>? UserVouchers { get; set; }
    }
}
