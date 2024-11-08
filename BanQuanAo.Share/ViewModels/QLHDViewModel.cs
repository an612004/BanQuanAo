using BanQuanAo.Share.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanQuanAo.Share.ViewModels
{
    public class QLHDViewModel
    {
        public Guid Id { get; set; }
        public Guid? OrderStatusId { get; set; }
        public string? PaymentType { get; set; }
        public Guid? VoucherId { get; set; }
        public string? OrderCode { get; set; } // mã bill
        public string? OrderStatusName { get; set; } // mã bill
        public string? RecipientName { get; set; }
        public string? RecipientAddress { get; set; }// địa chỉ
        public string? RecipientPhone { get; set; }// sdt
        public decimal? TotalAmout { get; set; } // tổng tiền trước khi áp dụng
        public decimal? VoucherValue { get; set; } // giá trị voucher
        public decimal? TotalAmoutAfterApplyingVoucher { get; set; } // giá sau khi áp voucher
        public decimal? ShippingFee { get; set; } // phí ship
        public DateTime? Create_Date { get; set; } // ngày tạo bill
        public DateTime? Ship_Date { get; set; } // ngày Ship
        public DateTime? Payment_Date { get; set; } // ngày thanh toán
        public DateTime? Delivery_Date { get; set; } // ngày nhận hàng
        public string? Description { get; set; } // mô tả
        public string? TenNguoiDung { get; set; } // mô tả

        public OrderStatus? OrderStatus { get; set; }

        public virtual List<OrderItemCTViewModel>? OrderItem { get; set; }
    }
    public class OrderItemCTViewModel
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
