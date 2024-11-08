using BanQuanAo.API.Data;
using BanQuanAo.API.IServices;
using BanQuanAo.Share.Models;
using BanQuanAo.Share.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BanQuanAo.API.Services
{
    public class CartItemService : ICartItemService
    {
        private readonly MyDbContext _dbContext;

        public CartItemService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> AddCartItem(CartItem item)
        {
            try
            {
                var cartItem = new CartItem()
                {
                    Id = item.Id,
                    CartId = item.CartId,
                    ProductDetail_ID = item.ProductDetail_ID,
                    Quantity = item.Quantity,
                    Price = item.Price,
                };
                await _dbContext.CartItem.AddAsync(cartItem);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpGet]
        public async Task<bool> DeleteCartItem( Guid CartId)
        {
            try
            {
                var item =   _dbContext.CartItem.Where(c => c.CartId == CartId);
                _dbContext.CartItem.RemoveRange(item);
                await _dbContext.SaveChangesAsync();
                return true;

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
                return false;
            }
        }
        [HttpGet]
        public async Task<bool> Delete1Item( Guid CartItemId)
        {
            try
            {
                var item =await   _dbContext.CartItem.FindAsync(CartItemId);
                _dbContext.CartItem.Remove(item);
                await _dbContext.SaveChangesAsync();
                return true;

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<List<CartItem>> GetAll()
        {
            return await _dbContext.CartItem.ToListAsync();
        }

        public async Task<IEnumerable<CartItem>> GetAllCartItemsByCartId(Guid cariId)
        {
            try
            {
                var model = await _dbContext.CartItem.Where(x => x.CartId == cariId).ToListAsync();
                return model;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<bool> UpdateCartItem(Guid cartItemId, int? newquantity, int? newPrice)
        {
            try
            {
                if (cartItemId != null)
                {
                    var item = await _dbContext.CartItem.FirstOrDefaultAsync(c => c.Id == cartItemId);
                    item.Quantity = newquantity;
                    item.Price = newPrice;
                    _dbContext.CartItem.Update(item);
                    await _dbContext.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> UpdateQuantityCartItem(CartItem cartItem)
        {
            _dbContext.Update(cartItem);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<CartItem> GetCartItemsByCartIds(Guid ?cartItemId)
        {
            var item = await _dbContext.CartItem.FirstOrDefaultAsync(c => c.Id == cartItemId);
            return item;
        }

        public async Task<bool> UpdateQuantity(Guid cartItemId, int? newquantity)
        {
            try
            {
                var cartitem = await _dbContext.CartItem.FirstOrDefaultAsync(c => c.Id == cartItemId);
                if (cartitem != null)
                {
                    cartitem.Quantity = newquantity;
                    _dbContext.CartItem.Update(cartitem);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
