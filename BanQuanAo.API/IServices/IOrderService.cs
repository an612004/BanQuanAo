using BanQuanAo.Share.Models;
using BanQuanAo.Share.ViewModels;

namespace BanQuanAo.API.IServices
{
    public interface IOrderService
    {
        public Task<bool> Create(OrderVM item);

        public Task<bool> Delete(Guid id);
        public Task<bool> UpdateStatus(Guid OrderId,Guid StatusId);
        public Task<bool> HuyDon(Guid OrderId,Guid StatusId);
        public  Task<QLHDViewModel> GetQLHDWithDetails(Guid orderId);
        public Task<List<Order>> GetAll();

        public Task<Order> GetItem(Guid id);
        public Task<List<DonMuaChiTietVM>> getAllDonMuaChiTiet(Guid idHoaDon);
        public Task<QLHDViewModel> GetDonMuaByOrderCode(string? orderCode);

        public Task<bool> Update(Guid id, Guid UserId, OrderVM item);
        public  Task<bool> UpdateTrangThaiGiaoHang(Guid idHoaDon, Guid idtrangThai, Guid? idNhanVien);
        public  bool ThanhCong(Guid idHoaDon, Guid? idNhanVien);
        public  Task<bool> HuyHD(Guid idhd, Guid idnv);

        public Task<bool> UpdateRank(int? point);
        public  Task<DonMuaChiTietVM> getAllDonMuaChiTiet1(Guid idhd);

        public bool UpdateGhiChuHD(Guid idhd, Guid idnv, string ghichu);

    }
}
