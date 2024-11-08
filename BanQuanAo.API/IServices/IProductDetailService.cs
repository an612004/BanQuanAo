using BanQuanAo.Share.Models;
using BanQuanAo.Share.ViewModels;

namespace BanQuanAo.API.IServices
{
    public interface IProductDetailService
    {
        public Task<bool> Create(ProductDetailVM item);

        public Task<bool> CreateMany(List<ProductDetailVM> items);

        public Task<bool> Delete(Guid id);

        //public Task<bool> DeleteMany(List<> items);

        public Task<IEnumerable<ProductDetailVM>> GetAll();
        List<ProductImageVM> GetAllAnhSanPham(Guid idSanPham);
        public Task<ProductDetailVM> GetItem(Guid id);
        public Task<bool> UpdateQuantityById(Guid productDetailId, int quantity);
        public Task<bool> UpdateQuantityOrderFail(Guid productDetailId, int quantity);

        public Task<bool> Update(ProductDetailVM item);
        public decimal GetPriceForProductDetail(Guid productId, Guid sizeId, Guid colorId);
        public ProductDetail GetProductDetail(Guid productId, Guid sizeId, Guid colorId);
        public Task<bool> ChangeStatusAsync(Guid idspct, bool status);
    }
}
