using BanQuanAo.Share.Models;
using BanQuanAo.Share.ViewModels;

namespace BanQuanAo.API.IServices
{
    public interface IProductImageService
    {
        public Task<bool> Create(ProductImageVM item);

        public Task<bool> CreateMany(List<ProductImageVM> items);

        public Task<bool> Delete(Guid id);

        //public Task<bool> DeleteMany(List<> items);

        public Task<IEnumerable<ProductImageVM>> GetAll();

        public Task<ProductImage> GetItem(Guid id);

        public Task<bool> Update(Guid id, ProductImageVM item);
    }
}
