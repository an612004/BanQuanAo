using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanQuanAo.Share.ViewModels
{
    public class ThongKeViewModel
    {
        public int SoLuongThanhVien { get; set; }
        public int SoLuongDonHang { get; set; }//Don Hang Cho
        public int SoLuongDonHuy{ get; set; }//Don Hang Huy
        public int? SoLuongSanPham { get; set; }
        public List<TopKhachHangVM>? topKhachHang { get; set; }
        public List<ThongKeCotViewModel> BieuDoCot { get; set; }
        public List<ThongKeDuongViewModel> BieuDoDuong { get; set; }
        public List<ThongKeTronViewModel> BieuDoTron { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
    }
}
