using BanQuanAo.API.Data;
using BanQuanAo.API.IServices;
using BanQuanAo.Share.Models;
using BanQuanAo.Share.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BanQuanAo.API.Services
{
    public class ProductImageService : IProductImageService
    {
        private MyDbContext _dbContext;

        public ProductImageService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Create(ProductImageVM item)
        {
            try
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                // Tạo thư mục nếu nó chưa tồn tại
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string imgPath = Path.Combine(path, item.ImageFile.FileName);

                using (var stream = new FileStream(imgPath, FileMode.Create))
                {
                    await item.ImageFile.CopyToAsync(stream);
                }

                var productImage = new ProductImage()
                {
                    Id = Guid.NewGuid(),
                    ProductDetailId = item.ProductDetailId,
                    ImageUrl = "/images/" + item.ImageFile.FileName
                };
                await _dbContext.ProductImage.AddAsync(productImage);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> CreateMany(List<ProductImageVM> items)
        {
            try
            {
                var lstPi = new List<ProductImage>();
                foreach (var x in items)
                {
                    ProductImage productImage = new ProductImage()
                    {
                        Id = Guid.NewGuid(),
                        ProductDetailId = x.ProductDetailId,
                        ImageUrl = x.ImageUrl,
                    };
                    lstPi.Add(productImage);

                }
                await _dbContext.ProductImage.AddRangeAsync(lstPi);
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
                var idp = await _dbContext.ProductImage.FindAsync(id);
                _dbContext.Remove(idp);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<ProductImageVM>> GetAll()
        {
            var lstim = from a in _dbContext.ProductImage
                        join b in _dbContext.ProductDetail on a.ProductDetailId equals b.Id
                        join c in _dbContext.Product on b.ProductId equals c.Id
                        select new ProductImageVM()
                        {
                            Id = a.Id,
                            ProductDetailId = b.Id,
                            ProductName = c.ProductName,
                            ImageUrl = a.ImageUrl
                        };
            return lstim.ToList();
        }

        public async Task<ProductImage> GetItem(Guid id)
        {
            return await _dbContext.ProductImage.FindAsync(id);
        }

        public async Task<bool> Update(Guid id, ProductImageVM item)
        {
            var idp = await _dbContext.ProductImage.FindAsync(id);
            if (idp == null)
            {
                return false;
            }
            else
            {
                idp.ProductDetailId = item.ProductDetailId;
                idp.ImageUrl = item.ImageUrl;
                _dbContext.Update(idp);
                await _dbContext.SaveChangesAsync();
                return true;
            }
        }
    }
}
