using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanQuanAo.Share.Models
{
    public class Category
    {
        [Key]
        public Guid Id { get; set; }
        public string? CategoryName { get; set; }
        public Guid? IdCategory { get; set; }
        [ForeignKey("IdCategory")]
        public virtual  Category? category { get; set; }
        public virtual List<Product>? Product { get; set; }
    }
}
