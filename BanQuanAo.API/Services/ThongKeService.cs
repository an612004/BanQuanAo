using BanQuanAo.API.Data;
using BanQuanAo.API.IServices;
using BanQuanAo.Share.Models;
using BanQuanAo.Share.ViewModels;

namespace BanQuanAo.API.Services
{

    public class ThongKeService: IThongKeService
    {
        private readonly MyDbContext context;
        IUserService _userService;

        public ThongKeService(MyDbContext context, IUserService userService)
        {
            this.context = context;
            _userService = userService;
        }

        public async Task< ThongKeViewModel> ThongKe(string startDate, string endDate)
        {
            try
            {
                var soLuongThanhVien = context.Users.Count();
                var soLuongDonHangCho = context.Order.Where(x => x.OrderStatusId == Guid.Parse("1C54C2DD-2FA5-4041-9B94-FB613BEBDFBC")).Count();//ddang duoc xu ly
                var soLuongDonHangHuy = context.Order.Where(x => x.OrderStatusId == Guid.Parse("6C54C2DD-2FA5-4041-9B94-FB613BEBDFBC")).Count();//Huy
                var soLuongSanPham = context.ProductDetail.Sum(x => x.Quantity);
                User user;
                List<OrderItem> lstChiTietHoaDon = new List<OrderItem>();
                var start = Convert.ToDateTime(startDate);
                var end = Convert.ToDateTime(endDate);
                //lấy đơn thành công và đơn huỷ
                List<Order> lstHoaDon = context.Order.Where(x => (x.OrderStatusId == Guid.Parse("4C54C2DD-2FA5-4041-9B94-FB613BEBDFBC") || x.OrderStatusId == Guid.Parse("6C54C2DD-2FA5-4041-9B94-FB613BEBDFBC")) && x.Payment_Date >= start && x.Payment_Date <= end).ToList();
                //End
                var tongHoaDonTron = lstHoaDon.Count();
                var kh = await _userService.GetAll();
                var topKhach = lstHoaDon
                                    .GroupBy(hd => hd.UserId)
                                    .Select(g => new
                                    {
                                        IdKH = g.Key,
                                        TongHoaDon = g.Count(),
                                        TongTien = g.Sum(hd => hd.TotalAmoutAfterApplyingVoucher ?? 0)
                                    })
                                    .Join(kh,
                                          hd => hd.IdKH,
                                          kh => kh.Id,
                                          (hd, kh) => new TopKhachHangVM
                                          {
                                              idKH = kh.Id,
                                              ten = kh.UserName,
                                              sdt = kh.PhoneNumber,
                                              tonghoadon = hd.TongHoaDon,
                                              tongtien = hd.TongTien
                                          })
                                    .OrderByDescending(hd => hd.tongtien)
                                    .Take(5)
                                    .ToList();

                //Lấy biểu đồ cột
                foreach (var hoaDon in lstHoaDon.Where(x => x.OrderStatusId == Guid.Parse("4C54C2DD-2FA5-4041-9B94-FB613BEBDFBC")))//đơn thành công
                {
                    lstChiTietHoaDon.AddRange(context.OrderItem.Where(x => x.OrderId == hoaDon.Id));
                }
                List<ThongKeCotViewModel> thongKeCot = (from a in lstChiTietHoaDon
                                                        group a by a.ProductDetailId into g
                                                        select new ThongKeCotViewModel()
                                                        {
                                                            TenSP = g.Key.ToString(),
                                                            SoLuong = g.Sum(x => x.Quantity),
                                                        }).OrderByDescending(x => x.SoLuong).Take(10).ToList();
                ProductDetail chiTietSanPham;
                Colors mauSac;
                Size kichCo;
                Product sanPham;
                foreach (var item in thongKeCot)
                {
                    chiTietSanPham = context.ProductDetail.First(x => x.Id == new Guid(item.TenSP));
                    mauSac = context.Colors.First(x => x.ColorId == chiTietSanPham.ColorId);
                    kichCo = context.Size.First(x => x.Id == chiTietSanPham.SizeId);
                    sanPham = context.Product.First(x => x.Id == chiTietSanPham.ProductId);
                    item.TenSP = sanPham.ProductName + "_" + mauSac.ColorName + "_" + kichCo.SizeName;
                }
                //Lấy biểu đồ đường
                List<ThongKeDuongViewModel> thongKeDuong = new List<ThongKeDuongViewModel>();
                for (var i = start; i <= end; i = i.AddDays(1))
                {
                    var ngayDoanhThu = context.Order
                        .Where(x => x.OrderStatusId == Guid.Parse("4C54C2DD-2FA5-4041-9B94-FB613BEBDFBC") && x.Payment_Date.Value.Date == i.Date)
                        .Sum(x => x.TotalAmoutAfterApplyingVoucher - x.ShippingFee).Value;

                    var ngayChiPhi = context.Order
                        .Where(x => x.OrderStatusId == Guid.Parse("4C54C2DD-2FA5-4041-9B94-FB613BEBDFBC") && x.Payment_Date.Value.Date == i.Date)
                        .SelectMany(x => x.OrderItem) 
                        .Sum(oi => oi.productDetail.Price * oi.Quantity).Value; // Tổng chi phí nhập hàng

                    decimal ngayLoiNhuan = ngayDoanhThu - ngayChiPhi;

                    thongKeDuong.Add(new ThongKeDuongViewModel() { Ngay = i.Date, DoanhThu = ngayDoanhThu, LoiNhuan = ngayLoiNhuan });
                }

                
                return new ThongKeViewModel() { SoLuongThanhVien = soLuongThanhVien, SoLuongDonHang = soLuongDonHangCho, SoLuongDonHuy = soLuongDonHangHuy , SoLuongSanPham = soLuongSanPham,topKhachHang = topKhach, BieuDoCot = thongKeCot, BieuDoDuong = thongKeDuong.OrderBy(x => x.Ngay).ToList(), Start = start.ToString("MM/dd/yyyy"), End = end.ToString("MM/dd/yyyy") };
            }
            catch
            {
                return new ThongKeViewModel();
            }
        }
    }
}
