using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanQuanAo.Share.Models
{
    public class Size
    {
        [Key]
        public Guid Id { get; set; }
        public string? SizeName { get; set; }
        public double? Width { get; set; }
        public double? Height { get; set; }
        public virtual List<ProductDetail>? ProductDetails { get; set; }
    }
}
