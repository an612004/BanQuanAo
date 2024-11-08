using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanQuanAo.Share.ViewModels
{
    public class DonMuaChiTietVM
    {
        public Guid ID { get; set; }
        public DateTime? NgayTao { get; set; }
        public DateTime? NgayThanhToan { get; set; }
        public string? OrderCode { get; set; } // mã bill
        public decimal? TotalAmout { get; set; } // tổng tiền trước khi áp dụng
        public decimal? TotalAmoutAfterApplyingVoucher { get; set; } // giá sau khi áp voucher
        public DateTime? Ship_Date { get; set; } // ngày Ship
        public DateTime? Delivery_Date { get; set; } // ngày nhận hàng 
        public string? Description { get; set; } // mô tả
        public string? TenNguoiNhan { get; set; }
        public string? SDT { get; set; }
        public string? DiaChi { get; set; }
        public decimal? TienShip { get; set; }
        public string? PhuongThucThanhToan { get; set; }
        public Guid? TrangThaiGiaoHang { get; set; }
        public Guid IDCTHD { get; set; }
        public int? DonGia { get; set; }
        public int? SoLuong { get; set; }
        public string? TenKichCo { get; set; }
        public string? TenMau { get; set; }
        public string? TenSanPham { get; set; }
        public string? DuongDan { get; set; }
        public string? TenNguoiDung { get; set; }
        public int? TrangThaiDanhGia { get; set; }
        public int? GiaTriVC { get; set; }
        public int? HinhThucGiamGia { get; set; }
        public virtual List<OrderItemCTViewModel1>? OrderItem { get; set; }

    }
    public class OrderItemCTViewModel1
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductDetailId { get; set; }
        public string ProductName { get; set; }
        public Guid ProductId { get; set; }
        public Guid SizeId { get; set; }
        public string SizeName { get; set; }
        public Guid ColorId { get; set; }
        public string ColorName { get; set; }
        public int Quantity { get; set; }
        public decimal PriceSale { get; set; }
        public List<string> ProductImages { get; set; }
    }
}
