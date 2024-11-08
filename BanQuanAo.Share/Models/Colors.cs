using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanQuanAo.Share.Models
{
    public class Colors
    {
        [Key]
        public Guid ColorId { get; set; }
        public string? ColorName { get; set; }
        public string? ColorCode { get; set; }
        public virtual List<ProductDetail>? ProductDetails { get; set; }
    }
}
