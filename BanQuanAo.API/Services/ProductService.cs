using BanQuanAo.API.Data;
using BanQuanAo.API.IServices;
using BanQuanAo.Share.Models;
using BanQuanAo.Share.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BanQuanAo.API.Services
{
    public class ProductService : IProductService
    {
        private MyDbContext _dbContext;
        // private cate, bran, mate
        public ProductService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Create(ProductVM item)
        {


            try
            {
                Product pr = new Product()
                {
                    Id = Guid.NewGuid(),
                    CategoryId = item.CategoryId,
                    BrandId = item.BrandId,
                    MaterialId = item.MaterialId,
                    ProductName = item.ProductName,
                    AvailableQuantity = item.AvailableQuantity,
                    Create_At = item.Create_At,
                    Update_At = item.Update_At,
                    Description = item.Description,
                    Long_Description = item.Long_Description,
                    Status = item.Status,
                };
                await _dbContext.Product.AddAsync(pr);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> CreateMany(List<ProductVM> items)
        {
            try
            {
                var lstP = new List<Product>();
                foreach (var x in items)
                {
                    Product pr = new Product()
                    {
                        Id = Guid.NewGuid(),
                        CategoryId = x.CategoryId,
                        BrandId = x.BrandId,
                        MaterialId = x.MaterialId,
                        ProductName = x.ProductName,
                        AvailableQuantity = x.AvailableQuantity,
                        Create_At = x.Create_At,
                        Update_At = x.Update_At,
                        Description = x.Description,
                        Long_Description = x.Long_Description,
                        Status = x.Status,
                    };
                    lstP.Add(pr);

                }
                await _dbContext.Product.AddRangeAsync(lstP);
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
                var idp = await _dbContext.Product.FindAsync(id);
                _dbContext.Remove(idp);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            return await _dbContext.Product.ToListAsync();
        }

        public async Task<IEnumerable<ProductVM>> GetAllVM()
        {
            #region join prd
            var lstPrd = from a in _dbContext.ProductDetail
                         join b in _dbContext.Size on a.SizeId equals b.Id
                         join c in _dbContext.Colors on a.ColorId equals c.ColorId
                         join d in _dbContext.Product on a.ProductId equals d.Id
                         select new ProductDetailVM
                         {
                             Id = a.Id,
                             SizeName = b.SizeName,
                             ColorName = c.ColorName,
                             ColorCode = c.ColorCode,
                             Quantity = a.Quantity,
                             Price = a.Price,
                             PriceSale = a.PriceSale,
                             Create_At = a.Create_At,
                             Update_At = a.Update_At,
                             Description = a.Description,
                             Status = a.Status,
                             ColorId = c.ColorId,
                             SizeId = b.Id,
                             ProductId = d.Id,
                             ProductName = d.ProductName
                         };
            #endregion
            var productDetail = await _dbContext.ProductDetail.ToListAsync();
            var lstPr = from a in _dbContext.ProductDetail
                        join b in _dbContext.Product on a.ProductId equals b.Id
                        join c in _dbContext.Brand on b.BrandId equals c.Id
                        join d in _dbContext.Category on b.CategoryId equals d.Id
                        join e in _dbContext.Material on b.MaterialId equals e.Id
                        select new ProductVM()
                        {
                            Id = b.Id,
                            BrandId = b.BrandId,
                            BrandName = c.BrandName,
                            CategoryId = b.CategoryId,
                            CategoryName = d.CategoryName,
                            MaterialId = b.MaterialId,
                            MaterialName = e.MaterialName,
                            ProductName = b.ProductName,
                            AvailableQuantity = b.AvailableQuantity,
                            Create_At = b.Create_At,
                            Update_At = b.Update_At,
                            Description = b.Description,
                            Long_Description = b.Long_Description,
                            Status = b.Status,
                            ProductDvms = productDetail
                        };
            return lstPr.ToList();
        }

        public async Task<Product> GetItem(Guid id)
        {
            return await _dbContext.Product.FindAsync(id);
        }

        public async Task<bool> Update(Guid id, ProductVM item)
        {
            var idp = await _dbContext.Product.FindAsync(id);
            if (idp == null)
            {
                return false;
            }
            else
            {
                idp.CategoryId = item.CategoryId;
                idp.BrandId = item.BrandId;
                idp.MaterialId = item.MaterialId;
                //idp.ProductName = item.ProductName;
                //idp.AvailableQuantity = item.AvailableQuantity;
                //idp.Create_At = item.Create_At;
                idp.Update_At = item.Update_At;
                idp.Description = item.Description;
                idp.Long_Description = item.Long_Description;
                idp.Status = item.Status;
                _dbContext.Update(idp);
                await _dbContext.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> UpdateSLTheoSPCT()
        {
            try
            {
                var products = await _dbContext.Product.ToListAsync();

                foreach (var product in products)
                {
                    var productDetails = await _dbContext.ProductDetail
                        .Where(pd => pd.ProductId == product.Id)
                        .ToListAsync();

                    // Tính tổng số lượng của tất cả sản phẩm chi tiết
                    int? totalQuantity = productDetails.Sum(pd => pd.Quantity);

                    // Cập nhật số lượng sản phẩm chính
                    product.AvailableQuantity = totalQuantity;

                    // Cập nhật vào cơ sở dữ liệu
                    _dbContext.Product.Update(product);
                }

                // Lưu thay đổi vào cơ sở dữ liệu
                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                // Xử lý exception tại đây (log, throw, ...)
                return false;
            }
        }

        public async Task<bool> ChangeStatusAsync(Guid idsp, bool status)
        {
            try
            {
                var sp = await _dbContext.Product.FindAsync(idsp);
                sp.Status = status;
                _dbContext.Product.Update(sp);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public async Task<bool> Updatetatus(Guid idsp)
        {
            try
            {
                var product = await _dbContext.Product.FindAsync(idsp);
                var spct = await _dbContext.ProductDetail.Where(c => c.ProductId == idsp).ToListAsync();
                if (product.Status == false) // nếu product status = false thì status
                {
                    foreach (var item in spct)
                    {
                        item.Status = false;
                        _dbContext.ProductDetail.Update(item);
                        await _dbContext.SaveChangesAsync();
                    }
                }
                int count = 0;
                foreach (var item in spct) // nếu all spct status false thì change status sp
                {
                    if (item.Status == false)
                    {
                        count++;
                    }
                }
                if (spct.Count == count)
                {
                    product.Status = false;
                    _dbContext.Product.Update(product);
                    await _dbContext.SaveChangesAsync();
                }
                if (spct.Count > count)
                {
                    product.Status = true;
                    _dbContext.Product.Update(product);
                    await _dbContext.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
