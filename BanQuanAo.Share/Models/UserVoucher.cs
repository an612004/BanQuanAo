using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanQuanAo.Share.Models
{
    public class UserVoucher
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid VoucherId { get; set; }
        public bool? Status { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }
        [ForeignKey("VoucherId")]
        public Voucher? Voucher { get; set; }
    }
}
