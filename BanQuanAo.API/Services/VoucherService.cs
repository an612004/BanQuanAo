using BanQuanAo.API.Data;
using BanQuanAo.API.IServices;
using BanQuanAo.Share.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;
using System.Reflection.Metadata.Ecma335;

namespace BanQuanAo.API.Services
{
    public class VoucherService : IVoucherService
    {
        private readonly MyDbContext _dbContext;

        public VoucherService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> Create(Voucher item)
        {
            try
            {
                var voucher = new Voucher()
                {
                    Id = item.Id,
                    Code = item.Code,
                    Quantity = item.Quantity,
                    Value = item.Value,
                    Discount_Type = item.Discount_Type,
                    Minimum_order_value = item.Minimum_order_value,
                    Create_Date = item.Create_Date,
                    Start_Date = item.Start_Date,
                    Description = item.Description,
                    MaxDiscountAmount = item.MaxDiscountAmount ,
                    End_Date = item.End_Date,
                    Status = item.Status,
                };
                await _dbContext.Voucher.AddAsync(voucher);
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
                var item = await _dbContext.Voucher.FirstOrDefaultAsync(c => c.Id == id);
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

        public async Task<List<Voucher>> GetAll()
        {
            return await _dbContext.Voucher.ToListAsync();
        }
        public async Task<bool> TangGiamSoLuongTheoId(Guid voucherId, bool tanggiam)
        {
            try
            {
                var voucher = await _dbContext.Voucher.FirstOrDefaultAsync(c => c.Id == voucherId);
                voucher.Quantity += tanggiam ? 1 : -1;
                _dbContext.Voucher.Update(voucher);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Voucher> GetItem(Guid id)
        {
            return await _dbContext.Voucher.FindAsync(id);
        }

        public async Task<Voucher> GetItemByCode(string code)
        {
            return await _dbContext.Voucher.FirstOrDefaultAsync(c => c.Code.ToLower() == code.ToLower());

        }

        public async Task<bool> Update(Guid id, Voucher rank)
        {
            try
            {
                var VoucherForcus = await _dbContext.Voucher.FirstOrDefaultAsync(c => c.Id == id);
                VoucherForcus.Minimum_order_value = rank.Minimum_order_value;
                VoucherForcus.Quantity = rank.Quantity;
                VoucherForcus.Start_Date = rank.Start_Date;
                VoucherForcus.End_Date = rank.End_Date;
                VoucherForcus.Description = rank.Description;
                VoucherForcus.MaxDiscountAmount  = rank.MaxDiscountAmount ;
                VoucherForcus.Status = rank.Status;
                _dbContext.Voucher.Update(VoucherForcus);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}
