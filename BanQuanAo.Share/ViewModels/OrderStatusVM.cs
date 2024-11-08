using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanQuanAo.Share.ViewModels
{
    public class OrderStatusVM
    {
        [Key]
        public Guid Id { get; set; }
        public string? OrderStatusName { get; set; }
    }
}
