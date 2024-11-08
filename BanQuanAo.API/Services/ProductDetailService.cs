using BanQuanAo.API.Common;
using BanQuanAo.API.Data;
using BanQuanAo.API.IServices;
using BanQuanAo.Share.Models;
using BanQuanAo.Share.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Runtime.Intrinsics.Arm;
using static System.Net.Mime.MediaTypeNames;

namespace BanQuanAo.API.Services
{
    public class ProductDetailService : IProductDetailService
    {
        private MyDbContext _dbContext;
        private readonly IStorageService _storageService;
        public ProductDetailService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        //luu anh vao thu muc wwwroot o api

        public async Task<bool> Create(ProductDetailVM item)
        {
            try
            {

                ProductDetail productDetail = new ProductDetail()
                {
                    Id = Guid.NewGuid(),
                    ProductId = item.ProductId,
                    SizeId = item.SizeId,
                    ColorId = item.ColorId,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    PriceSale = item.PriceSale,
                    Create_At = item.Create_At,
                    Update_At = item.Update_At,
                    Description = item.Description,
                    Status = item.Status,
                };

                await _dbContext.ProductDetail.AddAsync(productDetail);
                await _dbContext.SaveChangesAsync();
                // Cập nhật AvailableQuantity trong Product
                Product product = await _dbContext.Product.FindAsync(item.ProductId);
                if (product != null)
                {
                    product.AvailableQuantity += item.Quantity;
                    _dbContext.Product.Update(product);
                    await _dbContext.SaveChangesAsync();
                }
                //save image
                // HashSet để theo dõi các tên file đã xử lý
                var processedFiles = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                if (item.filecollection != null)//không null 
                {
                    foreach (var i in item.filecollection)
                    {
                        // Bỏ qua file này nếu đã xử lý
                        if (!processedFiles.Add(i.FileName))
                        {
                            continue;
                        }
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                        //create folder if not exist
                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);
                        string imgPath = Path.Combine(path, i.FileName);
                        using (var stream = new FileStream(imgPath, FileMode.Create))
                        {
                            await i.CopyToAsync(stream);
                        }
                        var proi = new ProductImage()
                        {
                            Id = Guid.NewGuid(),
                            ProductDetailId = productDetail.Id,
                            ImageUrl = "/images/" + i.FileName
                        };
                        _dbContext.ProductImage.Add(proi);
                    }
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

        public async Task<bool> CreateMany(List<ProductDetailVM> items)
        {
            try
            {
                var LstProductDetail = new List<ProductDetail>();
                foreach (var x in items)
                {
                    ProductDetail productDetail = new ProductDetail()
                    {
                        Id = Guid.NewGuid(),
                        ProductId = x.ProductId,
                        SizeId = x.SizeId,
                        ColorId = x.ColorId,
                        Quantity = x.Quantity,
                        Price = x.Price,
                        PriceSale = x.PriceSale,
                        Create_At = x.Create_At,
                        Update_At = x.Update_At,
                        Description = x.Description,
                        Status = x.Status,
                    };
                    LstProductDetail.Add(productDetail);
                }
                await _dbContext.ProductDetail.AddRangeAsync(LstProductDetail);
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
                var productDt = await _dbContext.ProductDetail.FindAsync(id) ?? throw new Exception("Không tìm thấy sản phẩm");
                var images = _dbContext.ProductImage.Where(i => i.ProductDetailId == id);
                _dbContext.ProductImage.RemoveRange(images);
                await _dbContext.SaveChangesAsync();
                _dbContext.ProductDetail.Remove(productDt);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<ProductDetailVM>> GetAll()
        {
            var lst = from a in _dbContext.ProductDetail
                      join b in _dbContext.Size on a.SizeId equals b.Id
                      join c in _dbContext.Colors on a.ColorId equals c.ColorId
                      join d in _dbContext.Product on a.ProductId equals d.Id
                      select new ProductDetailVM
                      {
                          Id = a.Id,
                          ProductName = d.ProductName,
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
                          ProductId = d.Id,
                          ColorId = c.ColorId,
                          SizeId = b.Id
                      };
            return lst.ToList();
        }

        public async Task<ProductDetailVM> GetItem(Guid id)
        {
            #region
            var prD = await _dbContext.ProductDetail.FindAsync(id);
            var product = _dbContext.Product.FirstOrDefault(x => x.Id == prD.ProductId);
            var size = _dbContext.Size.FirstOrDefault(x => x.Id == prD.SizeId);
            var color = _dbContext.Colors.FirstOrDefault(x => x.ColorId == prD.ColorId);
            var img = (from a in _dbContext.ProductImage
                       join b in _dbContext.ProductDetail on a.ProductDetailId equals b.Id
                       where a.ProductDetailId == prD.Id
                       select a.ImageUrl).ToList();//khó hiểu, dùng asyn k tìm được, dùng 1 luồng lại được ???
            var lstPrd = new ProductDetailVM()
            {
                Id = id,
                Images = img,
                ProductId = prD.ProductId,
                ProductName = product.ProductName,
                SizeId = prD.SizeId,
                SizeName = size.SizeName,
                ColorId = prD.ColorId,
                ColorName = color.ColorName,
                ColorCode = color.ColorCode,
                Quantity = prD.Quantity,
                Price = prD.Price,
                PriceSale = prD.PriceSale,
                Create_At = prD.Create_At,
                Update_At = prD.Update_At,
                Description = prD.Description,
                Status = prD.Status
            };
            return lstPrd;
            #endregion

        }
        public async Task<bool> Update(ProductDetailVM item)
        {
            // Tìm sản phẩm chi tiết theo Id
            var productDetail = await _dbContext.ProductDetail.FindAsync(item.Id);

            // Kiểm tra xem sản phẩm chi tiết có tồn tại không
            if (productDetail == null)
            {
                throw new Exception($"Không tìm thấy sản phẩm có id: {item.Id}");
            }

            // Lấy sự thay đổi trong Quantity
            int? quantityChange = item.Quantity - productDetail.Quantity;
            //Nếu quantityChange là dương, là  thêm vào Quantity, và nếu quantityChange là âm, là  đang giảm Quantity.
            productDetail.SizeId = item.SizeId;
            productDetail.ColorId = item.ColorId;
            productDetail.Quantity = item.Quantity;
            //productDetail.Price = item.Price;
            productDetail.PriceSale = item.PriceSale;
            productDetail.Update_At = item.Update_At;
            productDetail.Description = item.Description;
            productDetail.Status = item.Status;

            // Xóa hết các hình ảnh liên quan đến sản phẩm chi tiết
            //var images = _dbContext.ProductImage.Where(i => i.ProductDetailId == productDetail.Id);
            //_dbContext.ProductImage.RemoveRange(images);

            // Lưu lại thông tin hình ảnh mới (nếu có)
            var processedFiles = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (item.filecollection != null)
            {
                foreach (var file in item.filecollection)
                {
                    if (!processedFiles.Add(file.FileName))
                    {
                        continue;
                    }
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                    // Tạo thư mục nếu nó chưa tồn tại
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    // Lưu file vào thư mục
                    var imgPath = Path.Combine(path, file.FileName);
                    using (var stream = new FileStream(imgPath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    // Tạo đối tượng ProductImage mới và thêm vào danh sách
                    var productImage = new ProductImage()
                    {
                        ProductDetailId = productDetail.Id,
                        ImageUrl = "/images/" + file.FileName
                    };
                    _dbContext.ProductImage.Update(productImage);
                }
                _dbContext.Update(productDetail);
            }
            await _dbContext.SaveChangesAsync();

            // Cập nhật AvailableQuantity trong Product
            var product = await _dbContext.Product.FindAsync(productDetail.ProductId);
            if (product != null)
            {
                product.AvailableQuantity += quantityChange;
                _dbContext.Product.Update(product);
                await _dbContext.SaveChangesAsync();
            }

            return true;
        }


        public decimal GetPriceForProductDetail(Guid productId, Guid sizeId, Guid colorId)
        {
            // Tìm sản phẩm chi tiết dựa trên productId và sizeId color
            var productDetail = _dbContext.ProductDetail
            .FirstOrDefault(pd => pd.ProductId == productId && pd.SizeId == sizeId && pd.ColorId == colorId);
            if (productDetail != null && productDetail.PriceSale.HasValue)
            {
                return productDetail.PriceSale.Value;
            }
            throw new Exception("Không tìm thấy giá sản phẩm chi tiết/ chưa có size,color này.");
        }
        public ProductDetail GetProductDetail(Guid productId, Guid sizeId, Guid colorId)
        {
            // Tìm sản phẩm chi tiết dựa trên productId và sizeId color
            var productDetail = _dbContext.ProductDetail
            .FirstOrDefault(pd => pd.ProductId == productId && pd.SizeId == sizeId && pd.ColorId == colorId);
            if (productDetail != null && productDetail.PriceSale.HasValue)
            {
                return productDetail;
            }
            return null;
        }

        public async Task<bool> UpdateQuantityById(Guid productDetailId, int quantity)
        {
            try
            {
                var productDetail = _dbContext.ProductDetail.FirstOrDefault(c => c.Id == productDetailId);
                productDetail.Quantity -= quantity;
                _dbContext.ProductDetail.Update(productDetail);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }

        public async Task<bool> UpdateQuantityOrderFail(Guid productDetailId, int quantity)
        {
            try
            {
                var productDetail = _dbContext.ProductDetail.FirstOrDefault(c => c.Id == productDetailId);
                productDetail.Quantity += quantity;
                _dbContext.ProductDetail.Update(productDetail);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public List<ProductImageVM> GetAllAnhSanPham(Guid idSanPham)
        {
            try
            {
                var lst = (from a in _dbContext.ProductImage.Where(x => x.ProductDetailId == idSanPham)
                           select new ProductImageVM()
                           {
                               Id = a.Id,
                               ProductDetailId = a.ProductDetailId,
                               ImageUrl = a.ImageUrl
                           }).ToList();
                return lst;
            }
            catch
            {
                return new List<ProductImageVM>();
            }
        }
        public async Task<bool> ChangeStatusAsync(Guid idspct, bool status)
        {
            try
            {
                var spct = await _dbContext.ProductDetail.FindAsync(idspct);
                spct.Status = status;
                _dbContext.ProductDetail.Update(spct);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

    }

}
