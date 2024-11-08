using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanQuanAo.Share.ViewModels
{
    public class ThongKeDuongViewModel
    {
        public DateTime Ngay { get; set; }
        public decimal DoanhThu { get; set; }//Chỉ tính đơn thành công
        public decimal LoiNhuan { get; set; }//Chỉ tính đơn thành công
    }
}
