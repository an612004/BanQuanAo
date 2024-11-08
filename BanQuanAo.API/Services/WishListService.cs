using BanQuanAo.API.Data;
using BanQuanAo.API.IServices;
using BanQuanAo.Share.Models;
using BanQuanAo.Share.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;

namespace BanQuanAo.API.Services
{
    public class WishListService : IWishListService
    {
        private MyDbContext _dbContext;
        private IUserService _userService;
        private IProductService _productService;

        public WishListService(MyDbContext dbContext, IUserService userService, IProductService productService)
        {
            _dbContext = dbContext;
            _userService = userService;
            _productService = productService;
        }

        public async Task<bool> Create(Guid UserId, Guid ProductId)
        {
            try
            {
                //var idp = _dbContext.Product.FirstOrDefault(c => c.Id == ProductId).Id;
                //var iduser = _dbContext.Users.FirstOrDefault(c => c.Id == UserId).Id;
                WishList wishList = new WishList()
                {
                    Id = Guid.NewGuid(),
                    UserId = UserId,
                    ProductId = ProductId
                };
                await _dbContext.WishList.AddAsync(wishList);
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
                var result = await _dbContext.WishList.FindAsync(id);
                _dbContext.WishList.Remove(result);
                await _dbContext.SaveChangesAsync();
                return true;

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
                return false;
            }
        }
        public async Task<List<WishListVM>> GetAll()
        {
            //var query = from wishList in _dbContext.WishList
            //            join product in _dbContext.Product on wishList.ProductId equals product.Id
            //            join productDetail in _dbContext.ProductDetail on product.Id equals productDetail.ProductId
            //            join user in _dbContext.Users on wishList.UserId equals user.Id

            //            select new WishListVM()
            //            {
            //                Id = wishList.Id,
            //                ProductId = product.Id,
            //                UserId = user.Id,
            //                ProductName = product.ProductName,
            //                PriceSale = productDetail.PriceSale,
            //                Images = _dbContext.ProductImage
            //                            .Where(img => img.ProductDetailId == productDetail.Id)
            //                            .Select(img => img.ImageUrl)
            //                            .ToList()
            //            };
            var query = from a in _dbContext.WishList
                        join b in _dbContext.Product on a.ProductId equals b.Id
                        join c in _dbContext.Users on a.UserId equals c.Id
                        select new WishListVM()
                        {
                            Id = a.Id,
                            ProductName = b.ProductName,
                            UserId = c.Id,
                            ProductId = b.Id
                        };
            return query.ToList();
        }


        public async Task<WishList> GetItem(Guid id)
        {
            return await _dbContext.WishList.FindAsync(id);
        }

    }
}
