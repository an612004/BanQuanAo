using BanQuanAo.Share.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BanQuanAo.API.IServices
{
    public interface IUserVoucherService
    {
        public Task<bool> Create(UserVoucher item);

        public Task<bool> Delete(Guid id);

        public Task<List<UserVoucher>> GetAll();

        public Task<UserVoucher> GetItem(Guid id);
        public Task<UserVoucher> GetSoHuu(Guid voucherId,Guid userId);
        public Task<bool> UpdateTrangThai(Guid voucherId,Guid userId, bool status);

        public Task<bool> Update(Guid id, UserVoucher userVoucher);
    }
}
