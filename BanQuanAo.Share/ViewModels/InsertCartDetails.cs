using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanQuanAo.Share.ViewModels
{
    public class InsertCartDetails
    {
        public Guid Id { get; set; }
        public Guid CartId { get; set; }
        public Guid ProductDetail_ID { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
    }
}
