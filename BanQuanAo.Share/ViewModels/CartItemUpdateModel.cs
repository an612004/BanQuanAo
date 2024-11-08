using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanQuanAo.Share.ViewModels
{
    public class CartItemUpdateModel
    {
        public Guid ProductId { get; set; }
        public int UpdatedQuantity { get; set; }
    }
}
