using BanQuanAo.API.Data;
using BanQuanAo.API.IServices;
using BanQuanAo.Share.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;

namespace BanQuanAo.API.Services
{
    public class UserVoucherService : IUserVoucherService
    {
        private readonly MyDbContext _dbContext;

        public UserVoucherService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> Create(UserVoucher item)
        {
            try
            {
                var uservoucher = new UserVoucher()
                {
                    UserId = item.UserId,
                    VoucherId = item.VoucherId,
                    Status = item.Status
                 
                };
                await _dbContext.VoucherUser.AddAsync(uservoucher);
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
                var item = await _dbContext.VoucherUser.FirstOrDefaultAsync(c => c.Id == id);
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

        public async Task<List<UserVoucher>> GetAll()
        {
            return await _dbContext.VoucherUser.ToListAsync();
        }

        public async Task<UserVoucher> GetSoHuu(Guid voucherId, Guid userId)
        {
            try
            {
                return await _dbContext.VoucherUser.Where(c => c.VoucherId == voucherId && c.UserId == userId).FirstOrDefaultAsync();
            }
            catch (Exception)
            {

                return null;
            }
        }

        public async Task<UserVoucher> GetItem(Guid id)
        {
            return await _dbContext.VoucherUser.FindAsync(id);
        }

        public async Task<bool> Update(Guid id, UserVoucher userVoucher)
        {
            try
            {
                var rates = await _dbContext.VoucherUser.FirstOrDefaultAsync(c => c.Id == id);

                rates.UserId = userVoucher.UserId;
                rates.VoucherId = userVoucher.VoucherId;
                rates.Status = userVoucher.Status;
              

                _dbContext.VoucherUser.Update(rates);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> UpdateTrangThai(Guid voucherId, Guid userId, bool status)
        {
            try
            {
                var uservoucher = await _dbContext.VoucherUser.Where(c => c.VoucherId == voucherId && c.UserId == userId).FirstOrDefaultAsync();
                uservoucher.Status = status;
                _dbContext.VoucherUser.Update(uservoucher);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
