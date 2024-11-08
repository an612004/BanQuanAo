using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanQuanAo.Share.Models
{
    public class Brand
    {
        [Key]
        public Guid Id { get; set; }
        public string? BrandName { get; set; }
        public virtual List<Product>? Product { get; set; }
    }
}
