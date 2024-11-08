using BanQuanAo.API.Data;
using BanQuanAo.API.IServices;
using BanQuanAo.Share.Models;
using Microsoft.EntityFrameworkCore;

namespace BanQuanAo.API.Services
{
    public class RateService : IRateService
    {

        private readonly MyDbContext _dbContext;

        public RateService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> Create(Rate item)
        {
            try
            {
                var rate = new Rate()
                {
                    OrderItemId = item.OrderItemId,
                    Content = item.Content,
                    ImageUrl = item.ImageUrl,
                    Reply = item.Reply,
                    Rating = item.Rating,
                };
                await _dbContext.Rate.AddAsync(rate);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> Delete(Guid id,Guid orderid)
        {
            try
            {
                var item = await _dbContext.Rate.FirstOrDefaultAsync(c => c.Id == id&&c.OrderItemId == orderid);
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

        public async Task<List<Rate>> GetAll()
        {
            return await _dbContext.Rate.ToListAsync();
        }

        public async Task<Rate> GetItem(Guid id)
        {
            return await _dbContext.Rate.FindAsync(id);
        }

        public async Task<List<Rate>> GetListRatebyorderId(Guid orderId)
        {
            var rates = await _dbContext.Rate.Where(rate => rate.OrderItemId == orderId).ToListAsync();
            return rates;
        }

        public async Task<bool> Update(Guid orderid, int star, string? comment)
        {
            try
            {
                var rates = await _dbContext.Rate.FirstOrDefaultAsync(c => c.Id == orderid);
                rates.Content = comment;
                rates.Rating = star;
                rates.Status = 1;
                _dbContext.Rate.Update(rates);
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
