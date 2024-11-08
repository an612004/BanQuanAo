using BanQuanAo.API.Data;
using BanQuanAo.API.IServices;
using BanQuanAo.Share.Models;
using Microsoft.EntityFrameworkCore;

namespace BanQuanAo.API.Services
{
    public class CartService : ICartService
    {
        private readonly MyDbContext _dbContext;

        public CartService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> Create(Cart item)
        {
            try
            {
                var cart = new Cart()
                {
                    UserId = item.UserId,

                };
                await _dbContext.Cart.AddAsync(cart);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<Cart> GetItem(Guid userid)
        {
            return await _dbContext.Cart.FirstOrDefaultAsync(c=>c.UserId==userid);
        }
    }
}
