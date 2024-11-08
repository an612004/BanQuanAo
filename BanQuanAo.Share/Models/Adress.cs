using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanQuanAo.Share.Models
{
    public class Adress
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public string? Province { get; set; } // Tỉnh/Thành phố
        public string? District { get; set; } // Quận/Huyện
        public string? Ward { get; set; } // Xã
        public bool? IsDefault { get; set; } // is default
        public string? DescriptionAddress { get; set; } // is địa chỉ chi tiết demo
            

        [ForeignKey("UserId")]
        public User? User{ get; set; }
    }
}