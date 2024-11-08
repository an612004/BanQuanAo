using BanQuanAo.API.Data;
using BanQuanAo.API.IServices;
using BanQuanAo.Share.Models;
using BanQuanAo.Share.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BanQuanAo.API.Services
{
	public class OrderService : IOrderService
	{
		private readonly MyDbContext _dbContext;
		public OrderService(MyDbContext myDbContext)
		{
			_dbContext = myDbContext;
		}
		public async Task<bool> Create(OrderVM item)
		{
			try
			{
				var order = new Order()
				{
					Id = item.Id,
					UserId = item.UserId,
					VoucherId = item.VoucherId,
					PaymentType = item.PaymentType,
					OrderStatusId = item.OrderStatusId,
					OrderCode = item.OrderCode,
					RecipientAddress = item.RecipientAddress,
					RecipientPhone = item.RecipientPhone,
					RecipientName = item.RecipientName,
					TotalAmout = item.TotalAmout,
					TotalAmoutAfterApplyingVoucher = item.TotalAmoutAfterApplyingVoucher,
					VoucherValue = item.VoucherValue,
					Create_Date = item.Create_Date,
					Payment_Date = item.Payment_Date,
					Delivery_Date = item.Delivery_Date,
					Description = item.Description,
					ShippingFee = item.ShippingFee,
					Ship_Date = item.Ship_Date,

				};
				await _dbContext.Order.AddAsync(order);
				await _dbContext.SaveChangesAsync();
				return true;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return false;
			}
		}

		public async Task<bool> Delete(Guid id)
		{
			try
			{
				var item = await _dbContext.Order.FirstOrDefaultAsync(c => c.Id == id);
				_dbContext.Remove(item);
				await _dbContext.SaveChangesAsync();
				return true;

			}
			catch (Exception e)
			{

				Console.WriteLine(e.Message);
				return false;
			}
		}

		public async Task<List<Order>> GetAll()
		{
			return await _dbContext.Order.ToListAsync();

		}

		public async Task<List<DonMuaChiTietVM>> getAllDonMuaChiTiet(Guid idHoaDon)
		{
			var lstDonMuaCT = await (from a in _dbContext.OrderItem
									 where a.OrderId == idHoaDon
									 join b in _dbContext.Order on a.OrderId equals b.Id

									 join d in _dbContext.ProductDetail on a.ProductDetailId equals d.Id
									 join e in _dbContext.Size on d.SizeId equals e.Id
									 join f in _dbContext.Colors on d.ColorId equals f.ColorId
									 join g in _dbContext.Product on d.ProductId equals g.Id
									 join x in _dbContext.Users on b.UserId equals x.Id
									 join c in _dbContext.Rate on a.Id equals c.Id into rateJoin
									 from c in rateJoin.DefaultIfEmpty()
									 select new DonMuaChiTietVM()
									 {
										 ID = b.Id,
										 NgayTao = b.Create_Date,
										 NgayThanhToan = b.Payment_Date,
										 Ship_Date = b.Ship_Date,
										 Delivery_Date = b.Delivery_Date,
										 Description = b.Description,
										 OrderCode = b.OrderCode,
										 TotalAmout = b.TotalAmout,
										 TotalAmoutAfterApplyingVoucher = b.TotalAmoutAfterApplyingVoucher,
										 TenNguoiNhan = b.RecipientName,
										 SDT = b.RecipientPhone,
										 DiaChi = b.RecipientAddress,
										 TienShip = b.ShippingFee,
										 TrangThaiGiaoHang = b.OrderStatusId,
										 PhuongThucThanhToan = b.PaymentType,
										 IDCTHD = a.Id,
										 DonGia = a.Price,
										 SoLuong = a.Quantity,
										 TenKichCo = e.SizeName,
										 TenMau = f.ColorName,
										 TenSanPham = g.ProductName,
										 DuongDan = _dbContext.ProductImage.First(c => c.ProductDetailId == d.Id).ImageUrl,
										 HinhThucGiamGia = b.VoucherId == null ? null : (_dbContext.Voucher.FirstOrDefault(c => c.Id == b.VoucherId)).Discount_Type,
										 GiaTriVC = b.VoucherId == null ? null : (_dbContext.Voucher.FirstOrDefault(c => c.Id == b.VoucherId)).Value,
										 TenNguoiDung = b.UserId == null ? null : (_dbContext.Users.FirstOrDefault(c => c.Id == b.UserId)).UserName,
										 TrangThaiDanhGia = c != null ? c.Status : 0

									 }).ToListAsync();
			return lstDonMuaCT;
		}
		public async Task<DonMuaChiTietVM> getAllDonMuaChiTiet1(Guid idhd)
		{
			var lstDonMuaCT = await (from a in _dbContext.OrderItem
									 where a.OrderId == idhd
									 join b in _dbContext.Order on a.OrderId equals b.Id
									 //join c in _dbContext.Rate on a.Id equals c.Id //ko hiểu sao thêm cái này lại trả về []
									 join d in _dbContext.ProductDetail on a.ProductDetailId equals d.Id
									 join e in _dbContext.Size on d.SizeId equals e.Id
									 join f in _dbContext.Colors on d.ColorId equals f.ColorId
									 join g in _dbContext.Product on d.ProductId equals g.Id
									 join x in _dbContext.Users on b.UserId equals x.Id
									 select new DonMuaChiTietVM()
									 {
										 ID = b.Id,
										 NgayTao = b.Create_Date,
										 NgayThanhToan = b.Payment_Date,
										 Ship_Date = b.Ship_Date,
										 Delivery_Date = b.Delivery_Date,
										 Description = b.Description,
										 OrderCode = b.OrderCode,
										 TotalAmout = b.TotalAmout,
										 TotalAmoutAfterApplyingVoucher = b.TotalAmoutAfterApplyingVoucher,
										 TenNguoiNhan = b.RecipientName,
										 SDT = b.RecipientPhone,
										 DiaChi = b.RecipientAddress,
										 TienShip = b.ShippingFee,
										 TrangThaiGiaoHang = b.OrderStatusId,
										 PhuongThucThanhToan = b.PaymentType,
										 IDCTHD = a.Id,
										 DonGia = a.Price,
										 SoLuong = a.Quantity,
										 TenKichCo = e.SizeName,
										 TenMau = f.ColorName,
										 TenSanPham = g.ProductName,
										 DuongDan = _dbContext.ProductImage.First(c => c.ProductDetailId == d.Id).ImageUrl,
										 HinhThucGiamGia = b.VoucherId == null ? null : (_dbContext.Voucher.FirstOrDefault(c => c.Id == b.VoucherId)).Discount_Type,
										 GiaTriVC = b.VoucherId == null ? null : (_dbContext.Voucher.FirstOrDefault(c => c.Id == b.VoucherId)).Value,
										 TenNguoiDung = b.UserId == null ? null : (_dbContext.Users.FirstOrDefault(c => c.Id == b.UserId)).UserName,

										 OrderItem = (from odi in _dbContext.OrderItem
													  join prd in _dbContext.ProductDetail on odi.ProductDetailId equals prd.Id
													  join pr in _dbContext.Product on prd.ProductId equals pr.Id
													  join cl in _dbContext.Colors on prd.ColorId equals cl.ColorId
													  join sz in _dbContext.Size on prd.SizeId equals sz.Id
													  where odi.OrderId == idhd
													  select new OrderItemCTViewModel1
													  {
														  Id = odi.Id,
														  OrderId = idhd,
														  ProductDetailId = prd.Id,
														  ProductName = pr.ProductName,
														  ProductId = pr.Id,
														  SizeId = sz.Id,
														  SizeName = sz.SizeName,
														  ColorId = cl.ColorId,
														  ColorName = cl.ColorName,
														  Quantity = odi.Quantity ?? 0,
														  PriceSale = odi.Price ?? 0,
														  ProductImages = (from pi in _dbContext.ProductImage
																		   where pi.ProductDetailId == prd.Id
																		   select pi.ImageUrl).ToList()
													  }).ToList()

									 }).FirstOrDefaultAsync();


			return lstDonMuaCT;
		}

		public async Task<Order> GetItem(Guid id)
		{
			return await _dbContext.Order.FindAsync(id);

		}

		public async Task<bool> Update(Guid id, Guid UserId, OrderVM item)
		{
			try
			{
				var order = await _dbContext.Order.FirstOrDefaultAsync(c => c.Id == id);

				order.UserId = item.UserId;
				order.VoucherId = item.VoucherId;
				order.OrderStatusId = item.OrderStatusId;
				order.OrderCode = item.OrderCode;
				order.RecipientAddress = item.RecipientAddress;
				order.RecipientPhone = item.RecipientPhone;
				order.RecipientName = item.RecipientName;
				order.TotalAmout = item.TotalAmout;
				order.TotalAmoutAfterApplyingVoucher = item.TotalAmoutAfterApplyingVoucher;
				order.VoucherValue = item.VoucherValue;
				order.Create_Date = item.Create_Date;
				order.Payment_Date = item.Payment_Date;
				order.Delivery_Date = item.Delivery_Date;
				order.Description = item.Description;
				order.ShippingFee = item.ShippingFee;
				order.Ship_Date = item.Ship_Date;
				_dbContext.Order.Update(order);
				await _dbContext.SaveChangesAsync();
				return true;
			}
			catch (Exception e)
			{

				Console.WriteLine(e.Message);
				return false;
			}
		}

		public async Task<bool> UpdateStatus(Guid OrderId, Guid StatusId)
		{
			try
			{
				var order = await _dbContext.Order.FirstOrDefaultAsync(c => c.Id == OrderId);
				order.OrderStatusId = StatusId;
				_dbContext.Update(order);
				await _dbContext.SaveChangesAsync();
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
		public async Task<bool> UpdateRank(int? point)
		{
			var ranks = await _dbContext.Rank.ToListAsync();
			if (ranks != null)
			{


				foreach (var item in ranks)
				{
					var user = await _dbContext.Users.FirstOrDefaultAsync(c => c.RankId == item.Id);
					user.Points = point;
					if (user.Points >= 0 && user.Points <= 1000000)
					{
						var ranknamne = await _dbContext.Rank.FirstOrDefaultAsync(c => c.Name == "Bạc");
						user.RankId = ranknamne.Id;
					}
					else if (user.Points >= 2000001 && user.Points <= 30000000)
					{
						var ranknamne = await _dbContext.Rank.FirstOrDefaultAsync(c => c.Name == "Vàng");
						user.RankId = ranknamne.Id;
					}
					else if (user.Points >= 30000001 && user.Points <= 10000000)
					{
						var ranknamne = await _dbContext.Rank.FirstOrDefaultAsync(c => c.Name == "Kim Cương");
						user.RankId = ranknamne.Id;
					}
					_dbContext.Users.Update(user);
					await _dbContext.SaveChangesAsync();
					return true;

				}
			}
			return false;
		}
		public async Task<bool> UpdateTrangThaiGiaoHang(Guid idHoaDon, Guid idtrangThai, Guid? idNhanVien)
		{
			var update = await _dbContext.Order.FirstOrDefaultAsync(p => p.Id == idHoaDon);
			var chitiethoadon = await _dbContext.OrderItem.Where(p => p.OrderId == idHoaDon).ToListAsync();

			if (update != null)
			{
				if (idtrangThai == Guid.Parse("6C54C2DD-2FA5-4041-9B94-FB613BEBDFBC"))
				{
					foreach (var item in chitiethoadon)
					{
						var CTsanPham = await _dbContext.ProductDetail.FirstOrDefaultAsync(p => p.Id == item.ProductDetailId);
						CTsanPham.Quantity += item.Quantity;

						var product = await _dbContext.Product.FindAsync(CTsanPham.ProductId);
						if (product != null)
						{
							product.AvailableQuantity += item.Quantity;
							_dbContext.Product.Update(product);
						}

						_dbContext.ProductDetail.Update(CTsanPham);
					}
				}

				if (idtrangThai == Guid.Parse("4C54C2DD-2FA5-4041-9B94-FB613BEBDFBC"))
				{
					//sửa thành id khách
					var kh = await _dbContext.Users.FirstOrDefaultAsync(c => c.Id == update.UserId);
					var hoadon = await _dbContext.Order.FirstOrDefaultAsync(c => c.Id == idHoaDon);
					if (kh != null && hoadon != null)
					{
						kh.Points += Convert.ToInt32(hoadon.TotalAmout);
						UpdateRank(kh.Points);
					}

					update.Payment_Date ??= DateTime.Now;
					update.Delivery_Date ??= DateTime.Now;
				}

				update.OrderStatusId = idtrangThai;
				update.UserId = update.UserId;
				_dbContext.Order.Update(update);

				try
				{
					await _dbContext.SaveChangesAsync();
					return true;
				}
				catch (Exception ex)
				{
					// Handle exception and possibly rollback changes
					Console.WriteLine(ex.Message);
					return false;
				}
			}
			else
			{
				return false;
			}
		}
		public bool ThanhCong(Guid idHoaDon, Guid? idNhanVien) // Chỉ cho đơn online
		{
			try
			{
				var hd = _dbContext.Order.FirstOrDefault(c => c.Id == idHoaDon);
				if (hd != null)
				{
					hd.OrderStatusId = Guid.Parse("4C54C2DD-2FA5-4041-9B94-FB613BEBDFBC");
					hd.UserId = hd.UserId;
					hd.Delivery_Date = DateTime.Now;
					hd.Payment_Date = DateTime.Now;
					_dbContext.Order.Update(hd);
					_dbContext.SaveChanges(); // Chờ đợi lưu thay đổi vào cơ sở dữ liệu

					// Cộng tích điểm cho khách
					if (hd.UserId != Guid.Parse("2FA6148D-B530-421F-878E-CE4D54BFC6AB")) // nếu userid bill khác khách vãng lai thì cộng điểm
					{
						var kh = _dbContext.Users.FirstOrDefault(c => c.Id == idNhanVien);
						if (kh != null)
						{
							kh.Points += Convert.ToInt32(hd.TotalAmout);
							UpdateRank(kh.Points);
						}
					}

					return true;

				}
				return false;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task<bool> HuyHD(Guid idhd, Guid idnv)
		{
			try
			{
				var hd = _dbContext.Order.Where(c => c.Id == idhd).FirstOrDefault();
				//Update hd
				hd.UserId = hd.UserId;
				hd.OrderStatusId = Guid.Parse("6C54C2DD-2FA5-4041-9B94-FB613BEBDFBC");
				//hd.TongTien = 0;
				_dbContext.Order.Update(hd);
				await _dbContext.SaveChangesAsync();

				// Cộng lại số lượng hàng
				var chitiethoadon = await _dbContext.OrderItem.Where(p => p.OrderId == idhd).ToListAsync();


				if (chitiethoadon != null)
				{


					foreach (var item in chitiethoadon)
					{
						var CTsanPham = await _dbContext.ProductDetail.FirstOrDefaultAsync(p => p.Id == item.ProductDetailId);
						CTsanPham.Quantity += item.Quantity;

						var product = await _dbContext.Product.FindAsync(CTsanPham.ProductId);
						if (product != null)
						{
							product.AvailableQuantity += item.Quantity;
							_dbContext.Product.Update(product);
							await _dbContext.SaveChangesAsync();

						}

						_dbContext.ProductDetail.Update(CTsanPham);
						await _dbContext.SaveChangesAsync();

					}

				}
				// Cộng lại số lượng voucher nếu áp dụng
				if (hd.VoucherId != null)
				{
					var vc = await _dbContext.Voucher.FirstOrDefaultAsync(c => c.Id == hd.VoucherId);
					var uservc = await _dbContext.VoucherUser.FirstOrDefaultAsync(c => c.VoucherId == vc.Id && c.UserId == hd.UserId);
					vc.Quantity += 1;
					uservc.Status = true;
					_dbContext.Voucher.Update(vc);
					_dbContext.VoucherUser.Update(uservc);
					await _dbContext.SaveChangesAsync();
				}
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}
		public bool UpdateGhiChuHD(Guid idhd, Guid idnv, string ghichu)
		{
			try
			{
				var hd = _dbContext.Order.FirstOrDefault(c => c.Id == idhd);
				if (ghichu == "null")
				{
					hd.Description = null;
					hd.UserId = hd.UserId;
				}
				else
				{
					hd.Description = ghichu;
				}
				_dbContext.Order.Update(hd);
				_dbContext.SaveChangesAsync();
				return true;

			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public async Task<QLHDViewModel> GetQLHDWithDetails(Guid orderId)
		{
			var qlhdWithDetails = await (from od in _dbContext.Order
										 join ods in _dbContext.OrderStatus on od.OrderStatusId equals ods.Id
										 where od.Id == orderId
										 select new QLHDViewModel
										 {
											 Id = od.Id,
											 OrderStatusId = od.OrderStatusId,
											 OrderStatusName = ods.OrderStatusName,
											 PaymentType = od.PaymentType,
											 OrderCode = od.OrderCode,
											 RecipientName = od.RecipientName,
											 RecipientAddress = od.RecipientAddress,
											 RecipientPhone = od.RecipientPhone,
											 TotalAmout = od.TotalAmout,
											 VoucherValue = od.VoucherValue,
											 TotalAmoutAfterApplyingVoucher = od.TotalAmoutAfterApplyingVoucher,
											 ShippingFee = od.ShippingFee,
											 Create_Date = od.Create_Date,
											 Ship_Date = od.Ship_Date,
											 Payment_Date = od.Payment_Date,
											 Delivery_Date = od.Delivery_Date,
											 Description = od.Description,

											 OrderItem = (from odi in _dbContext.OrderItem
														  join prd in _dbContext.ProductDetail on odi.ProductDetailId equals prd.Id
														  join pr in _dbContext.Product on prd.ProductId equals pr.Id
														  join cl in _dbContext.Colors on prd.ColorId equals cl.ColorId
														  join sz in _dbContext.Size on prd.SizeId equals sz.Id
														  where odi.OrderId == od.Id
														  select new OrderItemCTViewModel
														  {
															  Id = odi.Id,
															  OrderId = od.Id,
															  ProductDetailId = prd.Id,
															  ProductName = pr.ProductName,
															  ProductId = pr.Id,
															  SizeId = sz.Id,
															  SizeName = sz.SizeName,
															  ColorId = cl.ColorId,
															  ColorName = cl.ColorName,
															  Quantity = odi.Quantity ?? 0,
															  PriceSale = odi.Price ?? 0,
															  ProductImages = (from pi in _dbContext.ProductImage
																			   where pi.ProductDetailId == prd.Id
																			   select pi.ImageUrl).ToList()
														  }).ToList()
										 }).FirstOrDefaultAsync();

			return qlhdWithDetails;
		}

		public Task<bool> HuyDon(Guid OrderId, Guid StatusId)
		{
			throw new NotImplementedException();
		}

        public async Task<QLHDViewModel> GetDonMuaByOrderCode(string orderCode)
        {
			var chitietdon = await (from od in _dbContext.Order
										 join ods in _dbContext.OrderStatus on od.OrderStatusId equals ods.Id
										 where od.OrderCode == orderCode 
										 select new QLHDViewModel
										 {
											 Id = od.Id,
											 OrderStatusId = od.OrderStatusId,
											 OrderStatusName = ods.OrderStatusName,
											 PaymentType = od.PaymentType,
											 OrderCode = od.OrderCode,
											 RecipientName = od.RecipientName,
											 RecipientAddress = od.RecipientAddress,
											 RecipientPhone = od.RecipientPhone,
											 TotalAmout = od.TotalAmout,
											 VoucherValue = od.VoucherValue,
											 TotalAmoutAfterApplyingVoucher = od.TotalAmoutAfterApplyingVoucher,
											 ShippingFee = od.ShippingFee,
											 Create_Date = od.Create_Date,
											 Ship_Date = od.Ship_Date,
											 Payment_Date = od.Payment_Date,
											 Delivery_Date = od.Delivery_Date,
											 Description = od.Description,

											 OrderItem = (from odi in _dbContext.OrderItem
														  join prd in _dbContext.ProductDetail on odi.ProductDetailId equals prd.Id
														  join pr in _dbContext.Product on prd.ProductId equals pr.Id
														  join cl in _dbContext.Colors on prd.ColorId equals cl.ColorId
														  join sz in _dbContext.Size on prd.SizeId equals sz.Id
														  where odi.OrderId == od.Id
														  select new OrderItemCTViewModel
														  {
															  Id = odi.Id,
															  OrderId = od.Id,
															  ProductDetailId = prd.Id,
															  ProductName = pr.ProductName,
															  ProductId = pr.Id,
															  SizeId = sz.Id,
															  SizeName = sz.SizeName,
															  ColorId = cl.ColorId,
															  ColorName = cl.ColorName,
															  Quantity = odi.Quantity ?? 0,
															  PriceSale = odi.Price ?? 0,
															  ProductImages = (from pi in _dbContext.ProductImage
																			   where pi.ProductDetailId == prd.Id
																			   select pi.ImageUrl).ToList()
														  }).ToList()
										 }).FirstOrDefaultAsync();


            return chitietdon;
        }

    }
}
