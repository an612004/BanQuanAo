using BanQuanAo.Share.Models;

namespace BanQuanAo.API.IServices
{
    public interface IVoucherService
    {
        public Task<bool> Create(Voucher item);

        public Task<bool> Delete(Guid id);

        public Task<List<Voucher>> GetAll();

        public Task<Voucher> GetItem(Guid id);
        public Task<bool> TangGiamSoLuongTheoId(Guid voucherId,bool tanggiam);
        public Task<Voucher> GetItemByCode(string code);

        public Task<bool> Update(Guid id, Voucher rank);
    }
}
