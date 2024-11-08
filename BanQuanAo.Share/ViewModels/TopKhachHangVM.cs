using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanQuanAo.Share.ViewModels
{
    public class TopKhachHangVM
    {
        public Guid idKH { get; set; }
        public string ten { get; set; }
        public string sdt { get; set; }
        public int tonghoadon { get; set; }
        public decimal? tongtien { get; set; }
    }
}
