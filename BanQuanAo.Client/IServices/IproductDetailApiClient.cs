using BanQuanAo.Share.Models;
using BanQuanAo.Share.ViewModels;

namespace BanQuanAo.Client.IServices
{
    public interface IproductDetailApiClient
    {
        Task<bool> CreateProduct(ProductDetailVM request, Guid productId, Guid sizeId, Guid colorId);
        Task<bool> UpdateProduct(ProductDetailVM request, Guid sizeId, Guid colorId);
        Task<bool> DeleteProductDetail(Guid request);
        Task<bool> DeleteProducImage(Guid request);
        Task<ProductDetailVM> GetByIdProductDetail(Guid productDetailId);
        Task<List<ProductDetailVM>> GetAllProductDetail();
        Task<List<ProductVM>> GetListProduct();
        Task<List<Colors>> GetListColor();
        Task<List<SizeVM>> GetListSize();
        Task<List<ProductImage>> GetListProI();
    }
}
