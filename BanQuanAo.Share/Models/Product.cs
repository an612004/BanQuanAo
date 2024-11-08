using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanQuanAo.Share.Models
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? BrandId { get; set; }
        public Guid? MaterialId { get; set; }
        public string? ProductName { get; set; }
        public int? AvailableQuantity { get; set; }
        public DateTime? Create_At { get; set; }
        public DateTime? Update_At { get; set; }
        public string? Description { get; set; }
        public string? Long_Description { get; set; }
        public bool? Status { get; set; }


        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }

        [ForeignKey("BrandId")]
        public Brand? Brand { get; set; }
        [ForeignKey("MaterialId")]
        public Material? Material { get; set; }

        public virtual List<ProductDetail>? ProductDetails { get; set; }
        public virtual List<WishList>? WishLists { get; set; }
    }
}
